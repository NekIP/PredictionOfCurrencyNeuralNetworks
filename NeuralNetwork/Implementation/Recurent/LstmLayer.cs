namespace NeuralNetwork {
	public class LstmLayer {
		public Matrix Weights { get; set; }
		public Matrix DeffWeights { get; set; }
		public Vector Bias { get; set; }
		public Vector Input { get; set; }
		public Vector Output { get; set; }
	}
}
