using System.Collections.Generic;
using System.Linq;

namespace NeuralNetwork {
	public class Lstm : Recurent {
		public RecurentParameters Parameters { get; set; }
		public List<LstmLayer> SequenceOfLayers { get; set; }
		public LstmGatesForLayer GatesForLayer { get; set; }
		public LstmLayer BaseLayerLstm { get; set; }

		/// <summary>
		/// Initialize a lstm recurent neural network
		/// </summary>
		public Lstm(RecurentParameters parameters) {
			CheckDataOnError(parameters);
			Parameters = parameters;
			InitializeElementOfLstmAndGates(parameters);
		}

		public override (Vector[] outputValues, Vector[] errors) Learn(Vector[] inputs, Vector[] ideals) {
			var actuals = Run(inputs);
			var outputs = new Vector[inputs.Length];
			var errors = new Vector[inputs.Length];
			var (diffsOutputFromNext, diffsForgetFromNext) = InitOutputAndForgetDiffs();
			for (var i = SequenceOfLayers.Count - 1; i >= 0; i--) {
				var layer = SequenceOfLayers[i];
				(outputs[i], errors[i], diffsOutputFromNext, diffsForgetFromNext) = layer.Learn(
					inputs[i], ideals[i], diffsOutputFromNext, diffsForgetFromNext, GatesForLayer);
			}
			GatesForLayer.ApplyDiff(Parameters.LearnSpeed);
			GatesForLayer.InitDiffs(Parameters.Cells);
			return (actuals, errors);
		}

		public override Vector[] Run(Vector[] inputs) {
			var result = new Vector[inputs.Length];
			SequenceOfLayers = new List<LstmLayer>() { BaseLayerLstm.Copy() };
			for (var i = 0; i < inputs.Length; i++) {
				var layer = SequenceOfLayers[i];
				result[i] = layer.Run(inputs[i], GatesForLayer);
				if (i < inputs.Length - 1) {
					SequenceOfLayers.Add(layer.CopyOnNext());
				}
			}
			BaseLayerLstm = SequenceOfLayers.Last().Copy();
			return result;
		}

		private (Vector[] diffsOutput, Vector[] diffsForget) InitOutputAndForgetDiffs() {
			var diffsOutputFromNext = new Vector[Parameters.Cells.Length];
			var diffsForgetFromNext = new Vector[Parameters.Cells.Length];
			for (var i = 0; i < Parameters.Cells.Length; i++) {
				diffsOutputFromNext[i] = new Vector(Parameters.Cells[i].LengthOfOutput);
				diffsForgetFromNext[i] = new Vector(Parameters.Cells[i].LengthOfOutput);
			}
			return (diffsOutputFromNext, diffsForgetFromNext);
		}

		private void CheckDataOnError(RecurentParameters parameters) {
			CheckConditionOnException(parameters.Cells.Length < 1,
				"Count of cells must be greater than 0");
			CheckConditionOnException(parameters.Cells.First().LengthOfInput != parameters.LengthOfInput,
				"The length of the input vector for the entire LSTM " +
					"and the length of the input vector for the first cell in the layer should be equal");
			CheckConditionOnException(parameters.Cells.Last().LengthOfOutput != parameters.LengthOfOutput,
				"The length of the output vector for the entire LSTM " +
					"and the length of the output vector for the last cell in the layer must be equal to");
			for (var i = 0; i < parameters.Cells.Length - 1; i++) {
				CheckConditionOnException(parameters.Cells[i].LengthOfOutput != parameters.Cells[i + 1].LengthOfInput,
					$"The length of the output vector in cell number {i} must be equal to the length of the input vector in cell number {i + 1}");
			}
		}

		private void InitializeElementOfLstmAndGates(RecurentParameters parameters) {
			BaseLayerLstm = new LstmLayer(parameters, 
				new SigmoidActivation(parameters.ActivationCoefficient), 
				new HyperbolicActivation(parameters.ActivationCoefficient));
			GatesForLayer = new LstmGatesForLayer(parameters.Cells);
		}
	}
}
