using System;

namespace NeuralNetwork {
	public class HyperbolicActivation : Activation {
		public HyperbolicActivation(double activationCoefficient = 1) {
			Func = HyperbolicActivationFunction;
			InverseFunc = InverseHyperbolicActivationFunction;
			DeriveFunc = DeriveHyperbolicActivationFunction;
			ActivationCoefficient = activationCoefficient;
		}

		private double HyperbolicActivationFunction(double input) {
			var exp = Math.Exp(2 * ActivationCoefficient * input);
			return (exp - 1) / (exp + 1);
		}

		private double DeriveHyperbolicActivationFunction(double input) =>
			1 - input * input;

		private double InverseHyperbolicActivationFunction(double input) =>
			Math.Log((1 + 2 * input) / (1 - 2 * input)) / (2 * ActivationCoefficient);
	}
}
