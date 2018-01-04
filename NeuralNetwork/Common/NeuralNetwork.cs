using System;

namespace NeuralNetwork {
	public abstract class NeuralNetwork {
		/// <summary>
		/// Number of iterations of neural network training
		/// </summary>
		public int Epoch { get; private set; }

		public Activation Activation { get; protected set; }

		/// <summary>
		/// Calculates the output vector of the neural network
		/// </summary>
		//public abstract Vector Run(Vector input);

		/// <summary>
		/// Calculates the output vector of the neural network oriented on a sequence
		/// </summary>
		//public abstract Vector Run(Vector[] input);

		/// <summary>
		/// Train a neural network
		/// </summary>
		//public abstract NeuralNetworkLearnResult Learn(Vector input, Vector ideal);

		/// <summary>
		/// Neural network is train oriented on a sequence
		/// </summary>
		//public abstract NeuralNetworkLearnResult Learn(Vector[] input, Vector ideal);

		/// <summary>
		/// Converts the output values of a neural network using the inverse activation function
		/// </summary>
		public virtual Vector ConvertOutput(Vector output) => Vector.Convert(output, Activation.InverseFunc);

		protected void SetNextEpoch() => Epoch++;

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
