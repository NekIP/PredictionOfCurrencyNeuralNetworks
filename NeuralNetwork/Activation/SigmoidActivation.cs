using System;

namespace NeuralNetwork {
	public class SigmoidActivation : Activation {
		public SigmoidActivation(double activationCoefficient = 1) {
			Func = SigmoidActivationFunction;
			InverseFunc = InverseSigmoidActivationFunction;
			DeriveFunc = DeriveSigmoidActivationFunction;
			ActivationCoefficient = activationCoefficient;
		}

		private double SigmoidActivationFunction(double input) =>
			1 / (1 + Math.Exp(-ActivationCoefficient * input));

		private double DeriveSigmoidActivationFunction(double input) =>
			(1 - input) * input;

		private double InverseSigmoidActivationFunction(double input) =>
			-Math.Log(1 / input - 1) / ActivationCoefficient;
	}
}
