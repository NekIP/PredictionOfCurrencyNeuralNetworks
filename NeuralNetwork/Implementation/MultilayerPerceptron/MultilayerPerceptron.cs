using System;
using System.Linq;

namespace NeuralNetwork {
	public class MultilayerPerceptron : NeuralNetwork {
		public Vector[] Neurons { get; private set; }

		/// <summary>
		/// Weigths for i(layer), for link from j(neuron in (i + 1) layer) to k(neuronin i layer)
		/// </summary>
		public Matrix[] Weights { get; set; }

		/// <summary>
		/// Defference weigths for i(layer), for link from j(neuron in (i + 1) layer) to k(neuronin i layer)
		/// </summary>
		public Matrix[] DefferenceWeights { get; set; }

		/// <summary>
		/// Number of iterations of neural network training
		/// </summary>
		public int Epoch { get; private set; }

		public MultilayerPerceptronParameters Parameters { get; set; }

		public MultilayerPerceptron(MultilayerPerceptronParameters parameters,
			Activation activation,
			params int[] lengthsOfEachLayer) {
			CheckConditionOnException(parameters is null, "Neural network parameters is null");
			CheckConditionOnException(activation is null, "Activation function is null");
			CheckConditionOnException(lengthsOfEachLayer.Length < 2, "The number of elements of lengths of vectors of neurons should be more than 1");
			InitializeNeurons(lengthsOfEachLayer);
			InitializeWeigthsAndDefference(lengthsOfEachLayer);
			Parameters = parameters;
			Activation = activation;
		}

		/// <summary>
		/// Calculates the output vector of the neural network
		/// </summary>
		public override Vector Run(Vector input) {
			InitializeNeuronsWithInput(input);
			for (var i = 0; i < Neurons.Length - 1; i++) {
				for (int j = 0; j < Neurons[i + 1].Length; j++) {
					for (int k = 0; k < Neurons[i].Length; k++) {
						Neurons[i + 1][j] += Weights[i][j][k] * Neurons[i][k];
					}
					Neurons[i + 1][j] = Activation.Func(Neurons[i + 1][j]);
				}
			}
			return Neurons.Last();
		}

		/// <summary>
		/// Perceptron is trained with the help of a teacher using the method of back propagation of an error using deltas
		/// </summary>
		/// <param name="ideal">The correct output value</param>
		public override NeuralNetworkLearnResult Learn(Vector input, Vector ideal) {
			CheckConditionOnException(Neurons.Last().Length != ideal.Length,
				"The length of the verification vector and the number of neurons in the output layer must be equals");
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

		private void InitializeNeurons(int[] lengthsOfEachLayerNeurons) {
			var countLayersOfNeurons = lengthsOfEachLayerNeurons.Length;
			Neurons = new Vector[countLayersOfNeurons];
			for (var i = 0; i < countLayersOfNeurons; i++) {
				Neurons[i] = new Vector(lengthsOfEachLayerNeurons[i]);
			}
		}

		private void InitializeWeigthsAndDefference(int[] lengthsOfEachLayer) {
			var random = new Random();
			var layerCountOfWeights = lengthsOfEachLayer.Length - 1;
			Weights = new Matrix[layerCountOfWeights];
			DefferenceWeights = new Matrix[layerCountOfWeights];
			for (var i = 0; i < layerCountOfWeights; i++) {
				var neuronsCountInNextLayer = lengthsOfEachLayer[i + 1];
				var neuronsCountInCurrentLayer = lengthsOfEachLayer[i];
				Weights[i] = new Matrix(neuronsCountInNextLayer, neuronsCountInCurrentLayer, 
					() => random.NextDouble());
				DefferenceWeights[i] = new Matrix(neuronsCountInNextLayer, neuronsCountInCurrentLayer);
			}
		}

		private void InitializeNeuronsWithInput(Vector input) {
			CheckConditionOnException(input.Length != Neurons.First().Length,
				"The length of the input vector and the number of neurons in the first layer must be equal");
			for (var i = 0; i < Neurons.Length; i++) {
				for (var j = 0; j < Neurons[i].Length; j++) {
					Neurons[i][j] = i == 0 ? Activation.Func(input[j]) : 0;
				}
			}
		}

		private void LearnWithBackPropagationError(Vector actual, Vector ideal) {
			var deltasOnPreviouseLayer = GetDelta0(actual, ideal);
			for (var i = Neurons.Length - 2; i >= 0; i--) {
				var deltasOnCurrentLayer = new Vector(Neurons[i].Length);
				for (var j = 0; j < Neurons[i + 1].Length; j++) {
					for (var k = 0; k < Neurons[i].Length; k++) {
						var gradientForCurrentWeight = GetGradient(deltasOnPreviouseLayer[j], Neurons[i][k]);
						deltasOnCurrentLayer[k] += GetChunkOfDeltaH(Weights[i][j][k], Neurons[i][k], deltasOnPreviouseLayer[j]);
						DefferenceWeights[i][j][k] = GetDefferenceWeight(DefferenceWeights[i][j][k], gradientForCurrentWeight);
						Weights[i][j][k] += DefferenceWeights[i][j][k];
					}
				}
				deltasOnPreviouseLayer = deltasOnCurrentLayer;
			}
		}

		private Vector GetDelta0(Vector actual, Vector ideal) =>
			Vector.Combine(actual, ideal, (actualItem, idealItem) => (idealItem - actualItem) * Activation.DeriveFunc(actualItem));

		private double GetChunkOfDeltaH(double weightSynapse, double neuronInBeginSynapse, double deltaInEndSynapse) =>
			weightSynapse * deltaInEndSynapse * Activation.DeriveFunc(neuronInBeginSynapse);

		private double GetError(double actual, double ideal) => Math.Pow(ideal - actual, 2);

		private double GetDefferenceWeight(double previousDeltaWeight, double gradient) =>
			Parameters.LearningSpeed * gradient + Parameters.Moment * previousDeltaWeight;

		private double GetGradient(double neuronInBeginSynapse, double deltaInEndSynapse) =>
			neuronInBeginSynapse * deltaInEndSynapse;

		private void SetNextEpoch() => Epoch++;
	}
}
