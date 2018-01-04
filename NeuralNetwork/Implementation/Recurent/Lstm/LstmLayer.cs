namespace NeuralNetwork {
	public class LstmLayer {
		public SinglelayerPerceptron ForgetGateLayer { get; set; }
		public SinglelayerPerceptron InputLayerGate { get; set; }
		public SinglelayerPerceptron TanhLayer { get; set; }
		public SinglelayerPerceptron OutputLayer { get; set; }
	}
}
