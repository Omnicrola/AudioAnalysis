using Lab_AudioAnalysis.Audio.Decorators;
using NAudio.Dsp;
using NAudio.Wave;

namespace Lab_AudioAnalysis.Audio.Api.Implementation
{
    public class NAudioPlayer : IAudioPlayer
    {
        private WaveOut _waveOut;
        private WaveIn _waveIn;
        private readonly FilteredBuffer _filteredBuffer;

        public bool IsPlaying { get; private set; }


        public NAudioPlayer(WaveOut waveOut, WaveIn waveIn)
        {
            _waveOut = waveOut;
            _waveIn = waveIn;

            _filteredBuffer = new FilteredBuffer(waveIn.WaveFormat);
            _filteredBuffer.Filters.Add(BiQuadFilter.LowPassFilter(waveIn.WaveFormat.SampleRate, 100, 1));


            _waveIn.BufferMilliseconds = 25;
            _waveOut.PlaybackStopped += WaveIn_PlaybackStopped;
            _waveIn.DataAvailable += StreamData;

            _waveOut.DesiredLatency = 100;
            _waveOut.Init(_filteredBuffer);
            _waveOut.PlaybackStopped += WaveOut_PlaybackStopped;

            _waveIn.StartRecording();
            _waveOut.Play();
        }

        private void StreamData(object sender, WaveInEventArgs e)
        {
            _filteredBuffer.AddSamples(e.Buffer, 0, e.BytesRecorded);
        }

        public void Start()
        {
            _waveIn.DataAvailable += StreamData;
            IsPlaying = true;
        }

        public void Stop()
        {
            _waveIn.DataAvailable -= StreamData;
            IsPlaying = false;
        }


        private void WaveIn_PlaybackStopped(object sender, StoppedEventArgs e)
        {
            Stop();
            _waveIn?.Dispose();
            _waveIn = null;
        }

        private void WaveOut_PlaybackStopped(object sender, StoppedEventArgs e)
        {
            Stop();
            _waveOut?.Dispose();
            _waveOut = null;
        }
    }
}