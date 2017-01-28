namespace Lab_AudioAnalysis
{
    public interface IAudioDevice
    {
        IAudioRecorder CreateRecording(string filename);
        IAudioRecorder CreateRecording();
    }
}