using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Windows;
using IntelligentSpineDiagnostics.Models.ActivationFunctions;
using IntelligentSpineDiagnostics.Models.Network;
using IntelligentSpineDiagnostics.Services.NNLearning;
using IntelligentSpineDiagnostics.Utils;
using IntelligentSpineDiagnostics.ViewModels;

namespace IntelligentSpineDiagnostics.Services
{
    public class LearningService
    {
        private MainViewModel viewModel;
        private bool _isNeedStop;
        public ActivationNetwork Network { get; set; }
        public double SigmoidAlphaValue { get; set; }
        public double LearningRate { get; set; }
        public double Momentum { get; set; }

        public LearningService(MainViewModel viewModel)
        {
            this.viewModel = viewModel;
        }

        public void StartProcessing()
        {
            _isNeedStop = false;
            SigmoidAlphaValue = viewModel.SigmoidsAlpha;
            LearningRate = viewModel.LearningRate;
            Momentum = viewModel.Momentum;

            var lines = File.ReadAllLines(viewModel.TrainingFilePath);
            
            // Initialize input and output values
            var dataset = CsvConverter.ToDouble(lines);

            // Layers (without input layer, contains output layer)
            int[] layers = { 20, dataset.Outputs[0].Length };

            // Create perceptron
            Network = new ActivationNetwork(new SigmoidFunction(SigmoidAlphaValue), dataset.Inputs[0].Length, layers);

            // Create teacher
            var teacher = new BackPropagationLearning(Network)
            {
                LearningRate = LearningRate,
                Momentum = Momentum
            };

            int iteration = 1;

            try
            {
                List<double> errorsList = new List<double>();

                while (!_isNeedStop)
                {
                    // run epoch of learning procedure
                    double error = teacher.RunEpoch(dataset.Inputs, dataset.Outputs);
                    errorsList.Add(error);


                    // show current iteration & error
                    viewModel.Epoches = iteration;
                    viewModel.AverageError = error;

                    iteration++;

                    // check if we need to stop
                    if (error <= viewModel.LearningErrorLimit)
                        break;

                    if (iteration > viewModel.EpochesLimit && viewModel.EpochesLimit > 0)
                        break;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }



        }

        public void StopProcessing()
        {
            _isNeedStop = true;
        }
    }
}