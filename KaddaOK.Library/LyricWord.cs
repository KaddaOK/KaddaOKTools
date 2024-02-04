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
    }
}
