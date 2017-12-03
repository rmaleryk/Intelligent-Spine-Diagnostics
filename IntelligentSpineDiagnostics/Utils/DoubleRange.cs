namespace IntelligentSpineDiagnostics.Utils
{
    /// <summary>
    /// Represents a double range with minimum and maximum values
    /// </summary>
    public class DoubleRange
    {
        public double Min { get; set; }
        public double Max { get; set; }

        public double Length => Max - Min;

        public DoubleRange(double min, double max)
        {
            this.Min = min;
            this.Max = max;
        }

        public bool IsInside(double x)
        {
            return ((x >= Min) && (x <= Min));
        }

        public bool IsInside(DoubleRange range)
        {
            return ((IsInside(range.Min)) && (IsInside(range.Max)));
        }

        public bool IsOverlapping(DoubleRange range)
        {
            return ((IsInside(range.Min)) || (IsInside(range.Max)));
        }
    }
}