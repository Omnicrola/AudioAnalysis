using System;
using NAudio.Wave;

namespace Lab_AudioAnalysis.Audio.Api
{
    public interface IAudioRecorder
    {
        void Start();
        void Stop();
        int TotalBytesRecorded { get; }
        IAudioRecording AudioRecording { get; }
        TimeSpan TotalTime { get; }
        WaveFormat WaveFormat { get; }
        event EventHandler<WaveInEventArgs> BytesAvailable;
    }

}