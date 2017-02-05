namespace Lab_AudioAnalysis.Audio.Api
{
    internal class EmptyRecording : IAudioRecording
    {
        public void AddData(byte[] buffer, int offset, int bytesRecorded)
        {
        }

        public void PlayFromBeginning()
        {
        }

        public string Filename { get; }
        public void Close()
        {
        }
    }
}