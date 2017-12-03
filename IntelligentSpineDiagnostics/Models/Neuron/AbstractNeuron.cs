using System;
using IntelligentSpineDiagnostics.Utils;

namespace IntelligentSpineDiagnostics.Models.Neuron
{
    public abstract class AbstractNeuron
    {
        public int InputsCount { get; } = 0;
        protected double[] Weights = null;
        public double Output { get; protected set; } = 0;
        public static Random RandGenerator { get; set; } = new Random((int) DateTime.Now.Ticks);
        public static DoubleRange RandRange { get; set; } = new DoubleRange(0.0, 1.0);

        // Neuron's weights accessor
        public double this[int index]
        {
            get => Weights[index];
            set => Weights[index] = value;
        }

        public AbstractNeuron(int inputs)
        {
            // allocate weights
            InputsCount = Math.Max(1, inputs);
            Weights = new double[InputsCount];
            // randomize the neuron
            Randomize();
        }

        public virtual void Randomize()
        {
            double d = RandRange.Length;

            // randomize weights
            for (int i = 0; i < InputsCount; i++)
                Weights[i] = RandGenerator.NextDouble() * d + RandRange.Min;
        }

        public abstract double Compute(double[] input);
    }
}