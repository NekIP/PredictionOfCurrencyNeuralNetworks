namespace NeuralNetwork {
	public class Lstm : Recurent {
		public LstmLayer[] Layers { get; set; }

		public Lstm(int lengthOfInputSequence, int lengthOfInputVectorInEachLayer, 
			int lengthOfOutputSequence, int lengthOfOutputVectorInEachLayer) {

		}

		public override (Vector[] outputValue, Vector[] error) Learn(Vector[] input, Vector[] ideal) {
			throw new System.NotImplementedException();
		}

		public override Vector[] Run(Vector[] input) {
			throw new System.NotImplementedException();
		}
	}
}
