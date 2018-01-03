namespace NeuralNetwork {
	public class Lstm : NeuralNetwork {
		public SinglelayerPerceptron ForgetGateLayer { get; set; }
		public SinglelayerPerceptron InputLayerGate { get; set; }
		public SinglelayerPerceptron TanhLayer { get; set; }
		public SinglelayerPerceptron OutputLayer { get; set; }

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
