using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using Lab_AudioAnalysis.Audio.Decorators;
using NAudio.Wave;

namespace Lab_AudioAnalysis.Audio
{
    public class AudioCanvasRenderer
    {
        private readonly Canvas _audioCanvas;
        private readonly int _dilutionScale;
        private readonly List<int> _allValues;

        public AudioCanvasRenderer(Canvas audioCanvas, int dilutionScale, int width, int height)
        {
            _audioCanvas = audioCanvas;
            _dilutionScale = dilutionScale;
            _allValues = new List<int>();
            Decorators = new List<IAudioCanvasDecorator>();
        }

        public List<IAudioCanvasDecorator> Decorators { get; private set; }

        public void Render(IWaveProvider audioProvider)
        {
            _allValues.Clear();

            byte[] sample = new byte[4];

            bool dataRemains = true;
            do
            {
                int bytesRead = audioProvider.Read(sample, 0, sample.Length);
                dataRemains = bytesRead > 0;
                int audioValue = BitConverter.ToInt32(sample, 0);
                _allValues.Add(audioValue);
            }
            while (dataRemains);

            ReRender();
        }

        private void ReRender()
        {
            _audioCanvas.Children.Clear();
            var dilutedValues = DiluteValues();
            Decorators.ForEach(d => d.Decorate(_audioCanvas, dilutedValues));
        }

        private int[] DiluteValues()
        {
            int totalPoints = (int)Math.Ceiling(_allValues.Count * 1.0 / _dilutionScale);
            int xPos = 0;
            var dilutedValues = new int[totalPoints];

            for (int index = 0; index < _allValues.Count - 1; index += _dilutionScale)
            {
                int audioValue = _allValues[index];
                dilutedValues[xPos] = audioValue;
                xPos++;
            }
            return dilutedValues;
        }

        public void Refresh()
        {
            ReRender();
        }
    }
}