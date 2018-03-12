using DataAssistants.Structs;

namespace NeuralNetwork {
	public class NeuralNetworkResult {
		public Shape Value { get; set; }

		public NeuralNetworkResult(Vector value) {
			Value = new Shape(value);
		}

		public NeuralNetworkResult(Vector[] value) {
			Value = new Shape(value);
		}

		public NeuralNetworkResult(Vector[][] value) {
			Value = new Shape(value);
		}

		public NeuralNetworkResult(Matrix value) {
			Value = new Shape(value);
		}
	}
}
