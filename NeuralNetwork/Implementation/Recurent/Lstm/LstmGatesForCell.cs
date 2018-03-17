using DataAssistants.Structs;
using Newtonsoft.Json;
using System;

namespace NeuralNetwork.Details {
	/// <summary>
	/// This class stores the gate weights for each cell in all layers (deployed in time)
	/// </summary>
	public class LstmGatesForCell {
        [JsonProperty]
        public Matrix ForgetLayer { get; set; }
        [JsonProperty]
        public Matrix InputLayer { get; set; }
        [JsonProperty]
        public Matrix TanhLayer { get; set; }
        [JsonProperty]
        public Matrix OutputLayer { get; set; }
        [JsonProperty]
        public Vector BiasForgetLayer { get; set; }
        [JsonProperty]
        public Vector BiasInputLayer { get; set; }
        [JsonProperty]
        public Vector BiasTanhLayer { get; set; }
        [JsonProperty]
        public Vector BiasOutputLayer { get; set; }

        [JsonProperty]
        public Matrix ForgetLayerDiff { get; set; }
        [JsonProperty]
        public Matrix InputLayerDiff { get; set; }
        [JsonProperty]
        public Matrix TanhLayerDiff { get; set; }
        [JsonProperty]
        public Matrix OutputLayerDiff { get; set; }
        [JsonProperty]
        public Vector BiasForgetLayerDiff { get; set; }
        [JsonProperty]
        public Vector BiasInputLayerDiff { get; set; }
        [JsonProperty]
        public Vector BiasTanhLayerDiff { get; set; }
        [JsonProperty]
        public Vector BiasOutputLayerDiff { get; set; }

        [JsonProperty]
        public Matrix ForgetLayerDiffPrevious { get; set; }
        [JsonProperty]
        public Matrix InputLayerDiffPrevious { get; set; }
        [JsonProperty]
        public Matrix TanhLayerDiffPrevious { get; set; }
        [JsonProperty]
        public Matrix OutputLayerDiffPrevious { get; set; }
        [JsonProperty]
        public Vector BiasForgetLayerDiffPrevious { get; set; }
        [JsonProperty]
        public Vector BiasInputLayerDiffPrevious { get; set; }
        [JsonProperty]
        public Vector BiasTanhLayerDiffPrevious { get; set; }
        [JsonProperty]
        public Vector BiasOutputLayerDiffPrevious { get; set; }

        public LstmGatesForCell() { }

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
