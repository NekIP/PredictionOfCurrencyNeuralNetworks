namespace NeuralNetwork {
	public class Lstm : Recurent {
		public int LengthOfOutputSequence {
			get {
				return PLengthOfOutputSequence;
			}
			set {
				CheckConditionOnException(value > Layers.Length, 
					"The length of the output sequence can not exceed the length of the input");
				PLengthOfOutputSequence = value;
			}
		}

		public LstmLayer[] Layers { get; set; }

		private int PLengthOfOutputSequence = 1;

		public Lstm(RecurentParameters parameters, int lengthOfInputSequence, int lengthOfInputVectorInEachLayer, 
			int lengthOfOutputSequence, int lengthOfOutputVectorInEachLayer) {
			Layers = new LstmLayer[lengthOfInputSequence];
			for (var i = 0; i < Layers.Length; i++) {
				Layers[i] = new LstmLayer(parameters, lengthOfInputVectorInEachLayer, lengthOfOutputVectorInEachLayer);
			}
			LengthOfOutputSequence = lengthOfOutputSequence;
		}

		public override (Vector[] outputValue, Vector[] error) Learn(Vector[] input, Vector[] ideal) {
			throw new System.NotImplementedException();
		}

		public override Vector[] Run(Vector[] input) {
			throw new System.NotImplementedException();
		}
	}
}
