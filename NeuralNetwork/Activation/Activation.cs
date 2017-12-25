using System;

namespace NeuralNetwork
{
    public abstract class Activation
    {
        /// <summary>
        /// Activation function convert input value in to range [0, 1] (if sigmoid) or [-1, 1] (if hyperbolic).
        /// coefficient - coefficient of coverage of input values
        /// </summary>
        public Func<double, double, double> Convert { get; set; }

        /// <summary>
        /// Converts input values from [0, 1] (sigmoid) or [-1, 1] (hyperbolic), in to initial values
        /// </summary>
        public Func<double, double, double> InverseConvert { get; set; }
    }
}
