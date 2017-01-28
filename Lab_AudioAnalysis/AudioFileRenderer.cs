using System;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using NAudio.Wave;

namespace Lab_AudioAnalysis
{
    public class AudioFileRenderer
    {
        public void Render(IAudioRecording audioRecording, Canvas audioCanvas)
        {
            using (var audioFileReader = new AudioFileReader(audioRecording.Filename))
            {
                var canvasWidth = audioCanvas.ActualWidth;

                var waveFormat = audioFileReader.WaveFormat;
                var totalSamples = audioFileReader.Length / (waveFormat.Channels * waveFormat.BitsPerSample / 8);

                var bytesPerBatch = (int)Math.Max(40, totalSamples / canvasWidth);
                var mid = 100;
                var yScale = 100;

                float[] byteBuffer = new float[bytesPerBatch];
                var xPos = 0;

                bool hasBytesRemaining = true;
                while (hasBytesRemaining)
                {
                    var actualBytesRead = audioFileReader.Read(byteBuffer, 0, bytesPerBatch);
                    hasBytesRemaining = actualBytesRead == bytesPerBatch;

                    float maxValue = FindMaxValue(byteBuffer);
                    DrawVerticalLine(audioCanvas, xPos, maxValue);
                    xPos++;
                }
            }
        }

        private void DrawVerticalLine(Canvas audioCanvas, int xPos, float maxValue)
        {
            float yScale = 100;
            float yOffset = 100;
            var line = new Line();
            line.X1 = xPos;
            line.X2 = xPos;
            line.Y1 = yOffset + (maxValue * yScale);
            line.Y2 = yOffset - (maxValue * yScale);
            line.StrokeThickness = 1;
            line.Stroke = Brushes.Green;
            audioCanvas.Children.Add(line);
        }

        private float FindMaxValue(float[] byteBuffer)
        {
            float max = 0;
            for (int i = 0; i < byteBuffer.Length; i++)
            {
                max = Math.Max(Math.Abs(byteBuffer[i]), max);
            }
            return max;
        }
    }
}