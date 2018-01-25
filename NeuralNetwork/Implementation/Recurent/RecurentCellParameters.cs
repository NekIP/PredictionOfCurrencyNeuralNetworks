namespace NeuralNetwork {
	public class RecurentCellParameters {
		public int LengthOfInput { get; set; }
		public int LengthOfOutput { get; set; }

		public RecurentCellParameters(int lengthOfInput, int lengthOfOutput) {
			LengthOfInput = lengthOfInput;
			LengthOfOutput = lengthOfOutput;
		}
	}
}
