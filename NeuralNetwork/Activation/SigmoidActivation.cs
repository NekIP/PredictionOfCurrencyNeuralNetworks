using System;

namespace NeuralNetwork {
	public class SigmoidActivation : Activation {
		public double ActivationCoefficient { get; set; }

		public SigmoidActivation(double activationCoefficient = 1) {
			ActivationCoefficient = activationCoefficient;
		}

		public double Func(double input) =>
			1 / (1 + Math.Exp(-ActivationCoefficient * input));

		public double DeriveFunc(double input) =>
			(1 - input) * input;

		public double InverseFunc(double input) =>
			-Math.Log(1 / input - 1) / ActivationCoefficient;
	}
}
