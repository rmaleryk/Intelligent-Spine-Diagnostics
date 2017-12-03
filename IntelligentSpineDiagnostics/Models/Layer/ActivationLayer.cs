using IntelligentSpineDiagnostics.Models.ActivationFunctions;
using IntelligentSpineDiagnostics.Models.Neuron;

namespace IntelligentSpineDiagnostics.Models.Layer
{
    public class ActivationLayer : AbstractLayer
    {

        public new ActivationNeuron this[int index]
        {
            get { return (ActivationNeuron)neurons[index]; }
        }

        public ActivationLayer(int neuronsCount, int inputsCount, IActivationFunction function)
            : base(neuronsCount, inputsCount)
        {
            // create each neuron
            for (int i = 0; i < neuronsCount; i++)
                neurons[i] = new ActivationNeuron(inputsCount, function);
        }
    }
}