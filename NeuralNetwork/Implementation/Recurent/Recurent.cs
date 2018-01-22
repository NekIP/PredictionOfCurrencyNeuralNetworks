namespace NeuralNetwork {
	public abstract class Recurent : NeuralNetwork, SequenceToSequenceWithTeacher {
		public abstract (Vector[] outputValues, Vector[] errors) Learn(Vector[] inputs, Vector[] ideals);
		public abstract Vector[] Run(Vector[] input);
	}
}
