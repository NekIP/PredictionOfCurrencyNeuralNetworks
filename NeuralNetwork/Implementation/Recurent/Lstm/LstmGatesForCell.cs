using System;

namespace NeuralNetwork {
	/// <summary>
	/// This class stores the gate weights for each cell in all layers (deployed in time)
	/// </summary>
	public class LstmGatesForCell {
		public Matrix ForgetLayer { get; set; }	
		public Matrix InputLayer { get; set; }
		public Matrix TanhLayer { get; set; }
		public Matrix OutputLayer { get; set; }
		public Vector BiasForgetLayer { get; set; }
		public Vector BiasInputLayer { get; set; }
		public Vector BiasTanhLayer { get; set; }
		public Vector BiasOutputLayer { get; set; }

		public Matrix ForgetLayerDiff { get; set; }
		public Matrix InputLayerDiff { get; set; }
		public Matrix TanhLayerDiff { get; set; }
		public Matrix OutputLayerDiff { get; set; }
		public Vector BiasForgetLayerDiff { get; set; }
		public Vector BiasInputLayerDiff { get; set; }
		public Vector BiasTanhLayerDiff { get; set; }
		public Vector BiasOutputLayerDiff { get; set; }

		public Matrix ForgetLayerDiffPrevious { get; set; }
		public Matrix InputLayerDiffPrevious { get; set; }
		public Matrix TanhLayerDiffPrevious { get; set; }
		public Matrix OutputLayerDiffPrevious { get; set; }
		public Vector BiasForgetLayerDiffPrevious { get; set; }
		public Vector BiasInputLayerDiffPrevious { get; set; }
		public Vector BiasTanhLayerDiffPrevious { get; set; }
		public Vector BiasOutputLayerDiffPrevious { get; set; }

		public LstmGatesForCell(int lengthOfInput, int lengthOfOutput) {
			var commonLength = lengthOfInput + lengthOfOutput;
			var rnd = new Random();
			Func<double> initializer = () => rnd.NextDouble();
			ForgetLayer = new Matrix(lengthOfOutput, commonLength, initializer);
			InputLayer = new Matrix(lengthOfOutput, commonLength, initializer);
			TanhLayer = new Matrix(lengthOfOutput, commonLength, initializer);
			OutputLayer = new Matrix(lengthOfOutput, commonLength, initializer);
			BiasForgetLayer = new Vector(lengthOfOutput);
			BiasInputLayer = new Vector(lengthOfOutput);
			BiasTanhLayer = new Vector(lengthOfOutput);
			BiasOutputLayer = new Vector(lengthOfOutput);
			FirstInitDiffs(lengthOfInput, lengthOfOutput);
		}

		public void ApplyDiffs(double learnSpeed, double moment) {
			InputLayer -= learnSpeed * InputLayerDiff + moment * InputLayerDiffPrevious;
			ForgetLayer -= learnSpeed * ForgetLayerDiff + moment * ForgetLayerDiffPrevious;
			OutputLayer -= learnSpeed * OutputLayerDiff + moment * OutputLayerDiffPrevious;
			TanhLayer -= learnSpeed * TanhLayerDiff + moment * TanhLayerDiffPrevious;
			BiasInputLayer -= learnSpeed * BiasInputLayerDiff + moment * BiasInputLayerDiffPrevious;
			BiasForgetLayer -= learnSpeed * BiasForgetLayerDiff + moment * BiasForgetLayerDiffPrevious;
			BiasOutputLayer -= learnSpeed * BiasOutputLayerDiff + moment * BiasOutputLayerDiffPrevious;
			BiasTanhLayer -= learnSpeed * BiasTanhLayerDiff + moment * BiasTanhLayerDiffPrevious;
		}

		public void FirstInitDiffs(int lengthOfInput, int lengthOfOutput) {
			var commonLength = lengthOfInput + lengthOfOutput;
			ForgetLayerDiff = new Matrix(lengthOfOutput, commonLength);
			InputLayerDiff = new Matrix(lengthOfOutput, commonLength);
			TanhLayerDiff = new Matrix(lengthOfOutput, commonLength);
			OutputLayerDiff = new Matrix(lengthOfOutput, commonLength);
			BiasForgetLayerDiff = new Vector(lengthOfOutput);
			BiasInputLayerDiff = new Vector(lengthOfOutput);
			BiasTanhLayerDiff = new Vector(lengthOfOutput);
			BiasOutputLayerDiff = new Vector(lengthOfOutput);

			ForgetLayerDiffPrevious = new Matrix(lengthOfOutput, commonLength);
			InputLayerDiffPrevious = new Matrix(lengthOfOutput, commonLength);
			TanhLayerDiffPrevious = new Matrix(lengthOfOutput, commonLength);
			OutputLayerDiffPrevious = new Matrix(lengthOfOutput, commonLength);
			BiasForgetLayerDiffPrevious = new Vector(lengthOfOutput);
			BiasInputLayerDiffPrevious = new Vector(lengthOfOutput);
			BiasTanhLayerDiffPrevious = new Vector(lengthOfOutput);
			BiasOutputLayerDiffPrevious = new Vector(lengthOfOutput);
		}

		public void InitDiffs(int lengthOfInput, int lengthOfOutput) {
			var commonLength = lengthOfInput + lengthOfOutput;

			ForgetLayerDiffPrevious = ForgetLayerDiff;
			InputLayerDiffPrevious = InputLayerDiff;
			TanhLayerDiffPrevious = TanhLayerDiff;
			OutputLayerDiffPrevious = OutputLayerDiff;
			BiasForgetLayerDiffPrevious = BiasForgetLayerDiff;
			BiasInputLayerDiffPrevious = BiasInputLayerDiff;
			BiasTanhLayerDiffPrevious = BiasTanhLayerDiff;
			BiasOutputLayerDiffPrevious = BiasOutputLayerDiff;

			ForgetLayerDiff = new Matrix(lengthOfOutput, commonLength);
			InputLayerDiff = new Matrix(lengthOfOutput, commonLength);
			TanhLayerDiff = new Matrix(lengthOfOutput, commonLength);
			OutputLayerDiff = new Matrix(lengthOfOutput, commonLength);
			BiasForgetLayerDiff = new Vector(lengthOfOutput);
			BiasInputLayerDiff = new Vector(lengthOfOutput);
			BiasTanhLayerDiff = new Vector(lengthOfOutput);
			BiasOutputLayerDiff = new Vector(lengthOfOutput);
		}

		public void CalculateDiff(Vector diffInputGate, Vector diffForgetGate,
			Vector diffOutputGate, Vector diffTanhGate, Vector inputConcatenated) {
			InputLayerDiff += Matrix.Outer(diffInputGate, inputConcatenated);
			ForgetLayerDiff += Matrix.Outer(diffForgetGate, inputConcatenated);
			OutputLayerDiff += Matrix.Outer(diffOutputGate, inputConcatenated);
			TanhLayerDiff += Matrix.Outer(diffTanhGate, inputConcatenated);
			BiasInputLayerDiff += diffInputGate;
			BiasForgetLayerDiff += diffForgetGate;
			BiasOutputLayerDiff += diffOutputGate;
			BiasTanhLayerDiff += diffTanhGate;
		}
	}
}
