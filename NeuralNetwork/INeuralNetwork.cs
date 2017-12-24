using System;

namespace NeuralNetwork
{
    public interface INeuralNetwork
    {
        double[] Run(double[] input);
        NeuralNetworkLearnResult Learn(double[] input, double[] ideal);

    }
}
