using System.Text.RegularExpressions;

namespace KaddaOK.Library
{
    public class KnownOriginalLyrics
    {
        public List<string>? Lyrics { get; }

        private KnownOriginalLyrics(string? lyrics)
        {
            Lyrics = lyrics?.Split(Environment.NewLine).Where(l => !string.IsNullOrWhiteSpace(l)).Select(l => l.Trim()).ToList() ?? new List<string>();
        }
        public static KnownOriginalLyrics FromText(string? text)
        {
            var lyrics = Regex.Replace(text?.Replace("/", "") ?? "", @"\r\n|\n\r|\n|\r", "\r\n");
            return new KnownOriginalLyrics(lyrics);
        }
        public static KnownOriginalLyrics FromFilePath(string? filePath)
        {
            if (string.IsNullOrWhiteSpace(filePath))
            {
                throw new ArgumentNullException(nameof(filePath), "Can't chop lyrics without a path to lyrics file!");
            }
            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException($"No file at path: {filePath}");
            }

            var lyrics = File.ReadAllText(filePath);
            return KnownOriginalLyrics.FromText(lyrics);
        }

        public List<string>? DistinctLines => Lyrics?.Distinct().ToList();

        public string? DistinctLinesAsText => DistinctLines != null ? string.Join(Environment.NewLine, DistinctLines) : null;

        public List<string>? LoweredLines => DistinctLines?.Select(s => s.ToLowerInvariant()).ToList();
    }
}
