using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Lab_AudioAnalysis.Audio.Decorators
{
    public abstract class AbstractDecorator : IAudioCanvasDecorator
    {
        public abstract void Decorate(Canvas audioCanvas, int[] dilutedValues);

        public Brush BrushColor { get; set; }

        protected Polyline CreatePolyline()
        {
            var polyline = new Polyline();
            polyline.Stroke = BrushColor;
            polyline.Name = "smoothed_waveform";
            polyline.StrokeThickness = 1;
            return polyline;
        }

        protected void Render(Canvas audioCanvas, int[] screenValues, Polyline polyline)
        {
            var width = audioCanvas.ActualWidth;
            var height = audioCanvas.ActualHeight;
            for (int xPos = 0; xPos < screenValues.Length; xPos++)
            {
                double smoothedValue = screenValues[xPos];
                var point = Normalize(screenValues.Length, width, height, xPos, smoothedValue);
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