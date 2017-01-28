using System;
using System.Threading;
using NAudio.Wave;

namespace Lab_AudioAnalysis
{
    internal class NAudioWavRecording : IAudioRecording
    {
        private readonly WaveFileWriter _waveFileWriter;
        public string Filename => _waveFileWriter.Filename;


        public NAudioWavRecording(WaveFileWriter waveFileWriter)
        {
            _waveFileWriter = waveFileWriter;
        }

        public void AddData(byte[] buffer, int offset, int bytesRecorded)
        {
            _waveFileWriter.Write(buffer, offset, bytesRecorded);
            _waveFileWriter.Flush();
        }

        public void PlayFromBeginning()
        {

            Console.WriteLine("Starting playback...");
            using (var waveFileReader = new WaveFileReader(_waveFileWriter.Filename))
            using (var waveChannel32 = new WaveChannel32(waveFileReader) { PadWithZeroes = false })
            using (var directSoundOut = new DirectSoundOut())
            {
                var totalBytes = waveFileReader.Length;
                var totalTime = waveFileReader.TotalTime;
                Console.WriteLine($"Playing {totalBytes}bytes ({totalTime})");

                directSoundOut.Init(waveChannel32);
                directSoundOut.Play();

                while (directSoundOut.PlaybackState != PlaybackState.Stopped)
                {
                    Thread.Sleep(20);
                }
                directSoundOut.Stop();
                Console.WriteLine("Recording ended.");
            }
        }

        public void Close()
        {
            _waveFileWriter.Dispose();
        }
    }
}