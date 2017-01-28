using System;
using NAudio.Wave;

namespace Lab_AudioAnalysis
{
    public interface IAudioRecorder
    {
        void Start();
        void Stop();
        int TotalBytesRecorded { get; }
        IAudioRecording AudioRecording { get; }
        TimeSpan TotalTime { get; }
        event EventHandler<WaveInEventArgs> BytesAvailable;
    }

}