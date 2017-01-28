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

namespace Lab_AudioAnalysis.Ui
{
    /// <summary>
    /// Interaction logic for Recording.xaml
    /// </summary>
    public partial class Recording : UserControl
    {
        private readonly NAudioApiWrapper _nAudioApiWrapper;
        private IAudioRecorder _audioRecorder;
        private AudioFileRenderer _audioFileRenderer;
        private DynamicAudioRenderer _dynamicAudioRenderer;
        private int recordingNumber;

        public event EventHandler<LogEventArgs> LogEvent;

        public Recording()
        {
            InitializeComponent();
            _nAudioApiWrapper = new NAudioApiWrapper();
            _audioFileRenderer = new AudioFileRenderer();
            _dynamicAudioRenderer = new DynamicAudioRenderer(AudioCanvas, 1000, 500);
            DataContext = _dynamicAudioRenderer;
        }


        private void Start_OnClick(object sender, RoutedEventArgs e)
        {
            Log("Starting  recording...");
            var firstDevice = _nAudioApiWrapper.InputDevices.FirstOrDefault();
            if (firstDevice == null)
            {
                Log("Error, no recording devices found!");
            }
            else
            {
                this.recordingNumber++;
                _audioRecorder = firstDevice.CreateRecording("temp00" + this.recordingNumber + ".wav");
                _audioRecorder.BytesAvailable += _dynamicAudioRenderer.StreamBytes;
                _audioRecorder.Start();

                StopButton.IsEnabled = true;
                StartButton.IsEnabled = false;
            }
        }

        private void Log(string message)
        {
            LogEvent?.Invoke(this, new LogEventArgs(message));
        }

        private void Stop_OnClick(object sender, RoutedEventArgs e)
        {
            Log("Stopping...");
            _audioRecorder.Stop();

            Log($"Recorded {_audioRecorder.TotalBytesRecorded}bytes " + _audioRecorder.TotalTime);

            StopButton.IsEnabled = false;
            StartButton.IsEnabled = true;
        }

        private void Replay_OnClick(object sender, RoutedEventArgs e)
        {
            _audioRecorder.AudioRecording.PlayFromBeginning();
        }
    }
}
