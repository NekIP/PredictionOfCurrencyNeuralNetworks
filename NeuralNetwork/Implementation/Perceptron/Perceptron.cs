using System;

namespace NeuralNetwork {
	public abstract class Perceptron : NeuralNetwork{
		public PerceptronParameters Parameters { get; set; }

		protected void CheckInitializationParameters(PerceptronParameters parameters, 
			Activation activation,
			params int[] lengthsOfEachLayer) {
			CheckConditionOnException(parameters is null, "Neural network parameters is null");
			CheckConditionOnException(activation is null, "Activation function is null");
			CheckConditionOnException(lengthsOfEachLayer.Length < 2, "The number of elements of lengths of vectors of neurons should be more than 1");
			for (var i = 0; i < lengthsOfEachLayer.Length; i++) {
				CheckConditionOnException(lengthsOfEachLayer[i] < 1, $"The number of neurons in layer {i} must be greater than 0");
			}
		}

		protected void CheckInputVector(Vector input, Vector inputNeurons) {
			CheckConditionOnException(input.Length != inputNeurons.Length,
				"The length of the input vector and the number of neurons in the first layer must be equal");
		}

		protected void CheckIdealVector(Vector ideal, Vector outputNeurons) {
			CheckConditionOnException(ideal.Length != outputNeurons.Length,
				"The length of the verification vector and the number of neurons in the output layer must be equals");
		}

		protected Vector GetDelta0(Vector actual, Vector ideal) =>
			Vector.Combine(actual, ideal, (actualItem, idealItem) => (idealItem - actualItem) * Activation.DeriveFunc(actualItem));

		protected double GetChunkOfDeltaH(double weightSynapse, double neuronInBeginSynapse, double deltaInEndSynapse) =>
			weightSynapse * deltaInEndSynapse * Activation.DeriveFunc(neuronInBeginSynapse);

		protected double GetError(double actual, double ideal) => Math.Pow(ideal - actual, 2);

		protected double GetDefferenceWeight(double previousDeltaWeight, double gradient) =>
			Parameters.LearningSpeed * gradient + Parameters.Moment * previousDeltaWeight;

		protected double GetGradient(double neuronInBeginSynapse, double deltaInEndSynapse) =>
			neuronInBeginSynapse * deltaInEndSynapse;
	}
}