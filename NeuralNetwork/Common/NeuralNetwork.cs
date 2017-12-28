using System;

namespace NeuralNetwork {
	public abstract class NeuralNetwork {
		public Activation Activation { get; protected set; }

		/// <summary>
		/// Calculates the output vector of the neural network
		/// </summary>
		public abstract double[] Run(double[] input);

		/// <summary>
		/// Neural network is train
		/// </summary>
		public abstract NeuralNetworkLearnResult Learn(double[] input, double[] ideal);

		/// <summary>
		/// Converts the output values of a neural network using the inverse activation function
		/// </summary>
		public virtual double[] ConvertOutput(double[] output) {
			var result = new double[output.Length];
			for (var i = 0; i < output.Length; i++) {
				result[i] = Activation.InverseFunc(output[i]);
			}
			return result;
		}

		protected void CheckConditionOnException<TException>(bool conditionFunc, string message) 
			where TException: Exception, new() {
			if (conditionFunc) {
				throw new NeuralNetworkException(message, new TException());
			}
		}

		protected void CheckConditionOnException(bool conditionFunc, string message) {
			if (conditionFunc) {
				throw new NeuralNetworkException(message);
			}
		}
	}
}
