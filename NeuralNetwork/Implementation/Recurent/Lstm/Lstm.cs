using System.Collections.Generic;
using System.Linq;

namespace NeuralNetwork {
	public class Lstm : Recurent {
		public RecurentParameters Parameters { get; set; }
		public List<LstmLayer> SequenceLayersLstm { get; set; }
		public LstmGates GatesForLayers { get; set; }
		public LstmLayer BaseLayerLstm { get; set; }

		private Activation Sigmoid;
		private Activation Tanh;

		/// <summary>
		/// Initialize a lstm recurent neural network
		/// </summary>
		/// <param name="lengthOfOutputSequence"></param>
		/// <param name="lengthsOfEachLayer">
		///		The lengths of the vectors on each of the layers. 
		///		The first number is the length of the input vector, and the last number is the length of the output vector
		/// </param>
		public Lstm(RecurentParameters parameters) {
			CheckConditionOnException(parameters.LengthOfOutputSequence < 1, "Length of output sequence must be greater than 0");
			/*CheckConditionOnException(lengthsOfEachLayer.Length < 2, 
				"lengthsOfEachLayer.Length should be greater than 2. " +
				"Because the first number is the length of the input vector, " +
				"and the last number is the length of the output vector");*/
			Parameters = parameters;
			InitializeElementOfLstmAndGates(parameters.LengthOfInput, parameters.LengthOfOutput);
			Sigmoid = new SigmoidActivation(parameters.ActivationCoefficient);
			Tanh = new HyperbolicActivation(parameters.ActivationCoefficient);
		}

		// пока количество выходов и количество входов должно совпадать
		public override (Vector[] outputValues, Vector[] errors) Learn(Vector[] inputs, Vector[] ideals) {
			var actuals = Run(inputs);
			var errors = new Vector[actuals.Length];
			//var diffOutputFromNext = 2 * (actuals.Last() - ideals.Last());
			var diffOutputFromNext = new Vector(Parameters.LengthOfOutput);
			var diffForgetFromNext = new Vector(Parameters.LengthOfOutput);
			for (var i = SequenceLayersLstm.Count - 1; i >= SequenceLayersLstm.Count - actuals.Length/*типа 0*/; i--) {
				/*var actual = actuals[i];
				var ind = SequenceLayersLstm.Count - i - 1;
				var layer = SequenceLayersLstm[ind];
				errors[ind] = (ideals[ind] - actuals[ind]) ^ 2;*/

				//var ind = SequenceLayersLstm.Count - i - 1;
				var layer = SequenceLayersLstm[i];
				errors[i] = (ideals[i] - actuals[i]) ^ 2;
				diffOutputFromNext += 2 * (actuals[i] - ideals[i]);

				var diffOutput = layer.OutputLayerGateResultO * diffOutputFromNext + diffForgetFromNext;

				var diffOutputGate = layer.Forget * diffOutputFromNext;
				var diffInputGate = layer.TanhLayerGateResultG * diffOutput;
				var diffTanhLayer = layer.InputLayerGateResultI * diffOutput;
				var diffForgetLayer = layer.ForgetFromPreviousLayer * diffOutput;

				var diffInputGateInput = Sigmoid.DeriveFunc(layer.InputLayerGateResultI) * diffInputGate;
				var diffForgetGateInput = Sigmoid.DeriveFunc(layer.ForgetGateResultF) * diffForgetLayer;
				var diffOutputGateInput = Sigmoid.DeriveFunc(layer.OutputLayerGateResultO) * diffOutputGate;
				var diffTanhLayerInput = Tanh.DeriveFunc(layer.TanhLayerGateResultG) * diffTanhLayer;
				
				GatesForLayers.InputLayerDiff += Matrix.Outer(diffInputGateInput, layer.InputConcatenated);
				GatesForLayers.ForgetLayerDiff += Matrix.Outer(diffForgetGateInput, layer.InputConcatenated);
				GatesForLayers.OutputLayerDiff += Matrix.Outer(diffOutputGateInput, layer.InputConcatenated);
				GatesForLayers.TanhLayerDiff += Matrix.Outer(diffTanhLayerInput, layer.InputConcatenated);
				GatesForLayers.BiasInputLayerDiff += diffInputGateInput;
				GatesForLayers.BiasForgetLayerDiff += diffForgetGateInput;
				GatesForLayers.BiasOutputLayerDiff += diffOutputGateInput;
				GatesForLayers.BiasTanhLayerDiff += diffTanhLayerInput;

				var diffInputConcataneted = Vector.CreateLikeA(layer.InputConcatenated);
				diffInputConcataneted += GatesForLayers.InputLayer.GetTransposed() * diffInputGateInput;
				diffInputConcataneted += GatesForLayers.ForgetLayer.GetTransposed() * diffForgetGateInput;
				diffInputConcataneted += GatesForLayers.OutputLayer.GetTransposed() * diffOutputGateInput;
				diffInputConcataneted += GatesForLayers.TanhLayer.GetTransposed() * diffTanhLayerInput;

				diffOutputFromNext = diffInputConcataneted.Section(Parameters.LengthOfInput);
				diffForgetFromNext = diffOutput * layer.ForgetGateResultF;
			}

			var lr = 0.1;
			// предпологается что Xt есть верное значение для Yt-1, если верное значение для слоя t-1 не задано
			GatesForLayers.InputLayer += lr * GatesForLayers.InputLayerDiff;
			GatesForLayers.ForgetLayer += lr * GatesForLayers.ForgetLayerDiff;
			GatesForLayers.OutputLayer += lr * GatesForLayers.OutputLayerDiff;
			GatesForLayers.TanhLayer += lr * GatesForLayers.TanhLayerDiff;

			GatesForLayers.BiasInputLayer += lr * GatesForLayers.BiasInputLayerDiff;
			GatesForLayers.BiasForgetLayer += lr * GatesForLayers.BiasForgetLayerDiff;
			GatesForLayers.BiasOutputLayer += lr * GatesForLayers.BiasOutputLayerDiff;
			GatesForLayers.BiasTanhLayer += lr * GatesForLayers.BiasTanhLayerDiff;

			GatesForLayers.InitDiffs(Parameters.LengthOfInput, Parameters.LengthOfOutput);

			return (actuals, errors);
		}

