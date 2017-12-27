using System;

namespace NeuralNetwork {
	public class NeuralNetworkException : Exception
    {
		public NeuralNetworkException() { }
		public NeuralNetworkException(string message) : base(message) { }
		public NeuralNetworkException(string message, Exception innerException) : base(message, innerException) { }
	}
}
