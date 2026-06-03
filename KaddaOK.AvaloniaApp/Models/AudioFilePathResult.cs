using System;
using System.Collections.Generic;

namespace KaddaOK.AvaloniaApp.Models
{
    public abstract class AudioFilePathResult
    {
        public List<Exception> Errors { get; set; } = new List<Exception>();        
    }

    public class SingleAudioFilePathResult : AudioFilePathResult
    {
        public bool AudioLoaded { get; set; }
        public string? AudioFilePath { get; set; }
    }

    public class AllAudioFilePathsResult : AudioFilePathResult
    {
        public bool UnseparatedAudioLoaded { get; set; }
        public string? UnseparatedAudioFilePath { get; set; }
        public bool VocalsAudioLoaded { get; set; }
        public string? VocalsAudioFilePath { get; set; }
        public bool InstrumentalAudioLoaded { get; set; }
        public string? InstrumentalAudioFilePath { get; set; }

    }
}