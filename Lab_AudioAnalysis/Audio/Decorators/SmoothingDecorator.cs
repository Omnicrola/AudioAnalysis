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
    public class SmoothingDecorator : AbstractDecorator
    {
        private readonly int _smoothingFactor;


        public SmoothingDecorator(int smoothingFactor)
        {
            _smoothingFactor = smoothingFactor;
            BrushColor = Brushes.Green;
        }

        public override void Decorate(Canvas audioCanvas, int[] dilutedValues)
        {
            var smoothedValues = SmoothValues(dilutedValues);
            var polyline = CreatePolyline();
            Render(audioCanvas, smoothedValues, polyline);
        }


        private int[] SmoothValues(int[] dilutedValues)
        {
            var smoothedValues = new int[dilutedValues.Length];
            int[] dataBuffer = new int[_smoothingFactor];
            for (int index = 0; index < dilutedValues.Length; index++)
            {
                dilutedValues.CopyRange(dataBuffer, index);
                int average = (int)dataBuffer.Average();
                smoothedValues[index] = average;
            }
            return smoothedValues;
        }
    }
}