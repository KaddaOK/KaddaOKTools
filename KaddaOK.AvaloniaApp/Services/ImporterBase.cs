using System;
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
        protected async Task LoadAudioIntoProcess(KaraokeProcess karaokeProcess, string audioFilePath)
        {
            string? originalAudioPath = null;
            string? vocalsOnlyAudioPath = null;
            string? instrumentalAudioPath = null;
            // first test the path and if it can't be reached, throw out
            if (!Path.Exists(audioFilePath))
            {
                throw new InvalidOperationException($"Could not access path '{audioFilePath}'");
            }

            var pathFolder = Path.GetDirectoryName(audioFilePath);
            var otherFilesInFolder = Directory.EnumerateFiles(pathFolder ?? "", "*.flac")
                .Union(Directory.EnumerateFiles(pathFolder ?? "", "*.wav"))
                .Where(p => p.ToLowerInvariant() != audioFilePath.ToLowerInvariant())
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
                        && audioFilePath.Contains(Path.GetFileNameWithoutExtension(f), StringComparison.InvariantCultureIgnoreCase))
                    .ToList();
                if (possibleOriginals.Count > 1)
                {
                    // TODO: rank them by similarity?
                }

                return possibleOriginals.FirstOrDefault();
            }

            // if it has "(Vocals)." in it, assume it's the vocal-only and look for instrumental and none
            if (audioFilePath.Contains(vocalSearchPattern, StringComparison.InvariantCultureIgnoreCase))
            {
                vocalsOnlyAudioPath = audioFilePath;
                instrumentalAudioPath = findBestMatchPart(instrumentalSearchPattern);
                originalAudioPath = findOriginalAudio();
            }
            // if it has "(Instrumental)." in it, assume it's the instrumental-only and look for vocals and none
            else if (audioFilePath.Contains(instrumentalSearchPattern, StringComparison.InvariantCultureIgnoreCase))
            {
                instrumentalAudioPath = audioFilePath;
                vocalsOnlyAudioPath = findBestMatchPart(vocalSearchPattern);
                originalAudioPath = findOriginalAudio();
            }
            // otherwise, assume it's the original and look for vocals and instrumental
            else
            {
                originalAudioPath = audioFilePath;
                instrumentalAudioPath = findBestMatchPart(instrumentalSearchPattern);
                vocalsOnlyAudioPath = findBestMatchPart(vocalSearchPattern);
            }

            // fill in any that we found
            if (!string.IsNullOrWhiteSpace(originalAudioPath))
            {
                karaokeProcess.UnseparatedAudioFilePath = originalAudioPath;
                karaokeProcess.UnseparatedAudioStream = AudioFileReader.GetAudioFromFile(originalAudioPath);
                karaokeProcess.UnseparatedAudioFloats = await Sampler.GetAllFloatsAsync(karaokeProcess.UnseparatedAudioStream, dataSamplingFactor);
            }
            if (!string.IsNullOrWhiteSpace(vocalsOnlyAudioPath))
            {
                karaokeProcess.VocalsAudioFilePath = vocalsOnlyAudioPath;
                karaokeProcess.VocalsAudioStream = AudioFileReader.GetAudioFromFile(vocalsOnlyAudioPath);
                karaokeProcess.VocalsAudioFloats = await Sampler.GetAllFloatsAsync(karaokeProcess.VocalsAudioStream, dataSamplingFactor);
            }
            if (!string.IsNullOrWhiteSpace(instrumentalAudioPath))
            {
                karaokeProcess.InstrumentalAudioFilePath = instrumentalAudioPath;
                karaokeProcess.InstrumentalAudioStream = AudioFileReader.GetAudioFromFile(instrumentalAudioPath);
                karaokeProcess.InstrumentalAudioFloats = await Sampler.GetAllFloatsAsync(karaokeProcess.InstrumentalAudioStream, dataSamplingFactor);
            }
        }
    }
}
