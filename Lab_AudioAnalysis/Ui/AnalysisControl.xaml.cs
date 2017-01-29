using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Lab_AudioAnalysis.Audio;
using Lab_AudioAnalysis.Audio.Decorators;
using Lab_AudioAnalysis.Ui.FilePicker;
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
            _fastFourierTransformRenderer = new FastFourierTransformRenderer(FftAudioCanvas);
            _backgroundFilterDecorator = new BackgroundFilterDecorator()
            {
                BrushColor = Brushes.Chartreuse,
                Floor = 100
            };
            _audioCanvasRenderer.Decorators.Add(_backgroundFilterDecorator);
        }

        private void Log(string message)
        {
            LogEvent?.Invoke(this, new LogEventArgs(message));
        }

        private void OnFileSelected(object sender, FileSelectEventArgs e)
        {
            using (WaveFileReader waveReader = new WaveFileReader(e.Filename))
            {
                _audioCanvasRenderer.Render(waveReader);
                _fastFourierTransformRenderer.Draw(waveReader);
            }
        }

        private void OnCanvasResize(object sender, SizeChangedEventArgs e)
        {
            _audioCanvasRenderer.Refresh();
        }
    }
}