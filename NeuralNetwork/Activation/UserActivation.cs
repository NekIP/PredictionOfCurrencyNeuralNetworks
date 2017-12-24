using System;
using System.Collections.Generic;
using System.Text;

namespace NeuralNetwork
{
    public class UserActivation : Activation
    {
        public UserActivation(Func<double, double, double> activationFunction,
            Func<double, double, double> inverseActivationFunction)
        {
            ActivationFunction = activationFunction;
            InverseActivationFunction = inverseActivationFunction;
        }
    }
}
