using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using KaddaOK.AvaloniaApp.Models;
using KaddaOK.Library;
using NAudio.Wave;

namespace KaddaOK.AvaloniaApp.Services
{
    public abstract class Importer
    {
        private readonly int dataSamplingFactor = 20;

        protected IAudioFromFile AudioFileReader { get; }
        protected IMinMaxFloatWaveStreamSampler Sampler { get; }

        protected Importer(IAudioFromFile audioFileReader, IMinMaxFloatWaveStreamSampler sampler)
        {
            AudioFileReader = audioFileReader;
            Sampler = sampler;
        }

        protected readonly string vocalSearchPattern = "(Vocals).";
        protected readonly string instrumentalSearchPattern = "(Instrumental).";
        public async Task<SingleAudioFilePathResult> LoadAudioIntoProcess(KaraokeProcess karaokeProcess, string singleAudioFilePathOfUnknownType)
        {
            string? originalAudioPath = null;
            string? vocalsOnlyAudioPath = null;
            string? instrumentalAudioPath = null;

            // first test the path and if it can't be reached, throw out
            if (!Path.Exists(singleAudioFilePathOfUnknownType))
            {
                return new SingleAudioFilePathResult
                {
                    AudioFilePath = singleAudioFilePathOfUnknownType,
                    AudioLoaded = false,
                    Errors = new List<Exception> { new FileNotFoundException($"Couldn't find the audio file at {singleAudioFilePathOfUnknownType}") }
                };
            }

            var pathFolder = Path.GetDirectoryName(singleAudioFilePathOfUnknownType);
            var otherFilesInFolder = Directory.EnumerateFiles(pathFolder ?? "", "*.flac")
                .Union(Directory.EnumerateFiles(pathFolder ?? "", "*.wav"))
                .Where(p => p.ToLowerInvariant() != singleAudioFilePathOfUnknownType.ToLowerInvariant())
                .OrderBy(Path.GetExtension).ThenBy(p => p)
                .ToList();


            string? findBestMatchPart(string partSearchPattern)
            {
                var possibleParts = otherFilesInFolder
                    .Where(f => f.Contains(partSearchPattern, StringComparison.InvariantCultureIgnoreCase))
                    .ToList();
                if (possibleParts.Count > 1)
                {
                    // TODO: rank them by similarity?
                }

                return possibleParts.FirstOrDefault();
            }
            string? findOriginalAudio()
            {
                var possibleOriginals = otherFilesInFolder.Where(f =>
                        !f.Contains(instrumentalSearchPattern, StringComparison.InvariantCultureIgnoreCase)
                        && !f.Contains(vocalSearchPattern, StringComparison.InvariantCultureIgnoreCase)
                        && singleAudioFilePathOfUnknownType.Contains(Path.GetFileNameWithoutExtension(f), StringComparison.InvariantCultureIgnoreCase))
                    .ToList();
                if (possibleOriginals.Count > 1)
                {
                    // TODO: rank them by similarity?
                }

                return possibleOriginals.FirstOrDefault();
            }

            // if it has "(Vocals)." in it, assume it's the vocal-only and look for instrumental and none
            if (singleAudioFilePathOfUnknownType.Contains(vocalSearchPattern, StringComparison.InvariantCultureIgnoreCase))
            {
                vocalsOnlyAudioPath = singleAudioFilePathOfUnknownType;
                instrumentalAudioPath = findBestMatchPart(instrumentalSearchPattern);
                originalAudioPath = findOriginalAudio();
            }
            // if it has "(Instrumental)." in it, assume it's the instrumental-only and look for vocals and none
            else if (singleAudioFilePathOfUnknownType.Contains(instrumentalSearchPattern, StringComparison.InvariantCultureIgnoreCase))
            {
                instrumentalAudioPath = singleAudioFilePathOfUnknownType;
                vocalsOnlyAudioPath = findBestMatchPart(vocalSearchPattern);
                originalAudioPath = findOriginalAudio();
            }
            // otherwise, assume it's the original and look for vocals and instrumental
            else
            {
                originalAudioPath = singleAudioFilePathOfUnknownType;
                instrumentalAudioPath = findBestMatchPart(instrumentalSearchPattern);
                vocalsOnlyAudioPath = findBestMatchPart(vocalSearchPattern);
            }

            var allPathsResult = await LoadAudioIntoProcess(karaokeProcess, originalAudioPath, vocalsOnlyAudioPath, instrumentalAudioPath);
            var singlePathResult = new SingleAudioFilePathResult
            {
                Errors = allPathsResult.Errors
            };
            if (allPathsResult.InstrumentalAudioLoaded)
            {
                singlePathResult.AudioFilePath = allPathsResult.InstrumentalAudioFilePath;
                singlePathResult.AudioLoaded = true;
            }
            else if (allPathsResult.VocalsAudioLoaded)
            {
                singlePathResult.AudioFilePath = allPathsResult.VocalsAudioFilePath;
                singlePathResult.AudioLoaded = true;
            }
            else if (allPathsResult.UnseparatedAudioLoaded)
            {
                singlePathResult.AudioFilePath = allPathsResult.UnseparatedAudioFilePath;
                singlePathResult.AudioLoaded = true;
            }
            else
            {
                singlePathResult.AudioFilePath = null;
                singlePathResult.AudioLoaded = false;
            }
            return singlePathResult;
        }

