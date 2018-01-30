namespace NeuralNetwork {
	public class RecurentParameters {
		public double ActivationCoefficient { get; set; } = 1;
		public double LearnSpeed { get; set; } = 0.1;

		public RecurentParameters() { }

		public RecurentParameters(double activationCoefficient, double learnSpeed) {
			ActivationCoefficient = activationCoefficient;
			LearnSpeed = learnSpeed;
		}
	}
}
