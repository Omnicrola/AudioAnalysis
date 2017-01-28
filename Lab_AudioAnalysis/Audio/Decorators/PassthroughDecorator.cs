using System.Linq;
using System.Windows.Controls;

namespace Lab_AudioAnalysis.Audio.Decorators
{
    public class PassthroughDecorator : AbstractDecorator
    {
        public override void Decorate(Canvas audioCanvas, int[] dilutedValues)
        {
            var polyline = CreatePolyline();
            Render(audioCanvas, dilutedValues, polyline);
        }
    }
}