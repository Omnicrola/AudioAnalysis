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
using Microsoft.Win32;

namespace Lab_AudioAnalysis.Ui.FilePicker
{
    /// <summary>
    /// Interaction logic for FilePickerControl.xaml
    /// </summary>
    public partial class FilePickerControl : UserControl
    {
        public static readonly DependencyProperty SelectedFilenameProperty =
            DependencyProperty.Register("SelectedFilename", typeof(string), typeof(FilePickerControl),
                new PropertyMetadata(default(string)));


        public event EventHandler<FileSelectEventArgs> FileSelected;

        public FilePickerControl()
        {
            InitializeComponent();
            LayoutRoot.DataContext = this;
        }

        public string SelectedFilename
        {
            get { return (string)GetValue(SelectedFilenameProperty); }
            set { SetValue(SelectedFilenameProperty, value); }
        }

        private void Browse_OnClick(object sender, RoutedEventArgs e)
        {
            var openFileDialog = new OpenFileDialog();
            openFileDialog.Multiselect = false;
            openFileDialog.AddExtension = true;
            openFileDialog.DefaultExt = "wav";
            openFileDialog.Filter = "Waveform | *.wav";

            var dialogResult = openFileDialog.ShowDialog();
            if (dialogResult.HasValue && dialogResult.Value)
            {
                var selectedFilename = openFileDialog.FileName;
                SelectedFilename = selectedFilename;
                FileSelected?.Invoke(this, new FileSelectEventArgs(selectedFilename));
            }
        }
    }
}