using System;

namespace NeuralNetwork {
	public abstract class NeuralNetwork {
		public Activation Activation { get; protected set; }

		/// <summary>
		/// Calculates the output vector of the neural network and converts it using the inverse activation function
		/// </summary>
		public abstract double[] Run(double[] input);

		/// <summary>
		/// Neural network is train
		/// </summary>
		public abstract NeuralNetworkLearnResult Learn(double[] input, double[] ideal);

		protected void CheckConditionOnException<TException>(bool conditionFunc, string message) 
			where TException: Exception, new() {
			if (conditionFunc) {
				throw new NeuralNetworkException(message, new TException());
			}
		}

		protected void CheckConditionOnException(bool conditionFunc, string message) {
			if (!conditionFunc) {
				throw new NeuralNetworkException(message);
			}
		}
	}
}
