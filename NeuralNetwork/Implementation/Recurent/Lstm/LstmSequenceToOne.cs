using System;

namespace NeuralNetwork {
	public class LstmSequenceToOne : RecurentSequenceToOne {
		public override (Vector outputValue, Vector error) Learn(Vector[] input, Vector ideal) {
			throw new NotImplementedException();
		}

		public override Vector Run(Vector[] input) {
			throw new NotImplementedException();
		}
	}
}
