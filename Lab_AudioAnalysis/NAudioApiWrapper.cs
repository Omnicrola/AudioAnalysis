using System;
using System.Collections.Generic;
using NAudio.Wave;

namespace Lab_AudioAnalysis
{
    public class NAudioApiWrapper : IAudioApi
    {
        public List<IAudioDevice> InputDevices { get; set; }

        public NAudioApiWrapper()
        {
            InputDevices = GetInputDevices();
        }


        private List<IAudioDevice> GetInputDevices()
        {
            var inputDevices = new List<IAudioDevice>();
            int waveInDevices = WaveIn.DeviceCount;
            for (int i = 0; i < waveInDevices; i++)
            {
                var waveInCapabilities = WaveIn.GetCapabilities(i);
                inputDevices.Add(new NAudioDevice(i, waveInCapabilities));
            }
            return inputDevices;
        }
    }
}