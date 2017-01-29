using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Lab_AudioAnalysis.BitmapRendering;
using NAudio.Dsp;
using NAudio.Wave;

namespace Lab_AudioAnalysis.Audio.Decorators
{
    public class FastFourierTransformRenderer
    {
        private readonly Canvas _canvas;
        private readonly SampleAggregator _sampleAggregator;
        private WriteableBitmap _writeableBitmap;
        private Image _image;
        private BitmapWrapper _bitmapWrapper;

        private int xPos = 0;
        private static readonly int VERTICAL_SCALE = 1;

        private int _horizontalScale = 2;
        private static readonly int FFT_SAMPLE_SIZE = 1024;

        public FastFourierTransformRenderer(Canvas canvas)
        {
            _canvas = canvas;
            _sampleAggregator = new SampleAggregator(FFT_SAMPLE_SIZE);
            _sampleAggregator.PerformFFT = true;
            _sampleAggregator.FftCalculated += FftCalculated;
        }

        private void FftCalculated(object sender, FftEventArgs e)
        {
            xPos += _horizontalScale;
            int resultsPerBin = (int)Math.Ceiling(e.Result.Length * 1.0 / (_canvas.ActualHeight - VERTICAL_SCALE));
            double[] singleBin = new double[resultsPerBin];
            int binIndex = 0;
            int yPos = 0;
            for (int index = 0; index < e.Result.Length; index++)
            {
                Complex complex = e.Result[index];
                double intensity = GetIntensity(complex);
                singleBin[binIndex] = intensity;

                if (binIndex >= resultsPerBin - 1)
                {
                    double average = singleBin.Average();
                    byte rgb = (byte)(255 - (average * 255));
                    var color = new Color { R = rgb, G = rgb, B = rgb };
                    DrawPixel(xPos, yPos, color);
                    yPos++;
                    binIndex = 0;
                }
                else
                {
                    binIndex++;
                }
            }
        }

        private void DrawPixel(int markerX, int markerY, Color color)
        {
            for (int x = 0; x < _horizontalScale; x++)
            {
                for (int y = 0; y < VERTICAL_SCALE; y++)
                {
                    _bitmapWrapper.SetPixel(markerX + x, markerY + y, color);
                }
            }
        }

        private double GetIntensity(Complex c)
        {
            // not entirely sure whether the multiplier should be 10 or 20 in this case.
            // going with 10 from here http://stackoverflow.com/a/10636698/7532
            double intensityDB = 20 * Math.Log10(Math.Sqrt(c.X * c.X + c.Y * c.Y));
            double minDB = -90;
            if (intensityDB < minDB) intensityDB = minDB;
            double percentIntensity = intensityDB / minDB;
            return percentIntensity;
        }


        public void Draw(WaveFileReader wavReader)
        {
            _canvas.Children.Clear();
            _bitmapWrapper = new BitmapWrapper(_canvas.ActualWidth, _canvas.ActualHeight);
            //            _horizontalScale = (int)(wavReader.Length / wavReader.WaveFormat.SampleRate / FFT_SAMPLE_SIZE);
            xPos = 0;

            wavReader.Position = 0;
            for (int i = 0; i < wavReader.SampleCount; i++)
            {
                var sampleFrame = wavReader.ReadNextSampleFrame();
                foreach (float sampleValue in sampleFrame)
                {
                    _sampleAggregator.Add(sampleValue);
                }
            }

            using (var fileStream = new FileStream("temp.bmp", FileMode.Create))
            {
                var pngBitmapEncoder = new PngBitmapEncoder();
                pngBitmapEncoder.Frames.Add(BitmapFrame.Create(_bitmapWrapper.Bitmap));
                pngBitmapEncoder.Save(fileStream);
            }
            _canvas.Children.Add(new Image { Source = _bitmapWrapper.Bitmap });

        }
    }
}