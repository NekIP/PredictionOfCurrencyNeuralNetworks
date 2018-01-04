using NeuralNetwork;
using System;
using System.Diagnostics;

namespace Experiment {
	public class PerceptronNeuralNetworkTest : Experiment {
		public override void Run() {
			var nt = new MultilayerPerceptron(
				new PerceptronParameters { LearningSpeed = 0.7, Moment = 0.3 },
				new SigmoidActivation(4),
				2, 2, 1);
			//Helper.PrintMatrix(nt.Neurons);
			/*nt.Weights = new double[2][][];
			nt.Weights[0] = new double[3][] {
				new double[] { 0.1, 0.2 },
				new double[] { 0.3, 0.4 },
				new double[] { 0.5, 0.6 },
			};
			nt.Weights[1] = new double[1][] {
				new double[] { 0.7, 0.9, 0.11 },
				//new double[] { 0.8, 0.14, 0.12 }
			};*/
			Console.WriteLine("Weights: ");
			//Helper.PrintMatrix(nt.Weights);
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
			var middleError = 0.0;
			for (var k = 1; k < 10000000; k++) {
				Vector result = null;
				Vector error = null;
				var st = new Stopwatch();
				st.Start();
				for (var j = 0; j < 1000; j++) {
					for (var i = 0; i < learn.Length - 1; i++) {
						
						(result, error) = nt.Learn(learn[i], new double[] { learn[i + 1][0] });
						middleError += error[0];
						//Console.WriteLine(result.Error[0] + "_");
					}
				}
				st.Stop();
				Console.Clear();
				Console.WriteLine(st.Elapsed);
				Console.WriteLine(middleError / nt.Epoch);
				Console.WriteLine(error[0]);
				for (var i = 0; i < learn.Length - 1; i++) {
					var res = nt.Run(learn[i]);
					var result1 = nt.ConvertOutput(res);
					Console.WriteLine(result1[0] + "\t" + learn[i + 1][0]);
				}
			}

			middleError = middleError / (10000 * 11);
			Console.WriteLine(middleError + "_");
		}
	}
}