using System;

namespace NeuralNetwork {
	public class SigmoidActivation : Activation {
		public SigmoidActivation() {
			Func = SigmoidActivationFunction;
			InverseFunc = InverseSigmoidActivationFunction;
		}

		private double SigmoidActivationFunction(double input, double coefficient = 1) =>
			1 / (1 + Math.Exp(-coefficient * input));

		private double DeriveSigmoidActivationFunction(double input) =>
			(1 - input) * input;

		private double InverseSigmoidActivationFunction(double input, double coefficient = 1) =>
			-Math.Log(1 / input - 1) / coefficient;
	}
}
