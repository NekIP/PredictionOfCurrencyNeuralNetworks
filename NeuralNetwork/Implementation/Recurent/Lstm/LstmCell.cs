using System;
using System.Collections.Generic;
using System.Text;

namespace NeuralNetwork {
	public class LstmCell {
		public Vector Forget { get; set; }
		public Vector Input { get; set; }
		/// <summary>
		/// This is the combined vector of the input and the previous output(Left - Input, Right - OutputFromPreviousLayer)
		/// </summary>
		public Vector InputConcatenated { get; set; }
		public Vector Output { get; set; }
		public Vector OutputFromPreviousLayer { get; set; }
		public Vector ForgetFromPreviousLayer { get; set; }
		public Vector ForgetGateResultF { get; set; }
		public Vector InputLayerGateResultI { get; set; }
		public Vector TanhLayerGateResultG { get; set; }
		public Vector OutputLayerGateResultO { get; set; }

		private Activation Sigmoid;
		private Activation Tanh;

		// нужно передать значение активации
		public LstmCell(int lengthOfInput, int lengthOfOutput, Activation sigmoid, Activation tanh) {
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
			Output = Forget * OutputLayerGateResultO;
			return Output;
		}

		public (Vector diffOutput, Vector diffForget, Vector diffInput) Learn(Vector diffInputFromNextCell, 
			Vector diffOutputFromNextLayer, Vector diffForgetFromNextLayer, LstmGatesForCell gatesForCell) {
			var diffOutput = OutputLayerGateResultO * diffOutputFromNextLayer + diffForgetFromNextLayer;
			var diffOutputGate = Forget * diffOutputFromNextLayer;
			var diffInputGate = TanhLayerGateResultG * diffOutput;
			var diffTanhLayer = InputLayerGateResultI * diffOutput;
			var diffForgetLayer = ForgetFromPreviousLayer * diffOutput;
			var diffInputGateInput = Sigmoid.DeriveFunc(InputLayerGateResultI) * diffInputGate;
			var diffForgetGateInput = Sigmoid.DeriveFunc(ForgetGateResultF) * diffForgetLayer;
			var diffOutputGateInput = Sigmoid.DeriveFunc(OutputLayerGateResultO) * diffOutputGate;
			var diffTanhLayerInput = Tanh.DeriveFunc(TanhLayerGateResultG) * diffTanhLayer;
			gatesForCell.InputLayerDiff += Matrix.Outer(diffInputGateInput, InputConcatenated);
			gatesForCell.ForgetLayerDiff += Matrix.Outer(diffForgetGateInput, InputConcatenated);
			gatesForCell.OutputLayerDiff += Matrix.Outer(diffOutputGateInput, InputConcatenated);
			gatesForCell.TanhLayerDiff += Matrix.Outer(diffTanhLayerInput, InputConcatenated);
			gatesForCell.BiasInputLayerDiff += diffInputGateInput;
			gatesForCell.BiasForgetLayerDiff += diffForgetGateInput;
			gatesForCell.BiasOutputLayerDiff += diffOutputGateInput;
			gatesForCell.BiasTanhLayerDiff += diffTanhLayerInput;
			var diffInputConcataneted = Vector.CreateLikeA(InputConcatenated);
			diffInputConcataneted += gatesForCell.InputLayer.GetTransposed() * diffInputGateInput;
			diffInputConcataneted += gatesForCell.ForgetLayer.GetTransposed() * diffForgetGateInput;
			diffInputConcataneted += gatesForCell.OutputLayer.GetTransposed() * diffOutputGateInput;
			diffInputConcataneted += gatesForCell.TanhLayer.GetTransposed() * diffTanhLayerInput;
			var diffOutputOnNext = diffInputConcataneted.Skip(Input.Length);
			var diffInputOnNext = diffInputConcataneted.Take(Input.Length);
			var diffForgetOnNext = diffOutput * ForgetGateResultF;
			return (diffOutputOnNext, diffForgetOnNext, diffInputOnNext);
		}

		public LstmCell Copy() {
			var result = new LstmCell(Input.Length, Output.Length, Sigmoid, Tanh);
			//result.Forget = Forget.Copy();
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
	}
}
