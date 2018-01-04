namespace NeuralNetwork {
	public abstract class RecurentSequenceToOne : NeuralNetwork, SequenceToOneWithTeacher {
		public abstract (Vector outputValue, Vector error) Learn(Vector[] input, Vector ideal);
		public abstract Vector Run(Vector[] input);
	}
}
