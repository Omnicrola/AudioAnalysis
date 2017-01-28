using System;

namespace Lab_AudioAnalysis.Ui.FilePicker
{
    public class FileSelectEventArgs:EventArgs
    {
        public FileSelectEventArgs(string filename)
        {
            Filename = filename;
        }

        public string Filename { get; private set; }
    }
}