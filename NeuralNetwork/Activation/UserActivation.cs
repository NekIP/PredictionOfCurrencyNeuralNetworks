using System;

namespace NeuralNetwork {
	public class UserActivation : Activation {
		public UserActivation(Func<double, double, double> activationFunction,
			Func<double, double, double> inverseActivationFunction) {
			Func = activationFunction;
			InverseFunc = inverseActivationFunction;
		}
	}
}
