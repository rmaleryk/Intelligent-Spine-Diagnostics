using System;
using System.ComponentModel;
using System.Threading;
using System.Windows;
using System.Windows.Media;
using System.Windows.Threading;
using InteractiveDataDisplay.WPF;

namespace IntelligentSpineDiagnostics.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public double LearningRate { get; set; } = 0.05;
        public double Momentum { get; set; } = 0;
        public double SigmoidsAlpha { get; set; } = 1;
        public double LearningErrorLimit { get; set; } = 0.1;
        public double EpochesLimit { get; set; } = 0;
        public string TrainingFilePath { get; set; } = "data.csv";
        public LineGraph ErrorGraph { get; set; }
        private MainWindow _mw;
        private double _epoches = 0;
        private double _averageError = 0;

        public double Epoches
        {
            get { return _epoches; }
            set
            {
                _epoches = value;
                OnPropertyChanged("Epoches");
            }
        }

        public double AverageError
        {
            get { return _averageError; }
            set
            {
                Application.Current.Dispatcher.BeginInvoke(
                    DispatcherPriority.SystemIdle,
                    new Action(() =>
                    {
                        _mw.ErrorGraph.Points.Add(new Point(Epoches, AverageError));
                    }));

                _averageError = value;
                OnPropertyChanged("AverageError");
            }
        }

        public MainViewModel(MainWindow mw)
        {
            _mw = mw;
        }

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}