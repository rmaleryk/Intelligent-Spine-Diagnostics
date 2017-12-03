using System;
using IntelligentSpineDiagnostics.Models.Neuron;

namespace IntelligentSpineDiagnostics.Models.Layer
{
    public abstract class AbstractLayer
    {

        public int InputsCount { get; protected set; } = 0;
        public int NeuronsCount { get; protected set; } = 0;
        protected AbstractNeuron[] neurons;
        public double[] Output { get; protected set; }

        public AbstractNeuron this[int index]
        {
            get { return neurons[index]; }
        }

        protected AbstractLayer(int neuronsCount, int inputsCount)
        {
            this.InputsCount = Math.Max(1, inputsCount);
            this.NeuronsCount = Math.Max(1, neuronsCount);
            // create collection of neurons
            neurons = new AbstractNeuron[this.NeuronsCount];
            // allocate output array
            Output = new double[this.NeuronsCount];
        }

        public virtual double[] Compute(double[] input)
        {
            // compute each neuron
            for (int i = 0; i < NeuronsCount; i++)
                Output[i] = neurons[i].Compute(input);

            return Output;
        }

        public virtual void Randomize()
        {
            foreach (AbstractNeuron neuron in neurons)
                neuron.Randomize();
        }
    }
}