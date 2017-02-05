using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Lab_AudioAnalysis.Audio;
using Lab_AudioAnalysis.Audio.Decorators;
using Lab_AudioAnalysis.Ui.FilePicker;
using NAudio.Dsp;
using NAudio.Wave;

namespace Lab_AudioAnalysis.Ui
{
    /// <summary>
    /// Interaction logic for AnalysisControl.xaml
    /// </summary>
    public partial class AnalysisControl : UserControl
    {
        private readonly AudioCanvasRenderer _audioCanvasRenderer;
        private BackgroundFilterDecorator _backgroundFilterDecorator;
        private FastFourierTransformRenderer _fastFourierTransformRenderer;
        private BiQuadFilter _lowPassFilter;
        private BiQuadFilter _highPassFilter;
        public event EventHandler<LogEventArgs> LogEvent;

        public double FloorValue
        {
            get { return _backgroundFilterDecorator.Floor; }
            set { _backgroundFilterDecorator.Floor = value; _audioCanvasRenderer.Refresh(); }
        }

        public AnalysisControl()
        {
            InitializeComponent();
            DataContext = this;
            _audioCanvasRenderer = new AudioCanvasRenderer(AudioCanvas, 10, 500, 500);
            _audioCanvasRenderer.Decorators.Add(new PassthroughDecorator() { BrushColor = Brushes.CornflowerBlue });
            _audioCanvasRenderer.Decorators.Add(new SmoothingDecorator(10) { BrushColor = Brushes.OrangeRed });
            _backgroundFilterDecorator = new BackgroundFilterDecorator()
            {
                BrushColor = Brushes.Chartreuse,
                Floor = 100
            };
            _audioCanvasRenderer.Decorators.Add(_backgroundFilterDecorator);

            _lowPassFilter = BiQuadFilter.LowPassFilter(44100, 100, 1);
            _highPassFilter = BiQuadFilter.HighPassFilter(44100, 1000, 1);
            List<BiQuadFilter> filters = new List<BiQuadFilter>
            {
                _lowPassFilter,
                _highPassFilter
            };
            _fastFourierTransformRenderer = new FastFourierTransformRenderer(FftAudioCanvas, filters);
            HighPassSlider.Value = 20000;
            LowPassSlider.Value = 20000;
        }

        private void Log(string message)
        {
            LogEvent?.Invoke(this, new LogEventArgs(message));
        }

        private void OnFileSelected(object sender, FileSelectEventArgs e)
        {
            ReRender();
        }

        private void ReRender()
        {

            var selectedFilename = FilePicker.SelectedFilename;
            if (string.IsNullOrEmpty(selectedFilename)) return;
            using (WaveFileReader waveReader = new WaveFileReader(selectedFilename))
            {
                _audioCanvasRenderer.Render(waveReader);
                _fastFourierTransformRenderer.Draw(waveReader);
            }
        }

        private void OnCanvasResize(object sender, SizeChangedEventArgs e)
        {
            ReRender();
        }

        private void LowPass_OnValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            var newValue = e.NewValue;
            _lowPassFilter.SetLowPassFilter(44100, (float)newValue, 1);
            ReRender();
        }

        private void HighPass_OnValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            var newValue = e.NewValue;
            _highPassFilter.SetHighPassFilter(44100, (float)newValue, 1);
            ReRender();
        }
    }
}