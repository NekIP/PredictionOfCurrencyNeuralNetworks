using System;
using System.Collections.Generic;
using System.Linq;

namespace NeuralNetwork.Details {
	// Пока я реализую lstm лишь для случая когда слой состоит из одной клетки, в дальнейшем же 
	// данный класс будет содержать список клеток и в каждой клетки будет содержимое данного класса
	public class LstmLayer {
		public List<LstmCell> Cells { get; set; }
		public Vector Input { get; set; }
		public Vector Output { get; set; }

		private Activation Sigmoid;

		public LstmLayer(RecurentCellParameters[] cellsParameters, Activation sigmoid, Activation tanh) {
			InitializeCells(cellsParameters, sigmoid, tanh);
			Input = new Vector(cellsParameters.First().LengthOfInput);
			Output = new Vector(cellsParameters.Last().LengthOfOutput);
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

		public (Vector error, Vector[] diffsOutput, Vector[] diffsForget) Learn(Vector actual, Vector ideal, 
			Vector[] diffsOutputFromNext, Vector[] diffsForgetFromNext, LstmGatesForLayer gatesLayer) {
			var diffsOutput = new Vector[Cells.Count];
			var diffsForget = new Vector[Cells.Count];
			var error = ideal is null 
				? null 
				: ((actual - ideal) ^ 2) * 0.5;
			var diffInputFromNextCell = ideal is null 
				? new Vector(Cells.Last().Output.Length) 
				: actual - ideal;
			for (var i = Cells.Count - 1; i >= 0; i--) {
				var diffOutputFromNextLayer = diffsOutputFromNext[i];
				var diffForgetFromNextLayer = diffsForgetFromNext[i];
				var gatesForCell = gatesLayer[i];
				(diffsOutput[i], diffsForget[i], diffInputFromNextCell) = Cells[i].Learn(diffInputFromNextCell, diffOutputFromNextLayer, 
					diffForgetFromNextLayer, gatesForCell);
			}
			return (error, diffsOutput, diffsForget);
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

		private void InitializeCells(RecurentCellParameters[] cellsParameters, Activation sigmoid, Activation tanh) {
			Cells = new List<LstmCell>();
			for (var i = 0; i < cellsParameters.Length; i++) {
				Cells.Add(new LstmCell(cellsParameters[i].LengthOfInput, cellsParameters[i].LengthOfOutput, 
					sigmoid, tanh));
			}
		}
	}
}
