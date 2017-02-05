using Lab_AudioAnalysis.Audio.Decorators;
using NAudio.Wave;

namespace Lab_AudioAnalysis.Audio.Api.Implementation
{
    public class NAudioPlayer : IAudioPlayer
    {
        private WaveOut _waveOut;
        private WaveIn _waveIn;
        private readonly BufferedWaveProvider _bufferedWaveProvider;

        public bool IsPlaying { get; private set; }


        public NAudioPlayer(WaveOut waveOut, WaveIn waveIn)
        {
            _waveOut = waveOut;
            _waveIn = waveIn;

            _bufferedWaveProvider = new BufferedWaveProvider(waveIn.WaveFormat);

            _waveIn.BufferMilliseconds = 25;
            _waveOut.PlaybackStopped += WaveIn_PlaybackStopped;
            _waveIn.DataAvailable += StreamData;

            _waveOut.DesiredLatency = 100;
            _waveOut.Init(_bufferedWaveProvider);
            _waveOut.PlaybackStopped += WaveOut_PlaybackStopped;

            _waveIn.StartRecording();
            _waveOut.Play();
        }

        private void StreamData(object sender, WaveInEventArgs e)
        {
            _bufferedWaveProvider.AddSamples(e.Buffer, 0, e.BytesRecorded);
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