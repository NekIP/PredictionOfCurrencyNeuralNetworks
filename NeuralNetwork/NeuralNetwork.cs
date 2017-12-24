using System;

namespace NeuralNetwork
{
    public abstract class NeuralNetwork
    {
        /// <summary>
        /// Activation function convert input value in to range [0, 1] (if sigmoid) or [-1, 1] (if hyperbolic).
        /// coefficient - coefficient of coverage of input values
        /// </summary>
        public Func<double, double, double> Activation { get; set; }

        /// <summary>
        /// Converts input values from [0, 1] (sigmoid) or [-1, 1] (hyperbolic), in to initial values
        /// </summary>
        public Func<double, double, double> InverseActivation { get; set; }

        /// <summary>
        /// Run a neural network for input values
        /// </summary>
        public abstract double[] Run(double[] input);

        public abstract NeuralNetworkLearnResult Learn(double[] input, double[] ideal);

        protected double SigmoidActivation(double input, double coefficient = 1) => 1 / (1 + Math.Exp(-coefficient * input));

        protected double InverseSigmoidActivation(double input, double coefficient = 1) => -Math.Log(1 / input - 1) / coefficient;

        protected double HyperbolicActivation(double input, double coefficient = 1)
        {
            var exp = Math.Exp(2 * coefficient * input);
            return (exp - 1) / (exp + 1);
        }

        protected double InverseHyperbolicActivation(double input, double coefficient = 1) => 
            Math.Log((1 + 2 * input) / (1 - 2 * input)) / (2 * coefficient);
    }
}
