using System;
using System.Windows;
using System.Windows.Controls;
using Lab_AudioAnalysis.Audio.Api;
using Lab_AudioAnalysis.Audio.Api.Implementation;

namespace Lab_AudioAnalysis.Ui
{

    public partial class RealTimeFilteringControl : UserControl
    {
        private IAudioPlayer _audioPlayer;
        public event EventHandler<LogEventArgs> LogEvent;

        public RealTimeFilteringControl()
        {
            InitializeComponent();
            var nAudioApiWrapper = new NAudioApiWrapper();
            nAudioApiWrapper.InputDevices.ForEach(d => InputDevicesComboBox.Items.Add(d));
            InputDevicesComboBox.SelectedItem = InputDevicesComboBox.Items[0];

            nAudioApiWrapper.OutputDevices.ForEach(d => OutputDevicesComboBox.Items.Add(d));
            OutputDevicesComboBox.SelectedItem = OutputDevicesComboBox.Items[0];
        }

        private void Log(string message)
        {
            LogEvent?.Invoke(this, new LogEventArgs(message));
        }

        private void RecordButton_OnClick(object sender, RoutedEventArgs e)
        {
            if (_audioPlayer == null)
            {
                var audioOutputDevice = OutputDevicesComboBox.SelectedItem as IAudioOutputDevice;
                var audioInputDevice = InputDevicesComboBox.SelectedItem as IAudioInputDevice;

                _audioPlayer = audioOutputDevice.CreatePlayer(audioInputDevice);
            }

            if (_audioPlayer.IsPlaying)
            {
                _audioPlayer.Stop();
                RecordButton.Content = "Start";
            }
            else
            {
                _audioPlayer.Start();
                RecordButton.Content = "Stop";
            }
        }
    }
}