		public override Vector[] Run(Vector[] inputs) {
			CheckConditionOnException(inputs.Length < Parameters.LengthOfOutputSequence,
				"Length of output sequance and length of input must be equals");
			SequenceLayersLstm = new List<LstmLayer>() { BaseLayerLstm.Copy() };
			for (var i = 0; i < inputs.Length; i++) {
				var layer = SequenceLayersLstm[i];
				var gates = GatesForLayers;
				layer.Input = inputs[i];
				layer.InputConcatenated = Vector.Union(layer.Input, layer.OutputFromPreviousLayer);
				layer.ForgetGateResultF = Sigmoid.Func(gates.ForgetLayer * layer.InputConcatenated + gates.BiasForgetLayer);
				layer.InputLayerGateResultI = Sigmoid.Func(gates.InputLayer * layer.InputConcatenated + gates.BiasInputLayer);
				layer.TanhLayerGateResultG = Tanh.Func(gates.TanhLayer * layer.InputConcatenated + gates.BiasTanhLayer);
				layer.OutputLayerGateResultO = Sigmoid.Func(gates.OutputLayer * layer.InputConcatenated + gates.BiasOutputLayer);
				layer.Forget = layer.ForgetFromPreviousLayer * layer.ForgetGateResultF +
					layer.TanhLayerGateResultG * layer.InputLayerGateResultI;
				layer.Output = Tanh.Func(layer.Forget) *  layer.OutputLayerGateResultO;
				if (i < inputs.Length - 1) {
					SequenceLayersLstm.Add(layer.CopyOnNext());
				}
			}
			// под вопросом нужно ли тянуть все эти запоминающие вектора и далее
			BaseLayerLstm = SequenceLayersLstm.Last().Copy();
			var result = new Vector[Parameters.LengthOfOutputSequence];
			var length = SequenceLayersLstm.Count - Parameters.LengthOfOutputSequence;
			for (var i = SequenceLayersLstm.Count - 1; i >= length; i--) {
				result[i - length] = SequenceLayersLstm[i].Output;
			}
			return result;
		}

		private void InitializeElementOfLstmAndGates(int inputLength, int outputLength) {
			BaseLayerLstm = new LstmLayer(inputLength, outputLength);
			GatesForLayers = new LstmGates(inputLength, outputLength);
		}
	}
}
