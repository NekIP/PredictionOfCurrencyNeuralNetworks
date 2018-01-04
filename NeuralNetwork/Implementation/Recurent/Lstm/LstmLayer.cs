namespace NeuralNetwork {
	public class LstmLayer {
		public SinglelayerPerceptron ForgetGateLayer { get; set; }
		public SinglelayerPerceptron InputLayerGate { get; set; }
		public SinglelayerPerceptron TanhLayer { get; set; }
		public SinglelayerPerceptron OutputLayer { get; set; }

		public LstmLayer(PerceptronParameters perceptronParameters, 
			int lengthOfInput, int lengthOfOutput) {
			InitializeNeuralNetworkLayers(perceptronParameters, lengthOfInput, lengthOfOutput);
		}

		private void InitializeNeuralNetworkLayers(PerceptronParameters perceptronParameters,
			int lengthOfInput, int lengthOfOutput) {
			ForgetGateLayer = new SinglelayerPerceptron(
				perceptronParameters,
				new SigmoidActivation(),
				lengthOfInput + lengthOfOutput,
				lengthOfOutput);
			InputLayerGate = new SinglelayerPerceptron(
				perceptronParameters,
				new SigmoidActivation(),
				lengthOfInput + lengthOfOutput,
				lengthOfOutput);
			TanhLayer = new SinglelayerPerceptron(
				perceptronParameters,
				new HyperbolicActivation(),
				lengthOfInput + lengthOfOutput,
				lengthOfOutput);
			OutputLayer = new SinglelayerPerceptron(
				perceptronParameters,
				new SigmoidActivation(),
				lengthOfInput + lengthOfOutput,
				lengthOfOutput);
		}
	}
}
