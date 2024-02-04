using System;
using System.Collections.Generic;
using KaddaOK.Library;

namespace KaddaOK.AvaloniaApp.Models
{
    public class KaddaOKSettings : ObservableBase
    {
        private string? azureSpeechKey;
        public string? AzureSpeechKey
        {
            get => azureSpeechKey;
            set => SetProperty(ref azureSpeechKey, value);
        }

        private string? azureSpeechRegion;
        public string? AzureSpeechRegion
        {
            get => azureSpeechRegion;
            set => SetProperty(ref azureSpeechRegion, value);
        }

        private SupportedLanguage? recognitionLanguage;
        public SupportedLanguage? RecognitionLanguage
        {
            get => recognitionLanguage;
            set => SetProperty(ref recognitionLanguage, value);
        }
    }
}
