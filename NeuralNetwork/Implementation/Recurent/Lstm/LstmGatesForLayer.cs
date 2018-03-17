using Newtonsoft.Json;
using System.Collections.Generic;

namespace NeuralNetwork.Details {
	public class LstmGatesForLayer {
        [JsonProperty]
        public List<LstmGatesForCell> Gates { get; set; }

        public LstmGatesForLayer() { }

        public LstmGatesForLayer(RecurentCellParameters[] cellsParameters) {
			Gates = new List<LstmGatesForCell>();
			for (var i = 0; i < cellsParameters.Length; i++) {
				Gates.Add(new LstmGatesForCell(cellsParameters[i].LengthOfInput,
					cellsParameters[i].LengthOfOutput));
			}
		}

		public LstmGatesForCell this[int cellNumber] => Gates[cellNumber];

		public void ApplyDiff(double learnSpeed, double moment) {
			for (var i = 0; i < Gates.Count; i++) {
				Gates[i].ApplyDiffs(learnSpeed, moment);
			}
		}

		public void InitDiffs(RecurentCellParameters[] cellsParameters) {
			for (var i = 0; i < Gates.Count; i++) {
				Gates[i].InitDiffs(cellsParameters[i].LengthOfInput, cellsParameters[i].LengthOfOutput);
			}
		}
	}
}
