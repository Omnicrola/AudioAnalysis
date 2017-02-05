namespace Lab_AudioAnalysis.Audio.Api
{
    public interface IAudioPlayer
    {
        void Start();
        void Stop();
        bool IsPlaying { get; }
    }
}