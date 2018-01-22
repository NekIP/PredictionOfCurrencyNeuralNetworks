namespace NeuralNetwork {
	// Пока я реализую lstm лишь для случая когда слой состоит из одной клетки, в дальнейшем же 
	// данный класс будет содержать список клеток и в каждой клетки будет содержимое данного класса
	public class LstmLayer {
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

		public LstmLayer(int lengthOfInput, int lengthOfOutput) {
			InitializeData(lengthOfInput, lengthOfOutput);
		}

		public LstmLayer Copy() {
			var result = new LstmLayer(Input.Length, Output.Length);
			result.Forget = Forget.Copy();
			result.OutputFromPreviousLayer = OutputFromPreviousLayer.Copy();
			result.ForgetFromPreviousLayer = ForgetFromPreviousLayer.Copy();
			return result;
		}

		public LstmLayer CopyOnNext() {
			var result = new LstmLayer(Input.Length, Output.Length);
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
