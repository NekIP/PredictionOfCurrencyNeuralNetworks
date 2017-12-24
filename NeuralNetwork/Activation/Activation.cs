using System;
using System.Collections.Generic;
using System.Text;

namespace NeuralNetwork.ActivationFunctions
{
    public abstract class ActivationFunction
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
    }
}
