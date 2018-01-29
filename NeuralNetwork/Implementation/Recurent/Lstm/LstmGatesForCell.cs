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
			InitDiffs(lengthOfInput, lengthOfOutput);
		}

		public void ApplyDiffs(double learnSpeed) {
			InputLayer -= learnSpeed * InputLayerDiff;
			ForgetLayer -= learnSpeed * ForgetLayerDiff;
			OutputLayer -= learnSpeed * OutputLayerDiff;
			TanhLayer -= learnSpeed * TanhLayerDiff;
			BiasInputLayer -= learnSpeed * BiasInputLayerDiff;
			BiasForgetLayer -= learnSpeed * BiasForgetLayerDiff;
			BiasOutputLayer -= learnSpeed * BiasOutputLayerDiff;
			BiasTanhLayer -= learnSpeed * BiasTanhLayerDiff;
		}

		public void InitDiffs(int lengthOfInput, int lengthOfOutput) {
			var commonLength = lengthOfInput + lengthOfOutput;
			ForgetLayerDiff = new Matrix(lengthOfOutput, commonLength);
			InputLayerDiff = new Matrix(lengthOfOutput, commonLength);
			TanhLayerDiff = new Matrix(lengthOfOutput, commonLength);
			OutputLayerDiff = new Matrix(lengthOfOutput, commonLength);
			BiasForgetLayerDiff = new Vector(lengthOfOutput);
			BiasInputLayerDiff = new Vector(lengthOfOutput);
			BiasTanhLayerDiff = new Vector(lengthOfOutput);
			BiasOutputLayerDiff = new Vector(lengthOfOutput);
		}
	}
}
