using DataAssistants;
using DataAssistants.Structs;
using System;
using System.Reflection;

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

		public override NeuralNetworkResult Run(NeuralNetworkData inputData) {
			var input = ConvertDataToVector(inputData);
			var output = Run(input);
			return new NeuralNetworkResult(output);
		}

		public override NeuralNetworkLearnResult Learn(NeuralNetworkData inputData, NeuralNetworkData idealData) {
			var input = ConvertDataToVector(inputData);
			var ideal = ConvertDataToVector(idealData);
			var (output, error) = Learn(input, ideal);
			return new NeuralNetworkLearnResult(output, error);
		}

		public override NeuralNetworkLearnResult Learn(NeuralNetworkData inputData) {
			throw new NotImplementedException();
		}

		public Vector Run(Vector input) {
			CheckInput(input, InputNeurons);
			InputNeurons = input;
			OutputNeurons = Activation.Func(Weights * InputNeurons + Bias);
			return OutputNeurons;
		}

		public (Vector output, Vector error) Learn(Vector input, Vector ideal) {
			CheckIdealVector(ideal, OutputNeurons);
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

        public override void Load(string nameOfNeuralNetwork) {
            var serializer = new Serializer();
            Weights = serializer.Deserialize<Matrix>(DefaultPath + nameOfNeuralNetwork + "/" + nameOfNeuralNetwork + "_Weights.json");
            DefferenceWeights = serializer.Deserialize<Matrix>(DefaultPath + nameOfNeuralNetwork + "/" + nameOfNeuralNetwork + "_DefferenceWeights.json");
            Bias = serializer.Deserialize<Vector>(DefaultPath + nameOfNeuralNetwork + "/" + nameOfNeuralNetwork + "_Bias.json");
            InputNeurons = serializer.Deserialize<Vector>(DefaultPath + nameOfNeuralNetwork + "/" + nameOfNeuralNetwork + "_InputNeurons.json");
            OutputNeurons = serializer.Deserialize<Vector>(DefaultPath + nameOfNeuralNetwork + "/" + nameOfNeuralNetwork + "_OutputNeurons.json");
            Parameters = serializer.Deserialize<PerceptronParameters>(DefaultPath + nameOfNeuralNetwork + "/" + nameOfNeuralNetwork + "_Parameters.json");
            var activationStr = serializer.Deserialize<string>(DefaultPath + nameOfNeuralNetwork + "/" + nameOfNeuralNetwork + "_Activation.json");
            var (typename, activationCoef) = (activationStr.Split(";")[0], double.Parse(activationStr.Split(";")[1]));
            var type = Assembly.GetExecutingAssembly().GetType(typename);
            Activation = Activator.CreateInstance(type, activationCoef) as Activation;
        }

        public override void Save(string nameOfNeuralNetwork) {
            var serializer = new Serializer();
            serializer.Serialize(Weights, DefaultPath + nameOfNeuralNetwork + "/" + nameOfNeuralNetwork + "_Weights.json");
            serializer.Serialize(DefferenceWeights, DefaultPath + nameOfNeuralNetwork + "/" + nameOfNeuralNetwork + "_DefferenceWeights.json");
            serializer.Serialize(Bias, DefaultPath + nameOfNeuralNetwork + "/" + nameOfNeuralNetwork + "_Bias.json");
            serializer.Serialize(InputNeurons, DefaultPath + nameOfNeuralNetwork + "/" + nameOfNeuralNetwork + "_InputNeurons.json");
            serializer.Serialize(OutputNeurons, DefaultPath + nameOfNeuralNetwork + "/" + nameOfNeuralNetwork + "_OutputNeurons.json");
            serializer.Serialize(Parameters, DefaultPath + nameOfNeuralNetwork + "/" + nameOfNeuralNetwork + "_Parameters.json");
            serializer.Serialize(Activation.GetType().Name + ";" + Activation.ActivationCoefficient, DefaultPath + nameOfNeuralNetwork + "/" + nameOfNeuralNetwork + "_Activation.json");
        }
    }
}
