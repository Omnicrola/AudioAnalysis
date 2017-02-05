using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;

namespace Lab_AudioAnalysis.Ui
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        private string _logData;

        public MainWindow()
        {
            InitializeComponent();
        }

        public string LogData
        {
            get { return _logData; }
            set { SetField(value, ref _logData); }
        }

        #region PropertyChanged

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

        #endregion

        private void OnLogEvent(object sender, LogEventArgs e)
        {
            var dateTime = DateTime.Now;
            LogData += $"{dateTime} - {e.LogMessage}\n";
            LogTextBox.ScrollToEnd();
        }
    }
}