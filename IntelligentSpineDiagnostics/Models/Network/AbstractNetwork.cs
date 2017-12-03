using System;
using IntelligentSpineDiagnostics.Models.Layer;

namespace IntelligentSpineDiagnostics.Models.Network
{
    public abstract class AbstractNetwork
    {

        public int InputsCount { get; protected set; }
        public int LayersCount { get; protected set; }
        protected AbstractLayer[] layers;
        public double[] Output { get; protected set; }

        public AbstractLayer this[int index]
        {
            get { return layers[index]; }
        }

        protected AbstractNetwork(int inputsCount, int layersCount)
        {
            this.InputsCount = Math.Max(1, inputsCount);
            this.LayersCount = Math.Max(1, layersCount);
            // create collection of layers
            layers = new AbstractLayer[this.LayersCount];
        }

        public virtual double[] Compute(double[] input)
        {
            Output = input;

            // compute each layer
            foreach (AbstractLayer layer in layers)
            {
                Output = layer.Compute(Output);
            }

            return Output;
        }

        public virtual void Randomize()
        {
            foreach (AbstractLayer layer in layers)
            {
                layer.Randomize();
            }
        }
    }
}