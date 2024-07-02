using System.Text.RegularExpressions;

namespace KaddaOK.Library
{
    public class KnownOriginalLyrics
    {
        private static readonly string unguardedDashRegex = "(?<=[0-9A-Za-z])(?<!\\|)-(?!\\|)(?=[0-9A-Za-z])";
        private static readonly string unguardedDashReplacement = "/-";

        public List<string>? SeparatorCleansedLines { get; }
        public List<string>? UncleansedLines { get; }

        private KnownOriginalLyrics(string? lyrics)
        {
            UncleansedLines = lyrics?
                .Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.RemoveEmptyEntries)
                .Select(s => AdjustSeparators(s.Trim()))
                .ToList()
                ?? new List<string>();
            SeparatorCleansedLines = UncleansedLines.Select(s => s.Replace("|", "").Replace("/", "")).ToList();
        }

        private string AdjustSeparators(string input)
        {
            return Regex.Replace(input, unguardedDashRegex, unguardedDashReplacement);
        }
        public static KnownOriginalLyrics FromText(string? text)
        {
            return new KnownOriginalLyrics(text);
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

        public List<string>? DistinctLines => SeparatorCleansedLines?.Distinct().ToList();

        public string? DistinctLinesAsText => DistinctLines != null ? string.Join(Environment.NewLine, DistinctLines) : null;

        public List<string>? LoweredLines => DistinctLines?.Select(s => s.ToLowerInvariant()).ToList();
    }
}
