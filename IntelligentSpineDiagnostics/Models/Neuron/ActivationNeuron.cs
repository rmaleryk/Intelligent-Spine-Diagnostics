using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IntelligentSpineDiagnostics.Models.ActivationFunctions;

namespace IntelligentSpineDiagnostics.Models.Neuron
{
    public class ActivationNeuron : AbstractNeuron
    {

        public double Threshold { get; set; } = 0.0f;
        public IActivationFunction ActivationFunction { get; protected set; } = null;

        public ActivationNeuron(int inputs, IActivationFunction function) : base(inputs)
        {
            this.ActivationFunction = function;
        }

        public override void Randomize()
        {
            // randomize weights
            base.Randomize();
            // randomize threshold
            Threshold = RandGenerator.NextDouble() * (RandRange.Length) + RandRange.Min;
        }

        public override double Compute(double[] input)
        {
            // check for corrent input vector
            if (input.Length != InputsCount)
                throw new ArgumentException();

            // initial sum value
            double sum = 0.0;

            // compute weighted sum of inputs
            for (int i = 0; i < InputsCount; i++)
            {
                sum += Weights[i] * input[i];
            }
            sum += Threshold;

            return (Output = ActivationFunction.Function(sum));
        }
    }
}
