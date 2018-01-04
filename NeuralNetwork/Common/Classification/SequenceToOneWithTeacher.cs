namespace NeuralNetwork {
	public interface SequenceToOneWithTeacher {
		/// <summary>
		/// Calculates the output vector of the neural network
		/// </summary>
		Vector Run(Vector[] input);

		/// <summary>
		/// Train a neural network with teacher
		/// </summary>
		/// <param name="input">Sequence of vectors</param>
		/// <returns>Return a outputValue and error</returns>
		(Vector outputValue, Vector error) Learn(Vector[] input, Vector ideal);
	}
}
