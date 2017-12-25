using System;

namespace NeuralNetwork {
	public class HyperbolicActivation : Activation {
		public HyperbolicActivation(double activationCoefficient = 1) {
			Func = HyperbolicActivationFunction;
			InverseFunc = InverseHyperbolicActivationFunction;
			DeriveFunc = DeriveHyperbolicActivationFunction;
			ActivationCoefficient = activationCoefficient;
		}

		private double HyperbolicActivationFunction(double input, double coefficient = 1) {
			var exp = Math.Exp(2 * coefficient * input);
			return (exp - 1) / (exp + 1);
		}

		private double DeriveHyperbolicActivationFunction(double input) =>
			1 - input * input;

		private double InverseHyperbolicActivationFunction(double input, double coefficient = 1) =>
			Math.Log((1 + 2 * input) / (1 - 2 * input)) / (2 * coefficient);
	}
}
