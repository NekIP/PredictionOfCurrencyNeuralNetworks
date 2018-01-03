using System;

namespace NeuralNetwork {
	public class SinglelayerPerceptron : Perceptron {
		public Matrix Weights { get; set; }
		public Matrix DefferenceWeights { get; set; }
		public Vector Bias { get; set; }
		public Vector Input { get; set; }
		public Vector Output { get; set; }

		public SinglelayerPerceptron(PerceptronParameters parameters,
			Activation activation,
			int lengthOfInput, int lengthOfOutput) {
			CheckInitializationParameters(parameters, activation, lengthOfInput, lengthOfOutput);
			Activation = activation;
			InitVectors(lengthOfInput, lengthOfOutput);
			InitMatrixes(lengthOfInput, lengthOfOutput);
		}

		public override Vector Run(Vector input) {
			CheckInputVector(input, Input);
			Input = Vector.Convert(input, Activation.Func);
			Output = Vector.Convert(Weights * Input + Bias, Activation.Func);
			return Output;
		}

		public override NeuralNetworkLearnResult Learn(Vector input, Vector ideal) {
			CheckIdealVector(ideal, Output);
			var actual = Run(input);
			var error = new Vector(actual.Length);
			for (var i = 0; i < ideal.Length; i++) {
				ideal[i] = Activation.Func(ideal[i]);
				error[i] = GetError(actual[i], ideal[i]);
			}
			LearnWithBackPropagationError(actual, ideal);
			SetNextEpoch();
			return new NeuralNetworkLearnResult {
				Value = actual,
				Error = error
			};
		}

		private void InitVectors(int lengthOfInput, int lengthOfOutput) {
			Input = new Vector(lengthOfInput);
			Output = new Vector(lengthOfOutput);
			Bias = new Vector(lengthOfOutput);
		}

		private void InitMatrixes(int lengthOfInput, int lengthOfOutput) {
			var rnd = new Random();
			Weights = new Matrix(lengthOfOutput, lengthOfInput, () => rnd.NextDouble());
			DefferenceWeights = new Matrix(lengthOfOutput, lengthOfInput);
		}

		private void LearnWithBackPropagationError(Vector actual, Vector ideal) {
			var deltasOnPreviouseLayer = GetDelta0(actual, ideal);
			for (var j = 0; j < Output.Length; j++) {
				for (var k = 0; k < Input.Length; k++) {
					var gradientForCurrentWeight = GetGradient(deltasOnPreviouseLayer[j], Input[k]);
					DefferenceWeights[j][k] = GetDefferenceWeight(DefferenceWeights[j][k], gradientForCurrentWeight);
					Weights[j][k] += DefferenceWeights[j][k];
				}
			}
		}
	}
}
