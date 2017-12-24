using System;

namespace NeuralNetwork
{
    public abstract class NeuralNetwork
    {
        public Func<double, double, double> Activation { get; private set; }

        public abstract double[] Run(double[] input);

        public abstract NeuralNetworkLearnResult Learn(double[] input, double[] ideal);

        protected double SigmoidActivation(double input, double koef = 1) => 1 / (1 + Math.Exp(-koef * input));

        protected double InversSigmoidActivation(double input, double koef = 1) => -Math.Log(1 / input - 1) / koef;

        protected double HyperbolicActivation(double input, double koef = 1)
        {
            var exp = Math.Exp(2 * koef * input);
            return (exp - 1) / (exp + 1);
        }

        protected double InversHyperbolicActivation(double input, double koef = 1) => 
            Math.Log((1 + 2 * input) / (1 - 2 * input)) / (2 * koef);
    }
}
