using NeuralNetwork;
using System;

namespace Experiment {
	public class PerceptronNeuralNetworkTest : Experiment {
		public override void Run() {
			var nt = new PerceptronNeuralNetwork(
				new PeceptronNeuralNetworkParameters { LearningSpeed = 0.7, Moment = 0.3 },
				new SigmoidActivation(4),
				2, 3, 2);
			Helper.PrintMatrix(nt.Neurons);
			nt.Weights = new double[2][][];
			nt.Weights[0] = new double[3][] {
				new double[] { 0.1, 0.2 },
				new double[] { 0.3, 0.4 },
				new double[] { 0.5, 0.6 },
			};
			nt.Weights[1] = new double[2][] {
				new double[] { 0.7, 0.9, 0.11 },
				new double[] { 0.8, 0.14, 0.12 }
			};
			Console.WriteLine("Weights: ");
			Helper.PrintMatrix(nt.Weights);
			var learn = new double[12][] {
				new [] { 0.018088, 0.01591 },
				new [] { 0.0248, -0.00912 },
				new [] { -0.013727, 0.00502 },
				new [] { -0.023491, 0.007678 },
				new [] { -0.011982, 0.025521 },
				new [] { 0.00835, -0.0316 },
				new [] { 0.041049, -0.041505 },
				new [] { 0.050914, -0.046292 },
				new [] { 0.076138, -0.106684 },
				new [] { 0.131035, -0.092031 },
				new [] { 0.206694, -0.209201 },
				new [] { 0.168238, -0.211099 }
			};

			nt.Learn(learn[0], learn[1]);
		}
	}
}