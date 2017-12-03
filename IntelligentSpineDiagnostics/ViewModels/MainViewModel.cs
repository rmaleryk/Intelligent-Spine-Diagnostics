using System.ComponentModel;

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
                _averageError = value;
                OnPropertyChanged("AverageError");
            }
        }

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}