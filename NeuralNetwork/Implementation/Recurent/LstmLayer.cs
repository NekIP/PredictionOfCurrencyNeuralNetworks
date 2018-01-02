using System;

namespace NeuralNetwork {
	public class LstmLayer {
		public Activation Activation { get; set; }
		public Matrix Weights { get; set; }
		public Matrix DeffWeights { get; set; }
		public Vector Bias { get; set; }
		public Vector Input { get; set; }
		public Vector Output { get; set; }

		public LstmLayer(int lengthOfInput, int lengthOfOutput, Activation activation) {
			Activation = activation;
			InitVectors(lengthOfInput, lengthOfOutput);
			InitMatrixes(lengthOfInput, lengthOfOutput);
		}

		public Vector Run(Vector input) {
			Input = Vector.Convert(input, Activation.Func);
			Output = Vector.Convert(Weights * Input + Bias, Activation.Func);
			return Output;
		}

		private void InitVectors(int lengthOfInput, int lengthOfOutput) {
			Input = new Vector(lengthOfInput);
			Output = new Vector(lengthOfOutput);
			Bias = new Vector(lengthOfOutput);
		}

		private void InitMatrixes(int lengthOfInput, int lengthOfOutput) {
			var rnd = new Random();
			Weights = new Matrix(lengthOfOutput, lengthOfInput, () => rnd.NextDouble());
			DeffWeights = new Matrix(lengthOfOutput, lengthOfInput);
		}
	}
}
