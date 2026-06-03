namespace KaddaOK.AvaloniaApp.Models
{
    public class UnsavedChangesPrompt { }

    public class AudioFileNotFoundPrompt
    {
        public string AttemptedPath { get; }

        public AudioFileNotFoundPrompt(string attemptedPath)
        {
            AttemptedPath = attemptedPath;
        }
    }
}
