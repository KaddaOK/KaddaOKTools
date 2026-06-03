using System;

namespace KaddaOK.AvaloniaApp.Services
{
    public class AudioFileNotFoundException : Exception
    {
        public string AttemptedPath { get; }

        public AudioFileNotFoundException(string attemptedPath)
            : base($"Could not access audio file at path '{attemptedPath}'")
        {
            AttemptedPath = attemptedPath;
        }
    }
}
