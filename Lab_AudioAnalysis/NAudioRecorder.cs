using System;
using NAudio.Wave;

namespace Lab_AudioAnalysis
{
    internal class NAudioRecorder : IAudioRecorder
    {
        private readonly WaveIn _waveInput;
        private DateTime _startRecording;

        public int TotalBytesRecorded { get; private set; }
        public IAudioRecording AudioRecording { get; private set; }
        public TimeSpan TotalTime { get; private set; }
        public event EventHandler<WaveInEventArgs> BytesAvailable;

        public event EventHandler StoppedRecording;

        public NAudioRecorder(WaveIn waveInput, IAudioRecording audioRecording)
        {
            _waveInput = waveInput;
            AudioRecording = audioRecording;
        }

        public void Start()
        {
            _waveInput.DataAvailable += Input_DataAvailable;
            _waveInput.StartRecording();
            _startRecording = DateTime.Now;
        }

        public void Stop()
        {
            _waveInput.StopRecording();
            _waveInput.Dispose();
            AudioRecording.Close();
        }

        private void Input_DataAvailable(object sender, WaveInEventArgs e)
        {
            TotalTime = DateTime.Now.Subtract(_startRecording);
            TotalBytesRecorded += e.BytesRecorded;
            AudioRecording.AddData(e.Buffer, 0, e.BytesRecorded);
            BytesAvailable?.Invoke(sender, e);
        }
    }
}