namespace IntelligentSpineDiagnostics.Models
{
    public class Dataset
    {
        public double[][] Inputs { get; set; }
        public double[][] Outputs { get; set; }

        public Dataset(double[][] x, double[][] y)
        {
            Inputs = x;
            Outputs = y;
        }
    }
}