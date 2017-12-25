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
		public double[][][] Weights { get; private set; }

		public double[][][] Delts { get; private set; }

		private readonly MathHelper mathHelper = new MathHelper();

		public PerceptronNeuralNetwork(PeceptronNeuralNetworkParameters parameters,
			Activation activation,
			params int[] lengthsOfEachLayer) {
			InitializeNeurons(lengthsOfEachLayer);
			InitializeWeigthsAndDelta(lengthsOfEachLayer);
		}

		public override double[] Run(double[] input) {
			SetInputNeuronsAndClear(input);
			for (var i = 0; i < Neurons.Length - 1; i++) {
				Neurons[i + 1] = mathHelper.MullWithTransposeMatrix(Weights[i], Neurons[i], Activation);
			}
			return Neurons.Last();
		}

		public override NeuralNetworkLearnResult Learn(double[] input, double[] ideal) {
			var result = new NeuralNetworkLearnResult {
				Value = Run(input)
			};
			
			return result;
		}

		private void InitializeNeurons(int[] lengthsOfEachLayer) {
			var countNeuronLayer = lengthsOfEachLayer.Length;
			Neurons = new double[countNeuronLayer][];
			for (var i = 0; i < countNeuronLayer; i++) {
				Neurons[i] = new double[lengthsOfEachLayer[i]];
			}
		}

		private void SetInputNeuronsAndClear(double[] input) {
			if (input.Length != Neurons[0].Length) {
				throw new ArithmeticException("Lengths of input vector and length of first row in Neurons must be equals");
			}
			for (var i = 1; i < Neurons.Length; i++) {
				for (var j = 0; j < Neurons[i].Length; j++) {
					Neurons[i][j] = 0;
				}
			}
			Neurons[0] = input;
		}

		private void InitializeWeigthsAndDelta(int[] lengthsOfEachLayer) {
			var countLayer = lengthsOfEachLayer.Length - 1;
			Weights = new double[countLayer][][];
			Delts = new double[countLayer][][];
			for (var i = 0; i < countLayer; i++) {
				Weights[i] = mathHelper.CreateMatrix(lengthsOfEachLayer[i], lengthsOfEachLayer[i + 1], true);
				Delts[i] = mathHelper.CreateMatrix(lengthsOfEachLayer[i], lengthsOfEachLayer[i + 1], false);
			}
		}
	}

	public class PeceptronNeuralNetworkParameters {

	}
}
