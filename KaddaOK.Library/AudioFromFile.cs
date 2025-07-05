using NAudio.Flac;
using NAudio.Wave;

namespace KaddaOK.Library
{
    public interface IAudioFromFile
    {
        WaveStream? GetAudioFromFile(string filename);
    }

    public class AudioFromFile : IAudioFromFile
    {
        public WaveStream? GetAudioFromFile(string filename)
        {
            if (!File.Exists(filename)) return null;

            switch (Path.GetExtension(filename))
            {
                case ".wav":
                    return new WaveFileReader(filename);
                case ".flac":
                    return new FlacReader(filename);
                case ".mp3":
                    return new Mp3FileReader(filename);
                default:
                    throw new ArgumentException("Please use a .wav or .flac source.");
            }

        }
    }
}
