using NeuralNetwork.Details;
using System.Collections.Generic;
using System.Linq;

namespace NeuralNetwork {
	public class Lstm : Recurent {
		public List<LstmLayer> SequenceOfLayers { get; private set; }
		public LstmGatesForLayer GatesForLayer { get; private set; }
		public LstmLayer BaseLayerLstm { get; private set; }

		public RecurentParameters Parameters { get; set; }
		public int LengthOfInput { get; private set; }
		public int LengthOfOutput { get; private set; }
		public int LayerCount { get; private set; }
		public int LengthOfOutputSequence { get; private set; }
		public RecurentCellParameters[] CellsParameters { get; private set; }

		public bool ReturnFullSequences { get; private set; }
		public bool LayerCountEqualsLengthOfInputSequence { get; private set; }

		public Lstm(RecurentParameters parameters) {

		}

		/// <summary>
		/// Creates a network with cellsParameters.Length (if only cellsParameters is given else 1) lstm cell in each layer 
		/// and the number of layers equal to the size of the input sequence of vectors. 
		/// All sequence is returned
		/// </summary>
		/// <param name="lengthOfInput">Length of input vector</param>
		/// <param name="lengthOfOutput">Length of output vector</param>
		/// <param name="cellsParameters"></param>
		public Lstm(int lengthOfInput, int lengthOfOutput, RecurentParameters parameters, params RecurentCellParameters[] cellsParameters) {
			CheckDataOnError(lengthOfInput, lengthOfOutput, cellsParameters);
			LengthOfInput = lengthOfInput;
			LengthOfOutput = lengthOfOutput;
			ReturnFullSequences = true;
			LayerCountEqualsLengthOfInputSequence = true;	
			Parameters = parameters;
			CellsParameters = cellsParameters;
			InitCellsParametersIfNotExist();
			InitializeElementOfLstmAndGates();
		}

		/// <summary>
		/// Creates a network with cellsParameters.Length lstm cell in each layer 
		/// and the number of layers equal to layerCount. 
		/// Returns a sequence of vectors of length lengthOfOutputSequence from the last layers
		/// </summary>
		/// <param name="lengthOfInput">Length of input vector</param>
		/// <param name="lengthOfOutput">Length of output vector</param>
		/// <param name="parameters"></param>
		public Lstm(int lengthOfInput, int lengthOfOutput, int layerCount, int lengthOfOutputSequence,
			RecurentParameters parameters, params RecurentCellParameters[] cellsParameters) {
			CheckDataOnError(lengthOfInput, lengthOfOutput, layerCount, lengthOfOutputSequence, cellsParameters);
			LengthOfInput = lengthOfInput;
			LengthOfOutput = lengthOfOutput;
			LayerCount = layerCount;
			LengthOfOutputSequence = lengthOfOutputSequence;
			ReturnFullSequences = false;
			LayerCountEqualsLengthOfInputSequence = false;
			Parameters = parameters;
			CellsParameters = cellsParameters;
			InitCellsParametersIfNotExist();
			InitializeElementOfLstmAndGates();
		}

		/// <summary>
		/// Creates a network with cellsParameters.Length lstm cell in each layer 
		/// and the number of layers equal to layerCount. 
		/// All sequence is returned
		/// </summary>
		/// <param name="lengthOfInput">Length of input vector</param>
		/// <param name="lengthOfOutput">Length of output vector</param>
		/// <param name="parameters"></param>
		public Lstm(int lengthOfInput, int lengthOfOutput, int layerCount,
			RecurentParameters parameters, params RecurentCellParameters[] cellsParameters) {
			CheckDataOnError(lengthOfInput, lengthOfOutput, cellsParameters);
			LengthOfInput = lengthOfInput;
			LengthOfOutput = lengthOfOutput;
			LayerCount = layerCount;
			ReturnFullSequences = true;
			LayerCountEqualsLengthOfInputSequence = false;
			Parameters = parameters;
			CellsParameters = cellsParameters;
			InitCellsParametersIfNotExist();
			InitializeElementOfLstmAndGates();
		}

		public override NeuralNetworkResult Run(NeuralNetworkData inputData) {
			var inputs = ConvertDataToSequence(inputData);
			var outputs = Run(inputs);
			return new NeuralNetworkResult(outputs);
		}

		public override NeuralNetworkLearnResult Learn(NeuralNetworkData inputData, NeuralNetworkData idealData) {
			var inputs = ConvertDataToSequence(inputData);
			var ideals = ConvertDataToSequence(idealData);
			var (outputs, errors) = Learn(inputs, ideals);
			return new NeuralNetworkLearnResult(outputs, errors);
		}

		public override NeuralNetworkLearnResult Learn(NeuralNetworkData inputData) {
			throw new System.NotImplementedException();
		}

