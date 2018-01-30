using NeuralNetwork;
using System;

namespace Experiment {
	public class NeuralNetworkMatrixTest : Experiment {
		public override void Run() {
			var nt = new MultilayerPerceptron(
				new PerceptronParameters(),
				new SigmoidActivation(),
				2, 3, 2);
			//Helper.PrintMatrix((double[][])nt.Neurons);
			Console.WriteLine("Weights: ");
			//Helper.PrintMatrix(nt.Weights);
		}
	}
}
