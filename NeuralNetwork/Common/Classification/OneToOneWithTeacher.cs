namespace NeuralNetwork {
	public interface OneToOneWithTeacher {
		/// <summary>
		/// Calculates the output vector of the neural network
		/// </summary>
		Vector Run(Vector input);

		/// <summary>
		/// Train a neural network with teacher
		/// </summary>
		/// <returns>Return a outputValue and error</returns>
		(Vector outputValue, Vector error) Learn(Vector input, Vector ideal);
	}
}