namespace NeuralNetwork {
	public class NeuralNetworkLearnResult {
		public Shape Value { get; set; }
		public Shape Error { get; set; }

		public NeuralNetworkLearnResult(Vector value, Vector error) {
			Value = new Shape(value);
			Error = new Shape(error);
		}

		public NeuralNetworkLearnResult(Vector[] value, Vector[] error) {
			Value = new Shape(value);
			Error = new Shape(error);
		}

		public NeuralNetworkLearnResult(Vector[][] value, Vector[][] error) {
			Value = new Shape(value);
			Error = new Shape(error);
		}

		public NeuralNetworkLearnResult(Matrix value, Matrix error) {
			Value = new Shape(value);
			Error = new Shape(error);
		}
	}
}
