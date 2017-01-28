using System;

namespace Lab_AudioAnalysis.Ui
{
    public class LogEventArgs : EventArgs
    {
        public LogEventArgs(string logMessage)
        {
            LogMessage = logMessage;
        }

        public string LogMessage { get; private set; }
    }
}