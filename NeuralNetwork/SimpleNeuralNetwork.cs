using System;
using System.Collections.Generic;
using System.Text;

namespace NeuralNetwork
{
    public class SimpleNeuralNetwork : NeuralNetwork
    {
        public double[][] Neurons { get; private set; }

        public Weight[][][] WeightsLayerBack { get; private set; }
        public Weight[][][] WeightsLayerFront { get; private set; }

        public SimpleNeuralNetwork(SimpleNeuralNetworkParameters parameters, 
            Activation activation, 
            params int[] lengthsOfEachLayer)
        {

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
            var rnd = new Random();
            var countLayer = lengthsOfEachLayer.Length;
            Neurons = new double[countLayer][];
            WeightsLayerBack = new Weight[countLayer][][];
            WeightsLayerFront = new Weight[countLayer][][];
            for (var layer = 0; layer < countLayer; layer++)
            {
                var lengthOfCurrentLayer = lengthsOfEachLayer[layer];
                var nextLayerIsOutOfRange = layer == countLayer - 1 ? true : false;
                Neurons[layer] = new double[lengthOfCurrentLayer];
                if (!nextLayerIsOutOfRange)
                {
                    WeightsLayerFront[layer] = new Weight[lengthOfCurrentLayer][];
                }
                for (var numNeuronInCurrentLayer = 0; numNeuronInCurrentLayer < lengthOfCurrentLayer; numNeuronInCurrentLayer++)
                {
                    Neurons[layer][numNeuronInCurrentLayer] = 0;
                    if (!nextLayerIsOutOfRange)
                    {
                        var lengthOfNextLayer = lengthsOfEachLayer[layer + 1];
                        WeightsLayerFront[layer][numNeuronInCurrentLayer] = new Weight[lengthOfCurrentLayer];
                        for (var numNeuronInNextLayer = 0; numNeuronInNextLayer < lengthOfNextLayer; numNeuronInNextLayer++)
                        {
                            WeightsLayerFront[layer][numNeuronInCurrentLayer][numNeuronInNextLayer].W = rnd.NextDouble();
                            WeightsLayerFront[layer][numNeuronInCurrentLayer][numNeuronInNextLayer].D = 0;
                        }
                    }
                }
            }
            InitializeWeightsLayerBack(lengthsOfEachLayer);
        }

        private void InitializeWeightsLayerBack(int[] lengthsOfEachLayer)
        {
            var countLayer = lengthsOfEachLayer.Length;
            WeightsLayerBack = new Weight[countLayer][][];
            for (var layer = 1; layer < countLayer; layer++)
            {
                var lengthOfPreviusLayer = lengthsOfEachLayer[layer - 1];
                WeightsLayerBack[layer] = new Weight[lengthOfPreviusLayer][];
                for (var numNeuronInPreviusLayer = 0; numNeuronInPreviusLayer < lengthOfPreviusLayer; numNeuronInPreviusLayer++)
                {
                    var lengthOfCurrentLayer = lengthsOfEachLayer[layer];
                    WeightsLayerFront[layer][numNeuronInPreviusLayer] = new Weight[lengthOfCurrentLayer];
                    for (var numNeuronInCurrentLayer = 0; numNeuronInCurrentLayer < lengthOfCurrentLayer; numNeuronInCurrentLayer++)
                    {
                        WeightsLayerBack[layer][numNeuronInPreviusLayer][numNeuronInCurrentLayer] =
                            WeightsLayerFront[layer - 1][numNeuronInPreviusLayer][numNeuronInCurrentLayer];
                    }
                }
            }
        }
    }

    public class SimpleNeuralNetworkParameters
    {

    }
}
