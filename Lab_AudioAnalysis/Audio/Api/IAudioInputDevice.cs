using NAudio.Wave;

namespace Lab_AudioAnalysis.Audio.Api
{
    public interface IAudioInputDevice
    {
        IAudioRecorder CreateRecording(string filename);
        IAudioRecorder CreateRecording();
        WaveIn GetWavIn();
    }
}