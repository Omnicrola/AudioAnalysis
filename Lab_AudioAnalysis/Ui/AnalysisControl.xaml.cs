using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Lab_AudioAnalysis.Audio;
using Lab_AudioAnalysis.Ui.FilePicker;
using NAudio.Wave;

namespace Lab_AudioAnalysis.Ui
{
    /// <summary>
    /// Interaction logic for AnalysisControl.xaml
    /// </summary>
    public partial class AnalysisControl : UserControl
    {
        private readonly VoiceSampleAnalyzer _voiceSampleAnalyzer;
        private AudioCanvasRenderer _audioCanvasRenderer;
        public event EventHandler<LogEventArgs> LogEvent;
        public AnalysisControl()
        {
            InitializeComponent();
            _voiceSampleAnalyzer = new VoiceSampleAnalyzer();
            _audioCanvasRenderer = new AudioCanvasRenderer(AudioCanvas, 100, 500, 500);
        }

        private void Log(string message)
        {
            LogEvent?.Invoke(this, new LogEventArgs(message));
        }

        private void OnFileSelected(object sender, FileSelectEventArgs e)
        {
            using (WaveFileReader waveReader = new WaveFileReader(e.Filename))
            {
                //                _voiceSampleAnalyzer.Render(waveReader, AudioCanvas);
                _audioCanvasRenderer.Render(waveReader);
            }
        }

        private void OnCanvasResize(object sender, SizeChangedEventArgs e)
        {
            _audioCanvasRenderer.Width = (int)e.NewSize.Width;
            _audioCanvasRenderer.Height = (int)e.NewSize.Height;
        }
    }

    public class VoiceSampleAnalyzer
    {
        public void Render(WaveFileReader waveReader, Canvas audioCanvas)
        {
            audioCanvas.Children.Clear();
            var polyline = new Polyline();

        }
    }
}
