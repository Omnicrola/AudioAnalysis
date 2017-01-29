using System.Collections.ObjectModel;
using System.Windows.Controls;

namespace Lab_AudioAnalysis.Audio.Decorators
{
    public interface IAudioCanvasDecorator
    {
        void Decorate(Canvas audioCanvas, int[] dilutedValues);
        void Decorate(Canvas audioCanvas, byte[] rawData);
    }
}