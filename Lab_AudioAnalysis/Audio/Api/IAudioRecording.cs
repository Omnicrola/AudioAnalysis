namespace Lab_AudioAnalysis.Audio.Api
{
    public interface IAudioRecording
    {
        void AddData(byte[] buffer, int offset, int bytesRecorded);
        void PlayFromBeginning();
        string Filename { get; }
        void Close();
    }
}