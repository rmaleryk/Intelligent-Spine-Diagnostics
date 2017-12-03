using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
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
using IntelligentSpineDiagnostics.Services;
using IntelligentSpineDiagnostics.ViewModels;
using Microsoft.Win32;

namespace IntelligentSpineDiagnostics
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private LearningService _learningService;
        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = new MainViewModel();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            // Put serial ports to combobox
            SerialPortsBox.ItemsSource = SerialPort.GetPortNames();
            SerialPortsBox.SelectedIndex = 0;

            // Creating Learning Service
            _learningService = new LearningService(this.DataContext as MainViewModel);

        }

        private void Window_Closed(object sender, EventArgs e)
        {
            _learningService?.StopProcessing();
        }

        #region Diagnostic Tab Controllers

        private void ConnectBtn_Click(object sender, RoutedEventArgs e)
        {
            if (SerialPortsBox.IsEnabled)
            {
                SerialPortsBox.IsEnabled = false;
                ConnectBtn.Content = "Disconnect";
                MakeDiagnosisBtn.IsEnabled = true;
            }
            else
            {
                SerialPortsBox.IsEnabled = true;
                ConnectBtn.Content = "Connect";
                MakeDiagnosisBtn.IsEnabled = false;
            }
        }

        private void MakeDiagnosisBtn_Click(object sender, RoutedEventArgs e)
        {

        }

        #endregion

        #region NeuralNetwork Tab Controllers

        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9.]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void TeachingBtn_Click(object sender, RoutedEventArgs e)
        {
            if (TeachingSettingsBox.IsEnabled)
            {
                TeachingSettingsBox.IsEnabled = false;
                TeachingBtn.Content = "Stop";

                new Thread(_learningService.StartProcessing).Start();
            }
            else
            {
                TeachingSettingsBox.IsEnabled = true;
                TeachingBtn.Content = "Start teaching";
                _learningService.StopProcessing();
            }
        }

        private void BrowseBtn_Click(object sender, RoutedEventArgs e)
        {
            var fileDialog = new OpenFileDialog();
            fileDialog.ShowDialog();
            SelectedFileLabel.Content = fileDialog.FileName.Split('\\', '/').Last();
            (DataContext as MainViewModel).TrainingFilePath = fileDialog.FileName;
        }

        #endregion

    }
}
