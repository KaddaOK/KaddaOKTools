using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KaddaOK.Library
{
    public interface IMinMaxFloatWaveStreamSampler
    {
        Task<(float min, float max)[]?> GetAllFloatsAsync(WaveStream? waveStream, int dataSamplingFactor);
    }
    public class MinMaxFloatWaveStreamSampler : IMinMaxFloatWaveStreamSampler
    {
        public static (float min, float max)[]? GetAllFloats(WaveStream? waveStream, int dataSamplingFactor)
        {
            if (waveStream == null)
            {
                return null;
            }
            int bytesPerSample = (waveStream.WaveFormat.BitsPerSample / 8);
            var totalSamplesByBytes = waveStream.Length / (bytesPerSample);
            var totalSamples = (int)totalSamplesByBytes;
            if (waveStream.CanSeek) waveStream.Position = 0;
            var sampleProvider = waveStream.ToSampleProvider();
            var entireWaveform = new float[totalSamples];
            sampleProvider.Read(entireWaveform, 0, totalSamples);

            var samplingFactoredPeaks = new (float, float)[totalSamples / dataSamplingFactor];
            for (var i = 0; i < totalSamples / dataSamplingFactor; i++)
            {
                var currentSegment = new ArraySegment<float>(entireWaveform, i * dataSamplingFactor, dataSamplingFactor);
                samplingFactoredPeaks[i] = (currentSegment.Min(), currentSegment.Max());
            }

            return samplingFactoredPeaks;
        }

        public async Task<(float min, float max)[]?> GetAllFloatsAsync(WaveStream? waveStream, int dataSamplingFactor)
        {
            return await Task.Run(() => GetAllFloats(waveStream, dataSamplingFactor));
        }
    }
}
