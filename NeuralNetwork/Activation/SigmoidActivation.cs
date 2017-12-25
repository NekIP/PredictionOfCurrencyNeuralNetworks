using System;

namespace NeuralNetwork {
	public class SigmoidActivation : Activation {
		public SigmoidActivation(double activationCoefficient = 1) {
			Func = SigmoidActivationFunction;
			InverseFunc = InverseSigmoidActivationFunction;
			DeriveFunc = DeriveSigmoidActivationFunction;
			ActivationCoefficient = activationCoefficient;
		}

		private double SigmoidActivationFunction(double input, double coefficient = 1) =>
			1 / (1 + Math.Exp(-coefficient * input));

		private double DeriveSigmoidActivationFunction(double input) =>
			(1 - input) * input;

		private double InverseSigmoidActivationFunction(double input, double coefficient = 1) =>
			-Math.Log(1 / input - 1) / coefficient;
	}
}
