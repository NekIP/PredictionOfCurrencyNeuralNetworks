using NeuralNetwork;
using System;

namespace Experiment {
	public class LstmTest : Experiment {
		public override void Run() {
			var lstm = new Lstm(new RecurentParameters {
				ActivationCoefficient = 1,
				LengthOfOutputSequence = 1,
				LengthOfInput = 2,
				LengthOfOutput = 1
			});
			var rnd = new Random();

			lstm.BaseLayerLstm.Forget = new Vector(1, () => rnd.NextDouble());
			lstm.BaseLayerLstm.ForgetFromPreviousLayer = new Vector(1, () => rnd.NextDouble());
			lstm.BaseLayerLstm.OutputFromPreviousLayer = new Vector(1, () => rnd.NextDouble());

			var result = lstm.Run(new Vector[] {
				new[] { 0.23, 0.57 },
				new[] { 0.567, 0.35 }
			});
			Console.WriteLine(result[0].ToString());
		}
	}
}
