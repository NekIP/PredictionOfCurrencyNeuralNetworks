namespace NeuralNetwork {
	public abstract class Recurent : NeuralNetwork, SequenceToSequenceWithTeacher {
		public abstract (Vector[] outputValue, Vector[] error) Learn(Vector[] input, Vector[] ideal);
		public abstract Vector[] Run(Vector[] input);
	}
}
