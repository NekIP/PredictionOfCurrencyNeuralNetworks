using NeuralNetwork.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NeuralNetwork {
	public class PerceptronNeuralNetwork : NeuralNetwork {
		public double[][] Neurons { get; private set; }

		/// <summary>
		/// Weigths for i(layer), for link from j(neuron in i layer) to k(neuronin (i + 1) layer)
		/// </summary>
		public double[][][] Weights { get; set; }

		/// <summary>
		/// Weigths for i(layer), for link from j(neuron in i layer) to k(neuronin (i + 1) layer)
		/// </summary>
		public double[][][] DeltaWeights { get; set; }

		public int Epoch { get; private set; }
		public double LearningSpeed { get; set; }
		public double Moment { get; set; }

		private readonly MathHelper mathHelper = new MathHelper();

		public PerceptronNeuralNetwork(PeceptronNeuralNetworkParameters parameters,
			Activation activation,
			params int[] lengthsOfEachLayer) {
			InitializeNeurons(lengthsOfEachLayer);
			InitializeWeigthsAndDelta(lengthsOfEachLayer);
			LearningSpeed = parameters.LearningSpeed;
			Activation = activation;
			Moment = parameters.Moment;
		}

		public override double[] Run(double[] input) {
			SetInputNeuronsAndClear(input);
			for (var i = 0; i < Neurons.Length - 1; i++) {
				Neurons[i + 1] = mathHelper.MullMatrixOnVector(Weights[i], Neurons[i], Activation);
			}
			var result = Neurons.Last();
			return result;
		}

		public override NeuralNetworkLearnResult Learn(double[] input, double[] ideal) {
			var actual = Run(input);
			var idealConverted = new double[ideal.Length];
			for (var i = 0; i < idealConverted.Length; i++) {
				idealConverted[i] = Activation.Func(ideal[i]);
			}
			var result = new NeuralNetworkLearnResult {
				Value = actual,
				Error = GetError(actual, idealConverted)
			};
			var delts = GetDelta0(actual, idealConverted);
			for (var i = Neurons.Length - 2; i >= 0; i--) {
				var newDelts = new double[Neurons[i].Length];
				for (var j = 0; j < Neurons[i + 1].Length; j++) {
					for (var k = 0; k < Neurons[i].Length; k++) {
						newDelts[k] += Weights[i][j][k] * delts[j] * Activation.DeriveFunc(Neurons[i][k]);
						DeltaWeights[i][j][k] = GetDeffWeight(DeltaWeights[i][j][k], delts[j] * Neurons[i][k]);
						Weights[i][j][k] += DeltaWeights[i][j][k];
					}
				}
				delts = newDelts;
			}
			Epoch++;
			return result;
		}

		public double[] GetDelta0(double[] actual, double[] ideal) {
			var result = new double[actual.Length];
			for (var i = 0; i < actual.Length; i++) {
				result[i] = (ideal[i] - actual[i]) * Activation.DeriveFunc(actual[i]);
			}
			return result;
		}

		public double[] GetError(double[] actual, double[] ideal) {
			var result = new double[actual.Length];
			for (var i = 0; i < actual.Length; i++) {
				result[i] = Math.Pow(ideal[i] - actual[i], 2);
			}
			return result;
		}

		public double[] GetDeltaHForLayer(double[] neurons, double[][] weigth, double[] previousDelta) {
			var result = mathHelper.MullMatrixOnVector(weigth, previousDelta);
			for (var i = 0; i < result.Length; i++) {
				result[i] = result[i] * Activation.DeriveFunc(neurons[i]);
			}
			return result;
		}

		private double GetDeffWeight(double previousDeltaWeight, double gradient) =>
			LearningSpeed * gradient + Moment * previousDeltaWeight;

		private void SetInputNeuronsAndClear(double[] input) {
			if (input.Length != Neurons[0].Length) {
				throw new ArithmeticException("Lengths of input vector and length of first row in Neurons must be equals");
			}
			SetNeuronsInZeroAndSetInput(input);
		}

		private void InitializeNeurons(int[] lengthsOfEachLayer) {
			var countNeuronLayer = lengthsOfEachLayer.Length;
			Neurons = new double[countNeuronLayer][];
			for (var i = 0; i < countNeuronLayer; i++) {
				Neurons[i] = new double[lengthsOfEachLayer[i]];
			}
		}

		private void SetNeuronsInZeroAndSetInput(double[] input) {
			for (var i = 0; i < Neurons.Length; i++) {
				for (var j = 0; j < Neurons[i].Length; j++) {
					Neurons[i][j] = i == 0 ? input[j] : 0;
				}
			}
		}

		private void InitializeWeigthsAndDelta(int[] lengthsOfEachLayer) {
			var countLayer = lengthsOfEachLayer.Length - 1;
			Weights = new double[countLayer][][];
			DeltaWeights = new double[countLayer][][];
			for (var i = 0; i < countLayer; i++) {
				Weights[i] = mathHelper.CreateMatrix(lengthsOfEachLayer[i + 1], lengthsOfEachLayer[i], true);
				DeltaWeights[i] = mathHelper.CreateMatrix(lengthsOfEachLayer[i + 1], lengthsOfEachLayer[i], false);
			}
		}
	}

	public class PeceptronNeuralNetworkParameters {
		public double LearningSpeed { get; set; }
		public double Moment { get; set; }
	}
}
