using System;
using System.IO;
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
        void Load(KaraokeProcess process, string filePath);
        string? GetAutoSaveFilePath(KaraokeProcess process);
        void AutoSave(KaraokeProcess process);
    }

    public class KoktProjectService : IKoktProjectService
    {
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

        public void Load(KaraokeProcess process, string filePath)
        {
            var json = File.ReadAllText(filePath);
            var envelope = JObject.Parse(json);
            var processToken = envelope["Process"];
            if (processToken == null)
            {
                throw new InvalidOperationException("Invalid project file: missing Process data.");
            }

            using var reader = processToken.CreateReader();
            Serializer.Populate(reader, process);
            process.ProjectFilePath = filePath;

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
