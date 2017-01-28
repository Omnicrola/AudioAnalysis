using System;
using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Media;

namespace Lab_AudioAnalysis.Audio.Decorators
{
    public class BackgroundFilterDecorator : AbstractDecorator
    {
        public double Floor { get; set; }

        public override void Decorate(Canvas audioCanvas, int[] dilutedValues)
        {
            var screenValues = FloorValues(dilutedValues);

            var polyline = CreatePolyline();
            Render(audioCanvas, screenValues, polyline);
        }

        private int[] FloorValues(int[] dilutedValues)
        {
            var floorValues = new int[dilutedValues.Length];
            for (int index = 0; index < dilutedValues.Length; index++)
            {
                int value = dilutedValues[index];
                floorValues[index] = value < Math.Abs(Floor) ? 0 : value;
            }
            return floorValues;
        }
    }
}