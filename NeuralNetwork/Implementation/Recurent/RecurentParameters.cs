namespace NeuralNetwork {
	public class RecurentParameters {
		//public PerceptronParameters PerceptronsParameters { get; set; }
		public double ActivationCoefficient { get; set; } = 1;
		public double LearnSpeed { get; set; } = 0.1;
		public int LengthOfInput { get; set; }
		public int LengthOfOutput { get; set; }
		public RecurentCellParameters[] Cells { get; set; } 
	}
}
