using System;

namespace NeuralNetwork {
	public class UserActivation : Activation {
		public UserActivation(Func<double, double, double> activationFunction,
			Func<double, double, double> inverseActivationFunction,
			Func<double, double> deriveActivationFunction,
			double activationCoefficient = 1) {
			Func = activationFunction;
			InverseFunc = inverseActivationFunction;
			DeriveFunc = deriveActivationFunction;
			ActivationCoefficient = activationCoefficient;
		}
	}
}
