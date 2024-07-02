using KaddaOK.AvaloniaApp.Models;
using KaddaOK.Library;
using Microsoft.CognitiveServices.Speech;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reflection;
using NAudio.Flac;

namespace KaddaOK.AvaloniaApp.ViewModels.DesignTime
{
    public static class DesignTimeKaraokeProcess
    {
        public static KaraokeProcess Get()
        {
            var dataSamplingFactor = 50; // TODO: experiment with this value; too high crashes the app and that needs fixing
            var path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

            var unseparatedFilePath = Path.Combine(path!, "Assets", "DesignTime", "Example Song - Full.flac");
            var unseparatedFileStream = new FlacReader(unseparatedFilePath);
            var unseparatedAudioFloats = MinMaxFloatWaveStreamSampler.GetAllFloats(unseparatedFileStream, dataSamplingFactor);

            var vocalFilePath = Path.Combine(path!, "Assets", "DesignTime", "Example Song - Vocals.flac");
            var vocalFileStream = new FlacReader(vocalFilePath);
            var vocalAudioFloats = MinMaxFloatWaveStreamSampler.GetAllFloats(vocalFileStream, dataSamplingFactor);

            var instrumentalFilePath = Path.Combine(path!, "Assets", "DesignTime", "Example Song - Instruments.flac");
            var instrumentalFileStream = new FlacReader(instrumentalFilePath);
            var instrumentalAudioFloats = MinMaxFloatWaveStreamSampler.GetAllFloats(instrumentalFileStream, dataSamplingFactor);

            var originalResultsJson = File.ReadAllText(Path.Combine(path!, "Assets", "DesignTime", "Example Song - Azure Recognition.json"));
            var originalResults = JsonConvert.DeserializeObject<List<IEnumerable<DetailedSpeechRecognitionResult>>>(originalResultsJson);
            var linePossibilities = new ObservableCollection<LinePossibilities>(originalResults!.Select(s =>
                               new LinePossibilities(s.Select(q => new LyricLine(q)))));

            return new KaraokeProcess
            {
                VocalsAudioFilePath = vocalFilePath,
                VocalsAudioStream = vocalFileStream,
                VocalsAudioFloats = vocalAudioFloats,

                InstrumentalAudioFilePath = instrumentalFilePath,
                InstrumentalAudioStream = instrumentalFileStream,
                InstrumentalAudioFloats = instrumentalAudioFloats,

                UnseparatedAudioFilePath = unseparatedFilePath,
                UnseparatedAudioStream = unseparatedFileStream,
                UnseparatedAudioFloats = unseparatedAudioFloats,

                RecognitionIsRunning = true,

                DetectedLinePossibilities = linePossibilities,

                KaraokeSource = InitialKaraokeSource.AzureSpeechService,

                ChosenLines = new ObservableCollection<LyricLine>(linePossibilities.Select(l => l.Lyrics[0]))
            };
        }
    }
}
