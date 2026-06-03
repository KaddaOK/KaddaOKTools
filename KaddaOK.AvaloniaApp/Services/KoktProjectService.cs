using System;
using System.IO;
using System.Threading.Tasks;
using Avalonia.Media;
using KaddaOK.AvaloniaApp.Models;
using KaddaOK.Library;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace KaddaOK.AvaloniaApp.Services
{
    public interface IKoktProjectService
    {
        void Save(KaraokeProcess process, string filePath);
        Task<AllAudioFilePathsResult> Load(KaraokeProcess process, string projectFilePath, string? correctedOriginalAudioFilePath = null, string? correctedVocalAudioFilePath = null, string? correctedInstrumentalAudioFilePath = null);
        string? GetAutoSaveFilePath(KaraokeProcess process);
        void AutoSave(KaraokeProcess process);
    }

    public class KoktProjectService : IKoktProjectService
    {
        private readonly int dataSamplingFactor = 20;

        protected IAudioFromFile AudioFileReader { get; }
        protected IMinMaxFloatWaveStreamSampler Sampler { get; }

        public KoktProjectService(IAudioFromFile audioFileReader, IMinMaxFloatWaveStreamSampler sampler)
        {
            AudioFileReader = audioFileReader;
            Sampler = sampler;
        }

        private static readonly JsonSerializer Serializer = JsonSerializer.Create(new JsonSerializerSettings
        {
            Formatting = Formatting.Indented,
            NullValueHandling = NullValueHandling.Ignore,
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
            Converters = { new AvaloniaColorJsonConverter(), new KnownOriginalLyricsJsonConverter() }
        });

        public void Save(KaraokeProcess process, string filePath)
        {
            var envelope = new JObject
            {
                ["FormatVersion"] = 1,
                ["SavedAtUtc"] = DateTime.UtcNow,
                ["Process"] = JObject.FromObject(process, Serializer)
            };
            File.WriteAllText(filePath, envelope.ToString(Formatting.Indented));
            process.ProjectFilePath = filePath;
            process.HasUnsavedChanges = false;
        }

        public async Task<AllAudioFilePathsResult> Load(KaraokeProcess process, string projectFilePath, string? correctedOriginalAudioFilePath = null, string? correctedVocalAudioFilePath = null, string? correctedInstrumentalAudioFilePath = null)
        {
            var json = File.ReadAllText(projectFilePath);
            var envelope = JObject.Parse(json);
            var processToken = envelope["Process"];
            if (processToken == null)
            {
                throw new InvalidOperationException("Invalid project file: missing Process data.");
            }

            using var reader = processToken.CreateReader();
            Serializer.Populate(reader, process);
            process.ProjectFilePath = projectFilePath;

            // Fix up InPossibilities back-references (not serialized to avoid circular refs)
            if (process.DetectedLinePossibilities != null)
            {
                foreach (var lp in process.DetectedLinePossibilities)
                {
                    foreach (var lyric in lp.Lyrics)
                    {
                        lyric.InPossibilities = lp;
                    }
                }
            }

            process.HasUnsavedChanges = false;

            // Try to load the Stream and Floats for each type of audio file present;
            // return the result of doing so to the client so we can make them find the file if it's missing.
            return await LoadAudio(process, correctedOriginalAudioFilePath, correctedVocalAudioFilePath, correctedInstrumentalAudioFilePath);
        }

        public async Task<AllAudioFilePathsResult> LoadAudio(KaraokeProcess process, string? correctUnseparatedAudioFilePath = null, string? correctVocalAudioFilePath = null, string? correctInstrumentalAudioFilePath = null)
        {
            var result = new AllAudioFilePathsResult();

            var unseparatedAudioPath = correctUnseparatedAudioFilePath ?? process.UnseparatedAudioFilePath;


            try 
            {
                if (!string.IsNullOrWhiteSpace(unseparatedAudioPath) && File.Exists(unseparatedAudioPath))
                {
                    process.UnseparatedAudioFilePath = unseparatedAudioPath;
                    result.UnseparatedAudioFilePath = unseparatedAudioPath;
                    process.UnseparatedAudioStream = AudioFileReader.GetAudioFromFile(unseparatedAudioPath);
                    process.UnseparatedAudioFloats = await Sampler.GetAllFloatsAsync(process.UnseparatedAudioStream, dataSamplingFactor);
                    result.UnseparatedAudioLoaded = true;
                }
            }
            catch (Exception ex)
            {
                result.UnseparatedAudioLoaded = false;
                result.Errors.Add(new Exception($"Failed to load unseparated audio from path '{unseparatedAudioPath}'.", ex));
            }

            var vocalAudioPath = correctVocalAudioFilePath ?? process.VocalsAudioFilePath;
            try
            {
                if (!string.IsNullOrWhiteSpace(vocalAudioPath) && File.Exists(vocalAudioPath))
                {
                    process.VocalsAudioFilePath = vocalAudioPath;
                    result.VocalsAudioFilePath = vocalAudioPath;
                    process.VocalsAudioStream = AudioFileReader.GetAudioFromFile(vocalAudioPath);
                    process.VocalsAudioFloats = await Sampler.GetAllFloatsAsync(process.VocalsAudioStream, dataSamplingFactor);
                    result.VocalsAudioLoaded = true;
                }
            }
            catch (Exception ex)
            {
                result.VocalsAudioLoaded = false;
                result.Errors.Add(new Exception($"Failed to load vocal audio from path '{vocalAudioPath}'.", ex));
            }

            var instrumentalAudioPath = correctInstrumentalAudioFilePath ?? process.InstrumentalAudioFilePath;
            try 
            {
                 if (!string.IsNullOrWhiteSpace(instrumentalAudioPath) && File.Exists(instrumentalAudioPath))
                {
                    process.InstrumentalAudioFilePath = instrumentalAudioPath;
                    result.InstrumentalAudioFilePath = instrumentalAudioPath;
                    process.InstrumentalAudioStream = AudioFileReader.GetAudioFromFile(instrumentalAudioPath);
                    process.InstrumentalAudioFloats = await Sampler.GetAllFloatsAsync(process.InstrumentalAudioStream, dataSamplingFactor);
                    result.InstrumentalAudioLoaded = true;
                }
            }
            catch (Exception ex)
            {
                result.InstrumentalAudioLoaded = false;
                result.Errors.Add(new Exception($"Failed to load instrumental audio from path '{instrumentalAudioPath}'.", ex));
            }

            return result;
        }

        public void AutoSave(KaraokeProcess process)
        {
            var filePath = GetAutoSaveFilePath(process);
            if (!string.IsNullOrWhiteSpace(filePath))
            {
                Save(process, filePath);
            }
        }

        public string? GetAutoSaveFilePath(KaraokeProcess process)
        {
            if (!string.IsNullOrWhiteSpace(process.ProjectFilePath))
            {
                return process.ProjectFilePath;
            }

            string? basePath = null;

            switch (process.KaraokeSource)
            {
                case InitialKaraokeSource.RzlrcImport:
                case InitialKaraokeSource.KbpImport:
                    basePath = process.ImportedKaraokeSourceFilePath;
                    break;

                case InitialKaraokeSource.ManualSync:
                case InitialKaraokeSource.AzureSpeechService:
                case InitialKaraokeSource.CtmImport:
                    basePath = process.UnseparatedAudioFilePath
                               ?? process.InstrumentalAudioFilePath
                               ?? process.VocalsAudioFilePath;
                    break;
            }

            if (string.IsNullOrWhiteSpace(basePath))
            {
                return null;
            }

            var directory = Path.GetDirectoryName(basePath);
            var fileNameWithoutExtension = Path.GetFileNameWithoutExtension(basePath);
            return Path.Combine(directory ?? "", fileNameWithoutExtension + ".koktpj");
        }
    }

    internal class AvaloniaColorJsonConverter : JsonConverter<Color>
    {
        public override void WriteJson(JsonWriter writer, Color value, JsonSerializer serializer)
        {
            writer.WriteValue($"#{value.R:X2}{value.G:X2}{value.B:X2}");
        }

        public override Color ReadJson(JsonReader reader, Type objectType, Color existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            var hex = reader.Value as string;
            if (string.IsNullOrWhiteSpace(hex)) return existingValue;
            hex = hex.TrimStart('#');
            if (hex.Length >= 6)
            {
                var r = Convert.ToByte(hex.Substring(0, 2), 16);
                var g = Convert.ToByte(hex.Substring(2, 2), 16);
                var b = Convert.ToByte(hex.Substring(4, 2), 16);
                return Color.FromRgb(r, g, b);
            }
            return existingValue;
        }
    }

    internal class KnownOriginalLyricsJsonConverter : JsonConverter<KnownOriginalLyrics>
    {
        public override void WriteJson(JsonWriter writer, KnownOriginalLyrics? value, JsonSerializer serializer)
        {
            if (value?.UncleansedLines != null)
                writer.WriteValue(string.Join("\n", value.UncleansedLines));
            else
                writer.WriteNull();
        }

        public override KnownOriginalLyrics? ReadJson(JsonReader reader, Type objectType, KnownOriginalLyrics? existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            var text = reader.Value as string;
            return text != null ? KnownOriginalLyrics.FromText(text) : null;
        }
    }
}