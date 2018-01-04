namespace NeuralNetwork {
	public class LstmLayer {
		public Vector Forget { get; set; }
		public Vector Input { get; set; }
		public Vector Output { get; set; }
		public Vector OutputFromPreviousLayer { get; set; }
		public SinglelayerPerceptron ForgetGateLayer { get; set; }
		public SinglelayerPerceptron InputLayerGate { get; set; }
		public SinglelayerPerceptron TanhLayer { get; set; }
		public SinglelayerPerceptron OutputLayer { get; set; }

		public LstmLayer(RecurentParameters parameters, 
			int lengthOfInput, int lengthOfOutput) {
			InitializeData(lengthOfOutput);
			InitializeNeuralNetworkLayers(parameters, lengthOfInput, lengthOfOutput);
		}

		private void InitializeData(int lengthOfOutput) {
			Forget = new Vector(lengthOfOutput);
			OutputFromPreviousLayer = new Vector(lengthOfOutput);
		}

		private void InitializeNeuralNetworkLayers(RecurentParameters parameters,
			int lengthOfInput, int lengthOfOutput) {
			ForgetGateLayer = new SinglelayerPerceptron(
				parameters.PerceptronsParameters,
				new SigmoidActivation(parameters.ActivationCoefficient),
				lengthOfInput + lengthOfOutput,
				lengthOfOutput);
			InputLayerGate = new SinglelayerPerceptron(
				parameters.PerceptronsParameters,
				new SigmoidActivation(parameters.ActivationCoefficient),
				lengthOfInput + lengthOfOutput,
				lengthOfOutput);
			TanhLayer = new SinglelayerPerceptron(
				parameters.PerceptronsParameters,
				new HyperbolicActivation(parameters.ActivationCoefficient),
				lengthOfInput + lengthOfOutput,
				lengthOfOutput);
			OutputLayer = new SinglelayerPerceptron(
				parameters.PerceptronsParameters,
				new SigmoidActivation(parameters.ActivationCoefficient),
				lengthOfInput + lengthOfOutput,
				lengthOfOutput);
		}
	}
}
