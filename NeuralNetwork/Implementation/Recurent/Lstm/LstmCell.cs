using DataAssistants.Structs;
using Newtonsoft.Json;

namespace NeuralNetwork.Details {
	public class LstmCell {
        [JsonProperty]
        public Vector Forget { get; set; }
        [JsonProperty]
        public Vector Input { get; set; }
        [JsonProperty]
        /// <summary>
        /// This is the combined vector of the input and the previous output(Left - Input, Right - OutputFromPreviousLayer)
        /// </summary>
        public Vector InputConcatenated { get; set; }
        [JsonProperty]
        public Vector Output { get; set; }
        [JsonProperty]
        public Vector OutputFromPreviousLayer { get; set; }
        [JsonProperty]
        public Vector ForgetFromPreviousLayer { get; set; }
        [JsonProperty]
        public Vector ForgetGateResultF { get; set; }
        [JsonProperty]
        public Vector InputLayerGateResultI { get; set; }
        [JsonProperty]
        public Vector TanhLayerGateResultG { get; set; }
        [JsonProperty]
        public Vector OutputLayerGateResultO { get; set; }
        [JsonProperty]
        public SigmoidActivation Sigmoid { get; private set; }
        [JsonProperty]
        public HyperbolicActivation Tanh { get; private set; }

        public LstmCell() { }

        // нужно передать значение активации
        public LstmCell(int lengthOfInput, int lengthOfOutput, SigmoidActivation sigmoid, HyperbolicActivation tanh) {
			InitializeData(lengthOfInput, lengthOfOutput);
			Sigmoid = sigmoid;
			Tanh = tanh;
		}

		public Vector CalculateAndGetOutput(Vector input, LstmGatesForCell gatesForCell) {
			Input = input;
			InputConcatenated = Vector.Union(Input, OutputFromPreviousLayer);
			ForgetGateResultF = Sigmoid.Func(gatesForCell.ForgetLayer * InputConcatenated + gatesForCell.BiasForgetLayer);
			InputLayerGateResultI = Sigmoid.Func(gatesForCell.InputLayer * InputConcatenated + gatesForCell.BiasInputLayer);
			TanhLayerGateResultG = Tanh.Func(gatesForCell.TanhLayer * InputConcatenated + gatesForCell.BiasTanhLayer);
			OutputLayerGateResultO = Sigmoid.Func(gatesForCell.OutputLayer * InputConcatenated + gatesForCell.BiasOutputLayer);
			Forget = ForgetFromPreviousLayer * ForgetGateResultF + TanhLayerGateResultG * InputLayerGateResultI;
			Output = Tanh.Func(Forget) * OutputLayerGateResultO;
			return Output;
		}

		public (Vector diffOutput, Vector diffForget, Vector diffInput) Learn(Vector diffInputFromNextCell, 
			Vector diffOutputFromNextLayer, Vector diffForgetFromNextLayer, LstmGatesForCell gatesForCell) {
			var diffOutput = diffInputFromNextCell + diffOutputFromNextLayer;
			var one = new Vector(Forget.Length, () => 1);
			var diffForget = diffOutput * OutputLayerGateResultO * (one - Tanh.Func(Forget) ^ 2) + diffForgetFromNextLayer;
			var (diffTanhGate, diffInputGate, diffForgetGate, diffOutputGate) = GetDiffForGates(diffForget, diffOutput);
			gatesForCell.CalculateDiff(diffInputGate, diffForgetGate, diffOutputGate, diffTanhGate, InputConcatenated);
			var diffInputConcataneted = GetDiffInputConcataneted(diffInputGate, diffForgetGate, diffOutputGate, diffTanhGate, gatesForCell);
			var diffOutputOnNext = diffInputConcataneted.Skip(Input.Length);
			var diffInputOnNext = diffInputConcataneted.Take(Input.Length);
			var diffForgetOnNext = diffForget * ForgetGateResultF;
			return (diffOutputOnNext, diffForgetOnNext, diffInputOnNext);
		}

		public LstmCell Copy() {
			var result = new LstmCell(Input.Length, Output.Length, Sigmoid, Tanh);
			result.OutputFromPreviousLayer = OutputFromPreviousLayer.Copy();
			result.ForgetFromPreviousLayer = ForgetFromPreviousLayer.Copy();
			return result;
		}

		public LstmCell CopyOnNext() {
			var result = new LstmCell(Input.Length, Output.Length, Sigmoid, Tanh);
			result.OutputFromPreviousLayer = Output.Copy();
			result.ForgetFromPreviousLayer = Forget.Copy();
			return result;
		}

		private void InitializeData(int lengthOfInput, int lengthOfOutput) {
			Input = new Vector(lengthOfInput);
			InputConcatenated = new Vector(lengthOfInput + lengthOfOutput);
			Output = new Vector(lengthOfOutput);
			Forget = new Vector(lengthOfOutput);
			OutputFromPreviousLayer = new Vector(lengthOfOutput);
			ForgetFromPreviousLayer = new Vector(lengthOfOutput);
			ForgetGateResultF = new Vector(lengthOfOutput);
			InputLayerGateResultI = new Vector(lengthOfOutput);
			TanhLayerGateResultG = new Vector(lengthOfOutput);
			OutputLayerGateResultO = new Vector(lengthOfOutput);
		}

		private (Vector diffTanhGate, Vector diffInputGate, Vector diffForgetGate, Vector diffOutputGate) GetDiffForGates(Vector diffForget, Vector diffOutput) {
			var diffTanhGate = diffForget * InputLayerGateResultI * Tanh.DeriveFunc(TanhLayerGateResultG);
			var diffInputGate = diffForget * TanhLayerGateResultG * Sigmoid.DeriveFunc(InputLayerGateResultI);
			var diffForgetGate = diffForget * ForgetFromPreviousLayer * Sigmoid.DeriveFunc(ForgetGateResultF);
			var diffOutputGate = diffOutput * Tanh.Func(Forget) * Sigmoid.DeriveFunc(OutputLayerGateResultO);
			return (diffTanhGate, diffInputGate, diffForgetGate, diffOutputGate);
		}

		private Vector GetDiffInputConcataneted(Vector diffInputGate, Vector diffForgetGate,
			Vector diffOutputGate, Vector diffTanhGate, LstmGatesForCell gatesForCell) {
			var result = gatesForCell.InputLayer.GetTransposed() * diffInputGate;
			result += gatesForCell.ForgetLayer.GetTransposed() * diffForgetGate;
			result += gatesForCell.OutputLayer.GetTransposed() * diffOutputGate;
			result += gatesForCell.TanhLayer.GetTransposed() * diffTanhGate;
			return result;
		}
	}
}
