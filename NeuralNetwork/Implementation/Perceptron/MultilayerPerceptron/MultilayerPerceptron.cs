using DataAssistants;
using DataAssistants.Structs;
using System;
using System.IO;
using System.Linq;
using System.Reflection;

namespace NeuralNetwork {
    public class MultilayerPerceptron : Perceptron {
		public Vector[] Neurons { get; private set; }

		/// <summary>
		/// Weigths for i(layer), for link from j(neuron in (i + 1) layer) to k(neuron in i layer)
		/// </summary>
		public Matrix[] Weights { get; set; }

		/// <summary>
		/// Defference weigths for i(layer), for link from j(neuron in (i + 1) layer) to k(neuron in i layer)
		/// </summary>
		public Matrix[] DefferenceWeights { get; set; }

		public MultilayerPerceptron(PerceptronParameters parameters,
			Activation activation,
			params int[] lengthsOfEachLayer) {
			CheckInitializationParameters(parameters, activation, lengthsOfEachLayer);
			InitializeNeurons(lengthsOfEachLayer);
			InitializeWeigthsAndDefference(lengthsOfEachLayer);
			Parameters = parameters;
			Activation = activation;
		}

		/// <summary>
		/// Calculates the output vector of the neural network
		/// </summary>
		public override NeuralNetworkResult Run(NeuralNetworkData inputData) {
			var input = ConvertDataToVector(inputData);
			var output = Run(input);
			return new NeuralNetworkResult(output);
		}

		/// <summary>
		/// Perceptron is trained with the help of a teacher using the method of back propagation of an error using deltas
		/// </summary>
		/// <param name="ideal">The correct output value</param>
		public override NeuralNetworkLearnResult Learn(NeuralNetworkData inputData, NeuralNetworkData idealData) {
			var input = ConvertDataToVector(inputData);
			var ideal = ConvertDataToVector(idealData);
			var (output, error) = Learn(input, ideal);
			return new NeuralNetworkLearnResult(output, error);
		}

		public override NeuralNetworkLearnResult Learn(NeuralNetworkData inputData) {
			throw new NotImplementedException();
		}

		/// <summary>
		/// Calculates the output vector of the neural network
		/// </summary>
		public Vector Run(Vector input) {
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
		public (Vector output, Vector error) Learn(Vector input, Vector ideal) {
			CheckIdealVector(ideal, Neurons.Last());
			var actual = Run(input);
			var error = new Vector(actual.Length);
			for (var i = 0; i < ideal.Length; i++) {
				ideal[i] = Activation.Func(ideal[i]);
				error[i] = GetError(actual[i], ideal[i]);
			}
			LearnWithBackPropagationError(actual, ideal);
			SetNextEpoch();
			return (actual, error);
		}


        public override void Load(string nameOfNeuralNetwork) {
            var serializer = new Serializer();
            if (serializer.Exists(DefaultPath + nameOfNeuralNetwork + "/" + nameOfNeuralNetwork + "_Weights.json")) {
                Weights = serializer.Deserialize<Matrix[]>(DefaultPath + nameOfNeuralNetwork + "/" + nameOfNeuralNetwork + "_Weights.json");
                DefferenceWeights = serializer.Deserialize<Matrix[]>(DefaultPath + nameOfNeuralNetwork + "/" + nameOfNeuralNetwork + "_DefferenceWeights.json");
                Neurons = serializer.Deserialize<Vector[]>(DefaultPath + nameOfNeuralNetwork + "/" + nameOfNeuralNetwork + "_Neurons.json");
                Parameters = serializer.Deserialize<PerceptronParameters>(DefaultPath + nameOfNeuralNetwork + "/" + nameOfNeuralNetwork + "_Parameters.json");
                var activationStr = serializer.Deserialize<string>(DefaultPath + nameOfNeuralNetwork + "/" + nameOfNeuralNetwork + "_Activation.json");
                var (typename, activationCoef) = (activationStr.Split(";")[0], double.Parse(activationStr.Split(";")[1]));
                var type = Assembly.GetExecutingAssembly().GetType(typename);
                Activation = Activator.CreateInstance(type, activationCoef) as Activation;
            }
        }

        public override void Save(string nameOfNeuralNetwork) {
            var serializer = new Serializer();
            serializer.Serialize(Weights, DefaultPath + nameOfNeuralNetwork + "/" + nameOfNeuralNetwork + "_Weights.json");
            serializer.Serialize(Neurons, DefaultPath + nameOfNeuralNetwork + "/" + nameOfNeuralNetwork + "_Neurons.json");
            serializer.Serialize(DefferenceWeights, DefaultPath + nameOfNeuralNetwork + "/" + nameOfNeuralNetwork + "_DefferenceWeights.json");
            serializer.Serialize(Parameters, DefaultPath + nameOfNeuralNetwork + "/" + nameOfNeuralNetwork + "_Parameters.json");
            serializer.Serialize(Activation.GetType().ToString() + ";" + Activation.ActivationCoefficient, DefaultPath + nameOfNeuralNetwork + "/" + nameOfNeuralNetwork + "_Activation.json");
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
			CheckInput(input, Neurons.First());
			for (var i = 0; i < Neurons.Length; i++) {
				for (var j = 0; j < Neurons[i].Length; j++) {
					Neurons[i][j] = i == 0 ? input[j] : 0;
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
	}
}
