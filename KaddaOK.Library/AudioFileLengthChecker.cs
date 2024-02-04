using NAudio.Wave;

namespace KaddaOK.Library
{
    public interface IAudioFileLengthChecker
    {
        TimeSpan? CheckAudioLength(string filePath);
    }
    public class AudioFileLengthChecker : IAudioFileLengthChecker
    {
        public TimeSpan? CheckAudioLength(string filePath)
        {
            if (!File.Exists(filePath))
            {
                return null;
            }

            try
            {
                var reader = new AudioFileReader(filePath);
                return reader.TotalTime;
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
