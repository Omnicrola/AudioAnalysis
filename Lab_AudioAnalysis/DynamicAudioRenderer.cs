using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using NAudio.Wave;

namespace Lab_AudioAnalysis
{
    public class DynamicAudioRenderer : INotifyPropertyChanged
    {
        private static int POINTS_TO_DISPLAY = 2205;

        private readonly Canvas _audioCanvas;
        private readonly int _width;
        private readonly int _height;
        private Polyline _polyLine;
        private List<byte> _totalBytes;
        private Queue<int> _displayValues;
        private double _averageLevel;
        private double _maxLevel;
        private double _minLevel;

        public double AverageLevel
        {
            get { return _averageLevel; }
            private set { SetField(value, ref _averageLevel); }
        }

        public double MaxLevel
        {
            get { return _maxLevel; }
            private set { SetField(value, ref _maxLevel); }
        }

        public double MinLevel
        {
            get { return _minLevel; }
            private set { SetField(value, ref _minLevel); }
        }

        public DynamicAudioRenderer(Canvas audioCanvas, int width, int height)
        {
            _audioCanvas = audioCanvas;
            _width = width;
            _height = height;
            _totalBytes = new List<byte>();
            _displayValues = new Queue<Int32>();

            _polyLine = new Polyline();
            _polyLine.Stroke = Brushes.Blue;
            _polyLine.Name = "waveform";
            _polyLine.StrokeThickness = 1;
            _polyLine.MaxWidth = _width;
            _polyLine.MaxHeight = _height;
        }

        public void StreamBytes(object sender, WaveInEventArgs eventArgs)
        {
            _totalBytes.AddRange(eventArgs.Buffer);

            RecordSampleValue(eventArgs);

            _audioCanvas.Children.Clear();
            _polyLine.Points.Clear();

            var displayValues = _displayValues.ToArray();
            float min = float.MaxValue;
            float max = 0;
            AverageLevel = _displayValues.Average();
            for (int i = 0; i < displayValues.Length; i++)
            {
                var displayValue = displayValues[i];
                min = Math.Min(min, displayValue);
                max = Math.Max(max, displayValue);
                _polyLine.Points.Add(Normalize(i, displayValue));
            }
            MaxLevel = max;
            MinLevel = min;
            _audioCanvas.Children.Add(_polyLine);
        }

        private Point Normalize(int xPos, int y)
        {
            var point = new Point();
            var width = _width;//_audioCanvas.Width;
            var height = _height;//_audioCanvas.Height;

            point.X = 1.0 * xPos / POINTS_TO_DISPLAY * width;
            point.Y = height / 2.0 - y / (Int32.MaxValue * 1.0) * (height / 2.0);
            return point;
        }

        private void RecordSampleValue(WaveInEventArgs e)
        {
            byte[] sample = new byte[4];
            for (int i = 0; i < e.BytesRecorded - 1; i += 100)
            {
                sample[0] = e.Buffer[i];
                sample[1] = e.Buffer[i + 1];
                sample[2] = e.Buffer[i + 2];
                sample[3] = e.Buffer[i + 3];

                var sampleValue = BitConverter.ToInt32(sample, 0);
                if (_displayValues.Count < POINTS_TO_DISPLAY)
                {
                    _displayValues.Enqueue(sampleValue);
                }
                else
                {
                    _displayValues.Dequeue();
                    _displayValues.Enqueue(sampleValue);
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected void SetField<T>(T value, ref T field, [CallerMemberName] string callerName = null)
        {
            if (!EqualityComparer<T>.Default.Equals(field, value))
            {
                field = value;
                OnPropertyChanged(callerName);
            }
        }
    }
}