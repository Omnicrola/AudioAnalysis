﻿using NAudio.Wave;

namespace Lab_AudioAnalysis
{
    internal class NAudioDevice : IAudioDevice
    {
        private readonly int _deviceId;
        private readonly WaveInCapabilities _waveInCapabilities;

        public NAudioDevice(int deviceId, WaveInCapabilities waveInCapabilities)
        {
            _deviceId = deviceId;
            _waveInCapabilities = waveInCapabilities;
        }

        public IAudioRecorder CreateRecording()
        {
            var waveInput = CreateWaveInput();
            return new NAudioRecorder(waveInput, new EmptyRecording());
        }

        public IAudioRecorder CreateRecording(string filename)
        {
            var waveInput = CreateWaveInput();

            var waveFileWriter = new WaveFileWriter(filename, waveInput.WaveFormat);
            var nAudioWavRecording = new NAudioWavRecording(waveFileWriter);

            var nAudioRecorder = new NAudioRecorder(waveInput, nAudioWavRecording);
            return nAudioRecorder;
        }

        private WaveIn CreateWaveInput()
        {
            var waveInput = new WaveIn
            {
                DeviceNumber = _deviceId,
                WaveFormat = new WaveFormat(44100, _waveInCapabilities.Channels)
            };
            return waveInput;
        }
    }
}