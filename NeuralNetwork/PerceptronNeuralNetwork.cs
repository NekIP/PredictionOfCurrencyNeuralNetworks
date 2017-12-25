using NeuralNetwork.Math;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NeuralNetwork
{
    public class PerceptronNeuralNetwork : NeuralNetwork
    {
        public double[][] Neurons { get; private set; }
        public Weight[][][] Weights { get; private set; }
        public Weight[][][] WeightsTranspose { get; private set; }

        private readonly MathHelper mathHelper = new MathHelper();

        public PerceptronNeuralNetwork(SimpleNeuralNetworkParameters parameters, 
            Activation activation, 
            params int[] lengthsOfEachLayer)
        {
            InitializeMatrixes(lengthsOfEachLayer);
            var t = 0;
        }

        public override double[] Run(double[] input)
        {
            throw new NotImplementedException();
        }

        public override NeuralNetworkLearnResult Learn(double[] input, double[] ideal)
        {
            throw new NotImplementedException();
        }

        private void InitializeMatrixes(int[] lengthsOfEachLayer)
        {
            
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
                        Weights[i][j][k].W = rnd.NextDouble();
                        Weights[i][j][k].D = 0;
                    }
                }
                WeightsTranspose[i] = mathHelper.Transpose(Weights[i]);
            }
        }
    }

    public class SimpleNeuralNetworkParameters
    {

    }
}
