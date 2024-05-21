using Microsoft.CognitiveServices.Speech;

namespace KaddaOK.Library
{
    public class LyricWord : ObservableBase, IAudioSpan
    {
        private double startSecond;
        public double StartSecond
        {
            get => startSecond;
            set => SetProperty(ref startSecond, value);
        }
        private double endSecond;
        public double EndSecond
        {
            get => endSecond;
            set => SetProperty(ref endSecond, value);
        }
        private string? text;
        public string? Text
        {
            get => text;
            set => SetProperty(ref text, value);
        }

        public LyricWord(WordLevelTimingResult fromAzure)
        {
            StartSecond = TimeSpan.FromTicks(fromAzure.Offset).TotalSeconds;
            EndSecond = TimeSpan.FromTicks(fromAzure.Duration).TotalSeconds + StartSecond;
            Text = fromAzure.Word;
        }
        public LyricWord() {}

        public static List<LyricWord> GetLyricWordsAcrossTime(string? enteredText, double startTime, double endTime)
        {
            var fullWords = enteredText?.Split(' ') ?? new[] { enteredText };
            var syllables = fullWords
                .SelectMany(f => (f + " ")
                    .Split('|', '/'))
                .Where(w => !string.IsNullOrWhiteSpace(w))
                .ToList();
            var availableTime = endTime - startTime;
            var eachSyllableGets = Math.Round(availableTime / (double)syllables.Count, 2);
            var currentTime = Math.Round(startTime, 2);
            var listOfNewWords = new List<LyricWord>();
            for (int i = 0; i < syllables.Count; i++)
            {
                var newWord = new LyricWord
                {
                    Text = syllables[i],
                    StartSecond = currentTime
                };
                currentTime = Math.Round(currentTime + eachSyllableGets, 2);
                newWord.EndSecond = currentTime;
                listOfNewWords.Add(newWord);
            }

            return listOfNewWords;
        }
    }
}
