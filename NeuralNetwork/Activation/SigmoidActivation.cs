using System;

namespace NeuralNetwork {
	public class SigmoidActivation : Activation {
		public double ActivationCoefficient { get; set; }

		public SigmoidActivation(double activationCoefficient = 1) {
			ActivationCoefficient = activationCoefficient;
		}

		public override double Func(double input) =>
			1 / (1 + Math.Exp(-ActivationCoefficient * input));

		public override double DeriveFunc(double input) =>
			(1 - input) * input;

		public override double InverseFunc(double input) =>
			-Math.Log(1 / input - 1) / ActivationCoefficient;
	}
}
