﻿using System;

namespace NeuralNetwork {
	public class HyperbolicActivation : Activation {
		public HyperbolicActivation(double activationCoefficient = 1) {
			ActivationCoefficient = activationCoefficient;
		}

		public override double Func(double input) {
			var exp = Math.Exp(2 * ActivationCoefficient * input);
			return (exp - 1) / (exp + 1);
		}

		public override double DeriveFunc(double input) =>
			1 - input * input;

		public override double InverseFunc(double input) =>
			Math.Log((1 + 2 * input) / (1 - 2 * input)) / (2 * ActivationCoefficient);
	}
}
