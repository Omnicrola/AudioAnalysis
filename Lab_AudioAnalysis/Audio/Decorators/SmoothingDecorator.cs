using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using Lab_AudioAnalysis.Extensions;

namespace Lab_AudioAnalysis.Audio.Decorators
{
    public class SmoothingDecorator : IAudioCanvasDecorator
    {
        private readonly int _smoothingFactor;

        public Brush BrushColor { get; set; }

        public SmoothingDecorator(int smoothingFactor)
        {
            _smoothingFactor = smoothingFactor;
            BrushColor = Brushes.Green;
        }

        public void Decorate(Canvas audioCanvas, int[] dilutedValues)
        {
            var smoothedValues = SmoothValues(dilutedValues);
            var polyline = CreatePolyline();
            Render(audioCanvas, smoothedValues, polyline);
        }


        private List<double> SmoothValues(int[] dilutedValues)
        {
            var smoothedValues = new List<double>(dilutedValues.Length);
            int[] dataBuffer = new int[_smoothingFactor];
            for (int index = 0; index < dilutedValues.Length; index++)
            {
                dilutedValues.CopyRange(dataBuffer, index);
                var average = dataBuffer.Average();
                smoothedValues.Add(average);
            }
            return smoothedValues;
        }

        private Polyline CreatePolyline()
        {
            var polyline = new Polyline();
            polyline.Stroke = BrushColor;
            polyline.Name = "smoothed_waveform";
            polyline.StrokeThickness = 1;
            return polyline;
        }

        private void Render(Canvas audioCanvas, List<double> smoothedValues, Polyline polyline)
        {
            var width = audioCanvas.ActualWidth;
            var height = audioCanvas.ActualHeight;
            for (int xPos = 0; xPos < smoothedValues.Count; xPos++)
            {
                double smoothedValue = smoothedValues[xPos];
                var point = Normalize(smoothedValues.Count, width, height, xPos, smoothedValue);
                polyline.Points.Add(point);
            }
            audioCanvas.Children.Add(polyline);
        }

        private Point Normalize(int totalPoints, double width, double height, int xPos, double y)
        {
            var point = new Point();

            point.X = 1.0 * xPos / totalPoints * width;
            point.Y = height / 2.0 - y / (Int32.MaxValue * 1.0) * (height / 2.0);
            return point;
        }
    }
}