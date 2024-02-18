using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KaddaOK.AvaloniaApp.Models;
using KaddaOK.Library;
using Microsoft.CognitiveServices.Speech;
using Newtonsoft.Json;

namespace KaddaOK.AvaloniaApp.ViewModels.DesignTime
{
    public class DummyAzureRecognizer : IAzureRecognizer
    {
        public double? ProcessedSeconds => 32.8;
        public bool Recognizing => true;

        public Task Recognize(string speechKey, string speechRegion, string language, WaveStream waveStream, KnownOriginalLyrics lyrics,
            Action<LinePossibilities> reportRecognizedLine, Action<string> reportProgress)
        {
            throw new NotImplementedException();
        }

        public Task CancelRecognition(Action<string> reportProgress)
        {
            throw new NotImplementedException();
        }
    }
    public class DesignTimeRecognizeViewModel : RecognizeViewModel
    {
        public DesignTimeRecognizeViewModel() : base(new DummyAzureRecognizer(), new KaddaOKSettingsPersistor(), new NfaCtmImporter(), DesignTimeKaraokeProcess.Get())
        {
            HasEverBeenStarted = true;
            LogContents = new ObservableCollection<string>(new[]
            {
                "Started a new run at 2023.10.21_15.30.",
                "Configuring speech subscription...",
                "Configuring wav file input...",
                "Creating recognizer...",
                "Adding phrases...",
                "Starting continuous recognition...",
                "Recognition session started."
            });
        }
    }
}
