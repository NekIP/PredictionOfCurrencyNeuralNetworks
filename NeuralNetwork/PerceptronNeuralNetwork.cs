using NeuralNetwork.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NeuralNetwork
{
    public class PerceptronNeuralNetwork : NeuralNetwork
    {
        public double[][] Neurons { get; private set; }

        /// <summary>
        /// Weigths for i(layer), for link from j(neuron in i layer) to k(neuronin (i + 1) layer)
        /// </summary>
        public Weight[][][] Weights { get; private set; }

        /// <summary>
        /// Weigths for i(layer), for link from j(neuron in (i + 1) layer) to k(neuronin i layer)
        /// </summary>
        public Weight[][][] WeightsTranspose { get; private set; }

        private readonly MathHelper mathHelper = new MathHelper();

        public PerceptronNeuralNetwork(PeceptronNeuralNetworkParameters parameters, 
            Activation activation, 
            params int[] lengthsOfEachLayer)
        {
            InitializeNeurons(lengthsOfEachLayer);
            InitializeWeigths(lengthsOfEachLayer);
        }

        public override double[] Run(double[] input)
        {
            SetInputNeuronsAndClear(input);
            for (var i = 0; i < Neurons.Length - 1; i++)
            {
                Neurons[i + 1] = Mull(WeightsTranspose[i], Neurons[i]);
            }
            return Neurons.Last();
        }

        public override NeuralNetworkLearnResult Learn(double[] input, double[] ideal)
        {
            var result = new NeuralNetworkLearnResult
            {
                Value = Run(input)
            };

            return result;
        }

        private double[] Mull(Weight[][] weightsMatrix, double[] vector)
        {
            var result = new double[weightsMatrix.Length];
            for (int i = 0; i < weightsMatrix.Length; i++)
            {
                for (int j = 0; j < vector.Length; j++)
                {
                    result[i] += weightsMatrix[i][j].W * vector[j];
                }
                result[i] = Activation.Convert(result[i], ActivationCoefficient);
            }
            return result;
        }

        private void InitializeNeurons(int[] lengthsOfEachLayer)
        {
            var countNeuronLayer = lengthsOfEachLayer.Length;
            Neurons = new double[countNeuronLayer][];
            for (var i = 0; i < countNeuronLayer; i++)
            {
                Neurons[i] = new double[lengthsOfEachLayer[i]];
            }
        }

        private void SetInputNeuronsAndClear(double[] input)
        {
            if (input.Length != Neurons[0].Length)
            {
                throw new ArithmeticException("Lengths of input vector and length of first row in Neurons must be equals");
            }
            Neurons[0] = input;
            for (var i = 1; i < Neurons.Length; i++)
            {
                for (var j = 0; j < Neurons[i].Length; j++)
                {
                    Neurons[i][j] = 0;
                }
            }
        }

        private void InitializeWeigths(int[] lengthsOfEachLayer)
        {
            var rnd = new Random();
            var countWeigthLayer = lengthsOfEachLayer.Length - 1;
            Weights = new Weight[countWeigthLayer][][];
            WeightsTranspose = new Weight[countWeigthLayer][][];
            for (var i = 0; i < countWeigthLayer; i++)
            {
                Weights[i] = new Weight[lengthsOfEachLayer[i]][];
                for (var j = 0; j < Weights[i].Length; j++)
                {
                    Weights[i][j] = new Weight[lengthsOfEachLayer[i + 1]];
                    for (var k = 0; k < Weights[i][j].Length; k++)
                    {
                        Weights[i][j][k] = new Weight(rnd.NextDouble(), 0);
                    }
                }
                WeightsTranspose[i] = mathHelper.Transpose(Weights[i]);
            }
        }
    }

    public class PeceptronNeuralNetworkParameters
    {
        public double ActivationCoefficient { get; set; } = 1;
    }
}
