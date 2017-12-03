using System;
using System.Linq;
using IntelligentSpineDiagnostics.Models;

namespace IntelligentSpineDiagnostics.Utils
{
    public class CsvConverter
    {
        public static Dataset ToDouble(string[] lines)
        {
            int count = 0;
            var x = new double[lines.Length][];
            var y = new double[lines.Length][];

            foreach (var line in lines)
            {
                var strArray = line.Split(',');

                var tempX = strArray.ToList().GetRange(0, strArray.Length - 3);
                var tempY = strArray.ToList().GetRange(strArray.Length - 3, 3);

                try
                {
                    x[count] = tempX.ConvertAll(item => Double.Parse(item)).ToArray();
                    y[count] = tempY.ConvertAll(item => Double.Parse(item)).ToArray();
                }
                catch (Exception ex)
                {
                    
                }

                count++;
            }

            return new Dataset(x, y);
        }
    }
}