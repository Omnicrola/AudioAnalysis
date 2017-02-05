using NAudio.Wave;

namespace Lab_AudioAnalysis.Audio.Api
{
    public interface IAudioOutputDevice
    {
        IAudioPlayer CreatePlayer(IAudioInputDevice audioRecorder);
    }
}