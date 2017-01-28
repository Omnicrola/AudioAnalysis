using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using NAudio.Wave;

namespace Lab_AudioAnalysis.Audio
{
    public class AudioCanvasRenderer
    {
        private readonly Canvas _audioCanvas;
        private readonly int _dilutionScale;
        private Polyline _polyLine;
        private int _width;
        private int _height;
        private List<int> _allValues;

        public Brush BrushColor { get { return _polyLine.Stroke; } set { _polyLine.Stroke = value; } }

        public int Width
        {
            get { return _width; }
            set
            {
                _width = value;
                _polyLine.MaxWidth = value;
                ReRender();
            }
        }

        public int Height
        {
            get { return _height; }
            set
            {
                _height = value;
                _polyLine.MaxHeight = value;
                ReRender();
            }
        }


        public AudioCanvasRenderer(Canvas audioCanvas, int dilutionScale, int width, int height)
        {
            _audioCanvas = audioCanvas;
            _dilutionScale = dilutionScale;
            _allValues = new List<int>();

            _width = width;
            _height = height;

            _polyLine = new Polyline();
            _polyLine.Stroke = Brushes.Blue;
            _polyLine.Name = "waveform";
            _polyLine.StrokeThickness = 1;
            _polyLine.MaxWidth = Width;
            _polyLine.MaxHeight = Height;
        }

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
            _polyLine.Points.Clear();

            var totalPoints = _allValues.Count / _dilutionScale;
            int xPos = 0;
            double maxX = 0;
            for (int index = 0; index < _allValues.Count - 1; index += _dilutionScale)
            {
                int audioValue = _allValues[index];
                var point = Normalize(totalPoints, xPos++, audioValue);
                _polyLine.Points.Add(point);
                maxX = Math.Max(maxX, point.X);
            }
            Console.WriteLine($"Max X: {maxX}");
            _audioCanvas.Children.Add(_polyLine);
        }

        private Point Normalize(int totalPoints, int xPos, int y)
        {
            var point = new Point();

            point.X = 1.0 * xPos / totalPoints * Width;
            point.Y = Height / 2.0 - y / (Int32.MaxValue * 1.0) * (Height / 2.0);
            return point;
        }

    }
}