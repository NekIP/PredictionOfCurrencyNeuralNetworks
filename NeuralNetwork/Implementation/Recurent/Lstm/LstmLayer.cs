using System;
using System.Collections.Generic;
using System.Linq;

namespace NeuralNetwork {
	// Пока я реализую lstm лишь для случая когда слой состоит из одной клетки, в дальнейшем же 
	// данный класс будет содержать список клеток и в каждой клетки будет содержимое данного класса
	public class LstmLayer {
		public List<LstmCell> Cells { get; set; }
		public Vector Input { get; set; }
		public Vector Output { get; set; }

		private Activation Sigmoid;

		public LstmLayer(RecurentParameters parameters, Activation sigmoid, Activation tanh) {
			InitializeCells(parameters, sigmoid, tanh);
			Input = new Vector(parameters.Cells.First().LengthOfInput);
			Output = new Vector(parameters.Cells.Last().LengthOfOutput);
			Sigmoid = sigmoid;
		}

		protected LstmLayer() { }

		public Vector Run(Vector input, LstmGatesForLayer gatesLayer) {
			Input = input;
			var lastOutput = input;
			for (var cellNumber = 0; cellNumber < Cells.Count; cellNumber++) {
				var cell = Cells[cellNumber];
				var gatesForCell = gatesLayer[cellNumber];
				lastOutput = cell.CalculateAndGetOutput(lastOutput, gatesForCell);
			}
			Output = lastOutput; 
			return Output;
		}

		public (Vector output, Vector error, Vector[] diffsOutput, Vector[] diffsForget) Learn(Vector actual, Vector ideal, 
			Vector[] diffsOutputFromNext, Vector[] diffsForgetFromNext, LstmGatesForLayer gatesLayer) {
			var diffsOutput = new Vector[Cells.Count];
			var diffsForget = new Vector[Cells.Count];
			//ideal = ideal is null ? null : (new SigmoidActivation()).Func(ideal);
			var error = ideal is null ? null : (ideal - actual) ^ 2;
			var diffInputFromNextCell = new Vector(Cells.Last().Output.Length);
			for (var i = Cells.Count - 1; i >= 0; i--) {
				var diffOutputFromNextLayer = diffsOutputFromNext[i];
				if (i == Cells.Count - 1 && !(ideal is null)) {
					diffInputFromNextCell = 2 * (actual - ideal);
				}
				var diffForgetFromNextLayer = diffsForgetFromNext[i];
				var gatesForCell = gatesLayer[i];
				var (diffOutput, diffForget, diffInput) = Cells[i].Learn(diffInputFromNextCell, diffOutputFromNextLayer, 
					diffForgetFromNextLayer, gatesForCell);
				diffInputFromNextCell = diffInput;
				diffsOutput[i] = diffOutput;
				diffsForget[i] = diffForget;
			}
			return (actual, error, diffsOutput, diffsForget);
		}

		public LstmLayer Copy() => CopyTemplate(cell => cell.Copy());

		public LstmLayer CopyOnNext() => CopyTemplate(cell => cell.CopyOnNext());

		private LstmLayer CopyTemplate(Func<LstmCell, LstmCell> copyFunc) {
			var result = new LstmLayer();
			result.Cells = new List<LstmCell>();
			result.Input = new Vector(Input.Length);
			result.Output = new Vector(Output.Length);
			for (var i = 0; i < Cells.Count; i++) {
				result.Cells.Add(copyFunc(Cells[i]));
			}
			return result;
		}

		/*private void InitializeData(int lengthOfInput, int lengthOfOutput) {
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
		}*/

		private void InitializeCells(RecurentParameters parameters, Activation sigmoid, Activation tanh) {
			Cells = new List<LstmCell>();
			for (var i = 0; i < parameters.Cells.Length; i++) {
				Cells.Add(new LstmCell(parameters.Cells[i].LengthOfInput, parameters.Cells[i].LengthOfOutput, 
					sigmoid, tanh));
			}
		}
	}
}
