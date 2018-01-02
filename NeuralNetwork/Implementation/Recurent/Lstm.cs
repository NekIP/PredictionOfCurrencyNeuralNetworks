namespace NeuralNetwork {
	public class Lstm : NeuralNetwork {
		public LstmLayer ForgetGateLayer { get; set; }
		public LstmLayer InputLayerGate { get; set; }
		public LstmLayer TanhLayer { get; set; }
		public LstmLayer OutputLayer { get; set; }

		public Lstm(int lengthOfInputLayer, int LengthOutputLayer) {

		}

		public override Vector Run(Vector input) {
			throw new System.NotImplementedException();
		}

		public override NeuralNetworkLearnResult Learn(Vector input, Vector ideal) {
			throw new System.NotImplementedException();
		}
	}
}
