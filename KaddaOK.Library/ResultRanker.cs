using Microsoft.CognitiveServices.Speech;

namespace KaddaOK.Library
{
    public interface IResultRanker
    {
        (int matchedChars, int unmatchedChars) GetRankByPhrases(LyricLine result, List<string> lowerLines);
        (int matchedChars, int unmatchedChars) GetRankByPhrases(DetailedSpeechRecognitionResult result, List<string>? lowerLines);
        DetailedSpeechRecognitionResult? GetBestByPhrases(IEnumerable<DetailedSpeechRecognitionResult> results,
            double offset, List<string> lowerLines);
    }
    public class ResultRanker : IResultRanker
    {
        public (int matchedChars, int unmatchedChars) GetRankByPhrases(DetailedSpeechRecognitionResult result, List<string>? lowerLines)
        {
            if (lowerLines == null) return (0, 0);

            var text = result.LexicalForm.ToLowerInvariant();
            int parsed = 0;
            foreach (var line in lowerLines)
            {
                if (text.Contains(line))
                {
                    parsed += line.Length;
                    text = text.Replace(line, "");
                }
            }
            return (parsed, text.Length);
        }

        public (int matchedChars, int unmatchedChars) GetRankByPhrases(LyricLine result, List<string> lowerLines)
        {
            if (string.IsNullOrWhiteSpace(result?.Text))
            {
                return (0, int.MaxValue);
            }
            var text = result.Text.ToLowerInvariant();
            int parsed = 0;
            foreach (var line in lowerLines)
            {
                if (text.Contains(line))
                {
                    parsed += line.Length;
                    text = text.Replace(line, "");
                }
            }
            return (parsed, text.Length);
        }

        public DetailedSpeechRecognitionResult? GetBestByPhrases(IEnumerable<DetailedSpeechRecognitionResult> results, double offset, List<string> lowerLines)
        {
            var sortedResults = results
                .Select(s => new
                    {
                        Rank = GetRankByPhrases(s, lowerLines),
                        s.LexicalForm,
                        OriginalIndex = results.ToList().IndexOf(s),
                        Start = s.Words?.Select(q => q.Offset).Min(),
                        End = s.Words?.Select(q => q.Offset).Max(),
                        Result = s
                    }
                )
                .OrderByDescending(s => s.Rank.matchedChars).ThenBy(s => s.Rank.unmatchedChars);
            if (sortedResults.All(r => r.Rank.matchedChars == 0))
            {
                sortedResults = sortedResults.OrderBy(r => r.Start).ThenByDescending(r => r.End);
            }

            return sortedResults.Select(r => r.Result).FirstOrDefault();
        }
    }
}
