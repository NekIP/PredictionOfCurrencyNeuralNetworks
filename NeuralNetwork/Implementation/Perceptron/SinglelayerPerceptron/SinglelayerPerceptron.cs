using System;

namespace NeuralNetwork {
	public class SinglelayerPerceptron : Perceptron {
		public Matrix Weights { get; set; }
		public Matrix DefferenceWeights { get; set; }
		public Vector Bias { get; set; }
		public Vector InputNeurons { get; set; }
		public Vector OutputNeurons { get; set; }

		public SinglelayerPerceptron(PerceptronParameters parameters,
			Activation activation,
			int lengthOfInput, int lengthOfOutput) {
			CheckInitializationParameters(parameters, activation, lengthOfInput, lengthOfOutput);
			Activation = activation;
			InitVectors(lengthOfInput, lengthOfOutput);
			InitMatrixes(lengthOfInput, lengthOfOutput);
		}

		public override Vector Run(Vector input) {
			CheckInputVector(input, InputNeurons);
			InputNeurons = Vector.Convert(input, Activation.Func);
			OutputNeurons = Vector.Convert(Weights * InputNeurons + Bias, Activation.Func);
			return OutputNeurons;
		}

		public override NeuralNetworkLearnResult Learn(Vector input, Vector ideal) {
			CheckIdealVector(ideal, OutputNeurons);
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
			InputNeurons = new Vector(lengthOfInput);
			OutputNeurons = new Vector(lengthOfOutput);
			Bias = new Vector(lengthOfOutput);
		}

		private void InitMatrixes(int lengthOfInput, int lengthOfOutput) {
			var rnd = new Random();
			Weights = new Matrix(lengthOfOutput, lengthOfInput, () => rnd.NextDouble());
			DefferenceWeights = new Matrix(lengthOfOutput, lengthOfInput);
		}

		private void LearnWithBackPropagationError(Vector actual, Vector ideal) {
			var deltasOnPreviouseLayer = GetDelta0(actual, ideal);
			for (var j = 0; j < OutputNeurons.Length; j++) {
				for (var k = 0; k < InputNeurons.Length; k++) {
					var gradientForCurrentWeight = GetGradient(deltasOnPreviouseLayer[j], InputNeurons[k]);
					DefferenceWeights[j][k] = GetDefferenceWeight(DefferenceWeights[j][k], gradientForCurrentWeight);
					Weights[j][k] += DefferenceWeights[j][k];
				}
			}
		}
	}
}