		public (Vector[] output, Vector[] errors) Learn(Vector[] inputs, Vector[] ideals) {
			var actuals = Run(inputs);
			var errors = new Vector[SequenceOfLayers.Count];
			var (diffsOutputFromNext, diffsForgetFromNext) = InitOutputAndForgetDiffs();
			for (var (i, j) = (SequenceOfLayers.Count - 1, ideals.Length - 1); i >= 0; i--, j--) {
				var layer = SequenceOfLayers[i];
				var ideal = j >= 0 ? ideals[j] : null;
				(errors[i], diffsOutputFromNext, diffsForgetFromNext) = layer.Learn(
					SequenceOfLayers[i].Output, ideal, diffsOutputFromNext, diffsForgetFromNext, GatesForLayer);
			}
			GatesForLayer.ApplyDiff(Parameters.LearnSpeed, Parameters.Moment);
			GatesForLayer.InitDiffs(CellsParameters);
			SetNextEpoch();
			return (actuals, errors.TakeLast(actuals.Length).ToArray());
		}

		public Vector[] Run(Vector[] inputs) {
			var layerCount = GetLayerCount(inputs);
			var output = new Vector[layerCount];
			SequenceOfLayers = new List<LstmLayer>() { BaseLayerLstm.Copy() };
			for (var i = 0; i < layerCount; i++) {
				var layer = SequenceOfLayers[i];
				var input = inputs.Length > i ? inputs[i] : new Vector(LengthOfInput);
				output[i] = layer.Run(input, GatesForLayer);
				if (i < layerCount - 1) {
					SequenceOfLayers.Add(layer.CopyOnNext());
				}
			}
			BaseLayerLstm = SequenceOfLayers.Last().Copy();
			var result = ReturnFullSequences ? output : output.TakeLast(LengthOfOutputSequence).ToArray();
			return result;
		}

		private (Vector[] diffsOutput, Vector[] diffsForget) InitOutputAndForgetDiffs() {
			var diffsOutputFromNext = new Vector[CellsParameters.Length];
			var diffsForgetFromNext = new Vector[CellsParameters.Length];
			for (var i = 0; i < CellsParameters.Length; i++) {
				diffsOutputFromNext[i] = new Vector(CellsParameters[i].LengthOfOutput);
				diffsForgetFromNext[i] = new Vector(CellsParameters[i].LengthOfOutput);
			}
			return (diffsOutputFromNext, diffsForgetFromNext);
		}

		private void CheckDataOnError(int lengthOfInput, int lengthOfOutput, int layerCount, int lengthOfOutputSequence, RecurentCellParameters[] cellsParameters) {
			CheckConditionOnException(layerCount < lengthOfOutputSequence, 
				"The number of layers should not be less than the size of the output sequence");
			CheckDataOnError(lengthOfInput, lengthOfOutput, cellsParameters);
		}

		private void CheckDataOnError(int lengthOfInput, int lengthOfOutput, RecurentCellParameters[] cellsParameters) {
			CheckConditionOnException(lengthOfInput < 1, "The length of input vector should be greater than 0");
			CheckConditionOnException(lengthOfOutput < 1, "The length of output vector should be greater than 0");
			if (cellsParameters.Length > 0) {
				CheckConditionOnException(cellsParameters.First().LengthOfInput != lengthOfInput,
					"The length of the input vector for the entire LSTM " +
						"and the length of the input vector for the first cell in the layer should be equal");
				CheckConditionOnException(cellsParameters.Last().LengthOfOutput != lengthOfOutput,
					"The length of the output vector for the entire LSTM " +
						"and the length of the output vector for the last cell in the layer must be equal to");
				for (var i = 0; i < cellsParameters.Length - 1; i++) {
					CheckConditionOnException(cellsParameters[i].LengthOfOutput != cellsParameters[i + 1].LengthOfInput,
						$"The length of the output vector in cell number {i} must be equal to the length of the input vector in cell number {i + 1}");
				}
			}
		}

		private void InitCellsParametersIfNotExist() {
			if (CellsParameters is null || CellsParameters.Length < 1) {
				CellsParameters = new RecurentCellParameters[] {
					new RecurentCellParameters(LengthOfInput, LengthOfOutput)
				};
			}
		}

		private int GetLayerCount(Vector[] inputs) {
			if (LayerCountEqualsLengthOfInputSequence) {
				return inputs.Length;
			}
			else {
				CheckConditionOnException(inputs.Length > LayerCount, 
					"The length of input sequence should be less than count of layer");
				return LayerCount;
			}
		}

		private void InitializeElementOfLstmAndGates() {
			BaseLayerLstm = new LstmLayer(CellsParameters, 
				new SigmoidActivation(Parameters.ActivationCoefficient), 
				new HyperbolicActivation(Parameters.ActivationCoefficient));
			GatesForLayer = new LstmGatesForLayer(CellsParameters);
		}
	}
}
