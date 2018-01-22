namespace NeuralNetwork {
	public interface SequenceToSequenceWithTeacher {
		/// <summary>
		/// Calculates the output secuence of vector of the neural network
		/// </summary>
		Vector[] Run(Vector[] inputs);

		/// <summary>
		/// Train a neural network with teacher
		/// </summary>
		/// <param name="inputs">Sequence of vectors</param>
		/// <returns>Return a outputValue and error</returns>
		(Vector[] outputValues, Vector[] errors) Learn(Vector[] inputs, Vector[] ideals);
	}
}
