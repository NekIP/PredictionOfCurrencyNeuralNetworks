namespace NeuralNetwork {
	public interface OneToOneWithoutTeacher {
		/// <summary>
		/// Calculates the output vector of the neural network
		/// </summary>
		Vector Run(Vector input);

		/// <summary>
		/// Train a neural network without teacher
		/// </summary>
		/// <returns>Return a outputValue and error</returns>
		(Vector outputValue, Vector error) Learn(Vector input);
	}
}
