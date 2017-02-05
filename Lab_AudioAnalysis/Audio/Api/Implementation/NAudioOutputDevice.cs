using Lab_AudioAnalysis.Audio.Decorators;
using NAudio.Wave;

namespace Lab_AudioAnalysis.Audio.Api.Implementation
{
    public class NAudioOutputDevice : IAudioOutputDevice
    {
        private readonly int _deviceId;
        private readonly WaveOutCapabilities _waveOutCapabilities;

        public NAudioOutputDevice(int deviceId, WaveOutCapabilities waveOutCapabilities)
        {
            _deviceId = deviceId;
            _waveOutCapabilities = waveOutCapabilities;
        }

        public IAudioPlayer CreatePlayer(IAudioInputDevice audioRecorder)
        {
            var waveOut = new WaveOut { DesiredLatency = 200, DeviceNumber = _deviceId };
            var waveIn = audioRecorder.GetWavIn();
            return new NAudioPlayer(waveOut, waveIn);
        }

        public override string ToString()
        {
            return $"{_deviceId} : {_waveOutCapabilities.ProductName}";
        }
    }
}