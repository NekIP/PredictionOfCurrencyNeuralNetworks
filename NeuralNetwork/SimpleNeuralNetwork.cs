using System;
using System.Collections.Generic;
using System.Text;

namespace NeuralNetwork
{
    public class SimpleNeuralNetwork : NeuralNetwork
    {
        public SimpleNeuralNetwork(SimpleNeuralNetworkParameters parameters, 
            Activation activation, 
            params int[] lengthOfEachLayer)
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
    }

    public class SimpleNeuralNetworkParameters
    {

    }
}
