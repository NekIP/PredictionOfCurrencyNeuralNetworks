using System.Linq;

namespace NeuralNetwork {
	public class NeuralNetworkData {
		public Shape Shape { get; set; }

		public NeuralNetworkDataType DataType => Shape.Values.Length > 1
			? NeuralNetworkDataType.SequenceOfSequencesOfVectors
			: Shape.Values.First().Length > 1
				? NeuralNetworkDataType.SequenceOfVectors
				: NeuralNetworkDataType.Vector;

		public NeuralNetworkData(Vector value) {
			Shape = new Shape(value);
		}

		public NeuralNetworkData(Vector[] value) {
			Shape = new Shape(value);
		}

		public NeuralNetworkData(Vector[][] value) {
			Shape = new Shape(value);
		}

		public NeuralNetworkData(Matrix value) {
			Shape = new Shape(value);
		}
	}
}
