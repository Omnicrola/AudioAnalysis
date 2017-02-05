using System.Collections.Generic;
using NAudio.Wave;

namespace Lab_AudioAnalysis.Audio.Api.Implementation
{
    public class NAudioApiWrapper : IAudioApi
    {
        public List<IAudioInputDevice> InputDevices { get; }
        public List<IAudioOutputDevice> OutputDevices { get; }

        public NAudioApiWrapper()
        {
            InputDevices = GetInputDevices();
            OutputDevices = GetOutputDevices();
        }


        private List<IAudioOutputDevice> GetOutputDevices()
        {
            var audioOutputDevices = new List<IAudioOutputDevice>();
            var deviceCount = WaveOut.DeviceCount;
            for (int i = 0; i < deviceCount; i++)
            {
                var waveOutCapabilities = WaveOut.GetCapabilities(i);
                var nAudioOutputDevice = new NAudioOutputDevice(i, waveOutCapabilities);
                audioOutputDevices.Add(nAudioOutputDevice);
            }
            return audioOutputDevices;
        }


        private List<IAudioInputDevice> GetInputDevices()
        {
            var inputDevices = new List<IAudioInputDevice>();
            int waveInDevices = WaveIn.DeviceCount;
            for (int i = 0; i < waveInDevices; i++)
            {
                var waveInCapabilities = WaveIn.GetCapabilities(i);
                inputDevices.Add(new NAudioInputDevice(i, waveInCapabilities));
            }
            return inputDevices;
        }
    }
}