        protected async Task<AllAudioFilePathsResult> LoadAudioIntoProcess(KaraokeProcess karaokeProcess, string? originalAudioPath, string? vocalsOnlyAudioPath, string? instrumentalAudioPath)
        {
            var result = new AllAudioFilePathsResult();

            // fill in any that we found
            if (!string.IsNullOrWhiteSpace(originalAudioPath))
            {
                karaokeProcess.UnseparatedAudioFilePath = originalAudioPath;
                result.UnseparatedAudioFilePath = originalAudioPath;
                try
                {
                    karaokeProcess.UnseparatedAudioStream = AudioFileReader.GetAudioFromFile(originalAudioPath);
                    karaokeProcess.UnseparatedAudioFloats = await Sampler.GetAllFloatsAsync(karaokeProcess.UnseparatedAudioStream, dataSamplingFactor);
                    result.UnseparatedAudioLoaded = true;
                }
                catch (Exception ex)
                {
                    result.UnseparatedAudioLoaded = false;
                    result.Errors.Add(new Exception($"Failed to load original audio from {originalAudioPath}: {ex.Message}", ex));
                }
            }
            if (!string.IsNullOrWhiteSpace(vocalsOnlyAudioPath))
            {
                karaokeProcess.VocalsAudioFilePath = vocalsOnlyAudioPath;
                result.VocalsAudioFilePath = vocalsOnlyAudioPath;
                try
                {
                    karaokeProcess.VocalsAudioStream = AudioFileReader.GetAudioFromFile(vocalsOnlyAudioPath);
                    karaokeProcess.VocalsAudioFloats = await Sampler.GetAllFloatsAsync(karaokeProcess.VocalsAudioStream, dataSamplingFactor);
                    result.VocalsAudioLoaded = true;
                }
                catch (Exception ex)
                {
                    result.VocalsAudioLoaded = false;
                    result.Errors.Add(new Exception($"Failed to load vocals-only audio from {vocalsOnlyAudioPath}: {ex.Message}", ex));
                }
            }
            if (!string.IsNullOrWhiteSpace(instrumentalAudioPath))
            {
                karaokeProcess.InstrumentalAudioFilePath = instrumentalAudioPath;
                result.InstrumentalAudioFilePath = instrumentalAudioPath;
                try
                {
                    karaokeProcess.InstrumentalAudioStream = AudioFileReader.GetAudioFromFile(instrumentalAudioPath);
                    karaokeProcess.InstrumentalAudioFloats = await Sampler.GetAllFloatsAsync(karaokeProcess.InstrumentalAudioStream, dataSamplingFactor);
                    result.InstrumentalAudioLoaded = true;
                }
                catch (Exception ex)
                {
                    result.InstrumentalAudioLoaded = false;
                    result.Errors.Add(new Exception($"Failed to load instrumental audio from {instrumentalAudioPath}: {ex.Message}", ex));
                }
            }
            return result;
        }

    }
}
