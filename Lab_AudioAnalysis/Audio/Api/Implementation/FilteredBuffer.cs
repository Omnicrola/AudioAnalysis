using System;
using System.Collections.Generic;
using NAudio.Dsp;
using NAudio.Utils;
using NAudio.Wave;

namespace Lab_AudioAnalysis.Audio.Api.Implementation
{
    public class FilteredBuffer : ISampleProvider
    {
        private const int BYTES_IN_FLOAT32 = 4;

        private readonly CircularBuffer _circularBuffer;
        public List<BiQuadFilter> Filters { get; }
        public WaveFormat WaveFormat { get; private set; }

        public FilteredBuffer(WaveFormat waveFormat)
        {
            WaveFormat = waveFormat;
            Filters = new List<BiQuadFilter>();
            int bufferLength = waveFormat.AverageBytesPerSecond * 5;
            _circularBuffer = new CircularBuffer(bufferLength);
        }

        public int Read(float[] buffer, int offset, int count)
        {
            byte[] byteBuffer = new byte[buffer.Length * BYTES_IN_FLOAT32];
            WaveBuffer waveBuffer = new WaveBuffer(byteBuffer);
            int sampleCount = count / BYTES_IN_FLOAT32;
            var read = Read(waveBuffer.FloatBuffer, offset / BYTES_IN_FLOAT32, sampleCount) * BYTES_IN_FLOAT32;
            for (int position = 0; position < waveBuffer.FloatBufferCount; position++)
            {
                foreach (BiQuadFilter biQuadFilter in Filters)
                {
                    waveBuffer.FloatBuffer[position] = biQuadFilter.Transform(waveBuffer.FloatBuffer[position]);
                }
            }
            return read;
        }

        public void AddSamples(byte[] buffer, int offset, int bytesRecorded)
        {
            _circularBuffer.Write(buffer, offset, bytesRecorded);
        }

    }
}