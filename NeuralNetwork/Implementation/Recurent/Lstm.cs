namespace NeuralNetwork {
	public class Lstm {
		public LstmLayer ForgetGateLayer { get; set; }
		public LstmLayer InputLayerGate { get; set; }
		public LstmLayer TanhLayer { get; set; }
		public LstmLayer OutputLayer { get; set; }

		public Lstm(int lengthOfInputLayer, int LengthOutputLayer) {

		}
	}
}
