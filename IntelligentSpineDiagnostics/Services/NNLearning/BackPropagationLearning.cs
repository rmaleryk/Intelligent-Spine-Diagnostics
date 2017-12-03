using System;
using IntelligentSpineDiagnostics.Models.ActivationFunctions;
using IntelligentSpineDiagnostics.Models.Layer;
using IntelligentSpineDiagnostics.Models.Network;
using IntelligentSpineDiagnostics.Models.Neuron;

namespace IntelligentSpineDiagnostics.Services.NNLearning
{
    public class BackPropagationLearning : ISupervisedLearning
    {

        private ActivationNetwork network;
        private double _learningRate = 0.1;
        private double _momentum = 0.0;

        private double[][] _neuronErrors = null;
        private double[][][] _weightsUpdates = null;
        private double[][] _thresholdsUpdates = null;

        public double LearningRate
        {
            get { return _learningRate; }
            set
            {
                _learningRate = Math.Max(0.0, Math.Min(1.0, value));
            }
        }

        public double Momentum
        {
            get { return _momentum; }
            set
            {
                _momentum = Math.Max(0.0, Math.Min(1.0, value));
            }
        }

        public BackPropagationLearning(ActivationNetwork network)
        {
            this.network = network;

            // create error and deltas arrays
            _neuronErrors = new double[network.LayersCount][];
            _weightsUpdates = new double[network.LayersCount][][];
            _thresholdsUpdates = new double[network.LayersCount][];

            // initialize errors and deltas arrays for each layer
            for (int i = 0, n = network.LayersCount; i < n; i++)
            {
                AbstractLayer layer = network[i];

                _neuronErrors[i] = new double[layer.NeuronsCount];
                _weightsUpdates[i] = new double[layer.NeuronsCount][];
                _thresholdsUpdates[i] = new double[layer.NeuronsCount];

                // for each neuron
                for (int j = 0; j < layer.NeuronsCount; j++)
                {
                    _weightsUpdates[i][j] = new double[layer.InputsCount];
                }
            }
        }

        public double Run(double[] input, double[] output)
        {
            // compute the network's output
            network.Compute(input);

            // calculate network error
            double error = CalculateError(output);

            // calculate weights updates
            CalculateUpdates(input);

            // update the network
            UpdateNetwork();

            return error;
        }

        public double RunEpoch(double[][] input, double[][] output)
        {
            double error = 0.0;

            // run learning procedure for all samples
            for (int i = 0, n = input.Length; i < n; i++)
            {
                error += Run(input[i], output[i]);
            }

            // return summary error
            return error;
        }

        private double CalculateError(double[] desiredOutput)
        {
            // current and the next layers
            ActivationLayer layer, layerNext;

            // current and the next errors arrays
            double[] errors, errorsNext;

            double error = 0, e, sum;
            double output;
            int layersCount = network.LayersCount;

            // assume, that all neurons of the network have the same activation function
            IActivationFunction function = network[0][0].ActivationFunction;

            // calculate error values for the last layer first
            layer = network[layersCount - 1];
            errors = _neuronErrors[layersCount - 1];

            for (int i = 0, n = layer.NeuronsCount; i < n; i++)
            {
                output = layer[i].Output;
                // error of the neuron
                e = desiredOutput[i] - output;
                // error multiplied with activation function's derivative
                errors[i] = e * function.Derivative2(output);
                // squre the error and sum it
                error += (e * e);
            }

            // calculate error values for other layers
            for (int j = layersCount - 2; j >= 0; j--)
            {
                layer = network[j];
                layerNext = network[j + 1];
                errors = _neuronErrors[j];
                errorsNext = _neuronErrors[j + 1];

                // for all neurons of the layer
                for (int i = 0, n = layer.NeuronsCount; i < n; i++)
                {
                    sum = 0.0;
                    // for all neurons of the next layer
                    for (int k = 0, m = layerNext.NeuronsCount; k < m; k++)
                    {
                        sum += errorsNext[k] * layerNext[k][i];
                    }
                    errors[i] = sum * function.Derivative2(layer[i].Output);
                }
            }

            // return squared error of the last layer divided by 2
            return error / 2.0;
        }

        private void CalculateUpdates(double[] input)
        {
            // current neuron
            ActivationNeuron neuron;
            // current and previous layers
            ActivationLayer layer, layerPrev;

            double[][] layerWeightsUpdates;
            double[] layerThresholdUpdates;
            double[] errors;
            double[] neuronWeightUpdates;
            double error;

            // 1 - calculate updates for the last layer fisrt
            layer = network[0];
            errors = _neuronErrors[0];
            layerWeightsUpdates = _weightsUpdates[0];
            layerThresholdUpdates = _thresholdsUpdates[0];

            // for each neuron of the layer
            for (int i = 0, n = layer.NeuronsCount; i < n; i++)
            {
                neuron = layer[i];
                error = errors[i];
                neuronWeightUpdates = layerWeightsUpdates[i];

                // for each weight of the neuron
                for (int j = 0, m = neuron.InputsCount; j < m; j++)
                {
                    // calculate weight update
                    neuronWeightUpdates[j] = _learningRate * (
                        _momentum * neuronWeightUpdates[j] +
                        (1.0 - _momentum) * error * input[j]
                        );
                }

                // calculate treshold update
                layerThresholdUpdates[i] = _learningRate * (
                    _momentum * layerThresholdUpdates[i] +
                    (1.0 - _momentum) * error
                    );
            }

            // 2 - for all other layers
            for (int k = 1, l = network.LayersCount; k < l; k++)
            {
                layerPrev = network[k - 1];
                layer = network[k];
                errors = _neuronErrors[k];
                layerWeightsUpdates = _weightsUpdates[k];
                layerThresholdUpdates = _thresholdsUpdates[k];

                // for each neuron of the layer
                for (int i = 0, n = layer.NeuronsCount; i < n; i++)
                {
                    neuron = layer[i];
                    error = errors[i];
                    neuronWeightUpdates = layerWeightsUpdates[i];

                    // for each synapse of the neuron
                    for (int j = 0, m = neuron.InputsCount; j < m; j++)
                    {
                        // calculate weight update
                        neuronWeightUpdates[j] = _learningRate * (
                            _momentum * neuronWeightUpdates[j] +
                            (1.0 - _momentum) * error * layerPrev[j].Output
                            );
                    }

                    // calculate treshold update
                    layerThresholdUpdates[i] = _learningRate * (
                        _momentum * layerThresholdUpdates[i] +
                        (1.0 - _momentum) * error
                        );
                }
            }
        }

        private void UpdateNetwork()
        {
            ActivationNeuron neuron;
            ActivationLayer layer;
            double[][] layerWeightsUpdates;
            double[] layerThresholdUpdates;
            double[] neuronWeightUpdates;

            // for each layer of the network
            for (int i = 0, n = network.LayersCount; i < n; i++)
            {
                layer = network[i];
                layerWeightsUpdates = _weightsUpdates[i];
                layerThresholdUpdates = _thresholdsUpdates[i];

                // for each neuron of the layer
                for (int j = 0, m = layer.NeuronsCount; j < m; j++)
                {
                    neuron = layer[j];
                    neuronWeightUpdates = layerWeightsUpdates[j];

                    // for each weight of the neuron
                    for (int k = 0, s = neuron.InputsCount; k < s; k++)
                    {
                        // update weight
                        neuron[k] += neuronWeightUpdates[k];
                    }
                    // update treshold
                    neuron.Threshold += layerThresholdUpdates[j];
                }
            }
        }
    }
}