using System;

namespace NeuralNetwork {
	public class HyperbolicActivation : Activation {
		public double ActivationCoefficient { get; set; }

		public HyperbolicActivation(double activationCoefficient = 1) {
			ActivationCoefficient = activationCoefficient;
		}

		public double Func(double input) {
			var exp = Math.Exp(2 * ActivationCoefficient * input);
			return (exp - 1) / (exp + 1);
		}

		public double DeriveFunc(double input) =>
			1 - input * input;

		public double InverseFunc(double input) =>
			Math.Log((1 + 2 * input) / (1 - 2 * input)) / (2 * ActivationCoefficient);
	}
}
