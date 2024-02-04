using System.Diagnostics;
using Microsoft.CognitiveServices.Speech.Audio;
using Microsoft.CognitiveServices.Speech;
using NAudio.Wave;

namespace KaddaOK.Library
{
    public interface IAzureRecognizer
    {
        public double? ProcessedSeconds { get; }
        public bool Recognizing { get; }

        Task Recognize(string speechKey,
            string speechRegion,
            string recognitionLanguage,
            WaveStream waveStream,
            KnownOriginalLyrics lyrics,
            Action<LinePossibilities> reportRecognizedLine,
            Action<string> reportProgress);

        Task CancelRecognition(Action<string> reportProgress);
    }
    public class AzureRecognizer : ObservableBase, IAzureRecognizer
    {
        private double? processedSeconds;
        public double? ProcessedSeconds
        {
            get => processedSeconds;
            private set => SetProperty(ref processedSeconds, value);
        }

        private bool recognizing;
        public bool Recognizing
        {
            get => recognizing;
            private set => SetProperty(ref recognizing, value);
        }

        public SpeechRecognizer? Recognizer { get; private set; }
        public IResultRanker Ranker { get; }
        public AzureRecognizer(IResultRanker ranker)
        {
            Ranker = ranker;
        }

        private static void CopyAudioStream(WaveStream fromStream, PushAudioInputStream toStream)
        {
            if (fromStream.Position != 0) fromStream.Seek(0, SeekOrigin.Begin);
            byte[] readBytes = new byte[3200];
            int totalBytesRead = 0;
            int bytesLastRead = 0;
            do
            {
                bytesLastRead = fromStream.Read(readBytes, 0, 3200);
                totalBytesRead += bytesLastRead;
                toStream.Write(readBytes, bytesLastRead);
            } while (bytesLastRead > 0);
        }

        public async Task Recognize(string speechKey, 
            string speechRegion, 
            string recognitionLanguage,
            WaveStream waveStream, 
            KnownOriginalLyrics lyrics,
            Action<LinePossibilities> reportRecognizedLine,
            Action<string> reportProgress)
        {
            Recognizing = true;
            string timestamp = DateTime.Now.ToString("yyyy.MM.dd_HH.mm");
            reportProgress($"Started a new run at {timestamp}.");

            reportProgress("Configuring speech subscription...");
            var speechConfig = SpeechConfig.FromSubscription(speechKey, speechRegion);
            speechConfig.SpeechRecognitionLanguage = recognitionLanguage;
            speechConfig.RequestWordLevelTimestamps();
            speechConfig.SetProperty(PropertyId.Speech_SegmentationSilenceTimeoutMs, "100");

            var stopRecognition = new TaskCompletionSource<int>(TaskCreationOptions.RunContinuationsAsynchronously);
            reportProgress("Configuring wav file input...");
            using var audioConfigStream = AudioInputStream.CreatePushStream(
                AudioStreamFormat.GetWaveFormatPCM((uint)waveStream.WaveFormat.SampleRate, (byte)waveStream.WaveFormat.BitsPerSample, (byte)waveStream.WaveFormat.Channels));
            CopyAudioStream(waveStream, audioConfigStream);
            using var audioConfig = AudioConfig.FromStreamInput(audioConfigStream);
            reportProgress("Creating recognizer...");
            using var recognizer = new SpeechRecognizer(speechConfig, audioConfig);
            Recognizer = recognizer;
            reportProgress("Adding phrases...");
            var phraseList = PhraseListGrammar.FromRecognizer(recognizer);

            if (lyrics?.DistinctLines != null)
            {
                foreach (var line in lyrics.DistinctLines)
                {
                    phraseList.AddPhrase(line);
                }
            }

            recognizer.Recognizing += (s, e) =>
            {
                var startOfChunk = TimeSpan.FromTicks(e.Result.OffsetInTicks);
                var endOfChunk = startOfChunk + e.Result.Duration;
                if ((ProcessedSeconds ?? 0) < endOfChunk.TotalSeconds)
                {
                    ProcessedSeconds = endOfChunk.TotalSeconds;
                }
            };
            recognizer.Recognized += (s, e) =>
            {
                var startOfChunk = TimeSpan.FromTicks(e.Result.OffsetInTicks);
                var endOfChunk = startOfChunk + e.Result.Duration;
                if ((ProcessedSeconds ?? 0) < endOfChunk.TotalSeconds)
                {
                    ProcessedSeconds = endOfChunk.TotalSeconds;
                }

                if (e.Result.Reason == ResultReason.RecognizedSpeech)
                {
                    var originalResult = e.Result.Best()?.ToList();
                    if (originalResult != null)
                    {
                        var lines = originalResult
                            .Where(r => !string.IsNullOrWhiteSpace(r.Text))
                            .Select(d => new
                            {
                                Result = d,
                                Rank = Ranker.GetRankByPhrases(d, lyrics?.LoweredLines)
                            })
                            .OrderBy(q => q.Rank.unmatchedChars).ThenByDescending(q => q.Rank.matchedChars)
                            .Select(q => new LyricLine(q.Result))
                            .ToList();
                        var possibilities = new LinePossibilities(lines);
                        reportProgress($" --> {possibilities.StartSecond}-{possibilities.EndSecond}: '{possibilities.Lyrics[0].Text}' and {possibilities.Lyrics.Count - 1} others");
                        reportRecognizedLine(possibilities);
                    }
                }
                else if (e.Result.Reason == ResultReason.NoMatch)
                {
                    reportProgress("...no match... (probably a period of silence in the wave file)");
                }
            };

            recognizer.SessionStarted += (s, e) =>
            {
                reportProgress("Recognition session started.");
            };
            recognizer.Canceled += (s, e) =>
            {
                switch (e.Reason)
                {
                    case CancellationReason.CancelledByUser:
                        reportProgress("Recognizer reports session cancelled by user.");
                        break;
                    case CancellationReason.Error:
                        reportProgress("Recognizer encountered error:");
                        reportProgress(e.ErrorCode.ToString());
                        reportProgress(e.ErrorDetails);
                        break;
                }
            };
            recognizer.SessionStopped += (s, e) =>
            {
                stopRecognition.TrySetResult(0);
            };

            reportProgress("Starting continuous recognition...");
            // Starts continuous recognition. Uses StopContinuousRecognitionAsync() to stop recognition.
            await recognizer.StartContinuousRecognitionAsync().ConfigureAwait(false);

            // Waits for completion.
            // Use Task.WaitAny to keep the task rooted.
            Task.WaitAny(stopRecognition.Task);

            // Stops recognition.
            await recognizer.StopContinuousRecognitionAsync().ConfigureAwait(false);
            reportProgress("Finished recognizing.");
            Recognizing = false;
            Recognizer = null;
        }

        public async Task CancelRecognition(Action<string> reportProgress)
        {
            if (Recognizer != null)
            {
                await Recognizer.StopContinuousRecognitionAsync().ConfigureAwait(false);
                reportProgress("Recognition aborted using cancel button.");
                Recognizer = null;
                Recognizing = false;
            }
        }
    }
}
