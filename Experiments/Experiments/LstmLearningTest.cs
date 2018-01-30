using NeuralNetwork;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;

namespace Experiment {
	public class LstmLearningTest : Experiment {
		public override void Run() {
			var rnd = new Random();
			var lstm = new Lstm(2, 1, new RecurentParameters(0.5, 1, 0.5), 
				new RecurentCellParameters(2, 4),
				new RecurentCellParameters(4, 1));

			/*new RecurentParameters {
				ActivationCoefficient = 0.5,
				LearnSpeed = 1,
				LengthOfInput = 2,
				LengthOfOutput = 1,
				LengthOfOutputSequence = 3,
				LayerCount = 3,
				Cells = new RecurentCellParameters[] {
					new RecurentCellParameters(2, 4),
					new RecurentCellParameters(4, 1)
				}
			}*/

			/*lstm.GatesForLayers.ForgetLayer = new double[][] {
				new [] { 0.0097627, 0.04303787, 0.02055268, 0.00897664, -0.01526904 },
				new [] { 0.02917882, -0.01248256, 0.0783546, 0.09273255, -0.0233117 }
			};s
			lstm.GatesForLayers.InputLayer = new double[][] {
				new [] { 0.0097627, 0.04303787, 0.02055268, 0.00897664, -0.01526904 },
				new [] { 0.02917882, -0.01248256, 0.0783546, 0.09273255, -0.0233117 }
			};
			lstm.GatesForLayers.OutputLayer = new double[][] {
				new [] { 0.0097627, 0.04303787, 0.02055268, 0.00897664, -0.01526904 },
				new [] { 0.02917882, -0.01248256, 0.0783546, 0.09273255, -0.0233117 }
			};
			lstm.GatesForLayers.TanhLayer = new double[][] {
				new [] { 0.0097627, 0.04303787, 0.02055268, 0.00897664, -0.01526904 },
				new [] { 0.02917882, -0.01248256, 0.0783546, 0.09273255, -0.0233117 }
			};

			lstm.GatesForLayers.BiasForgetLayer = new [] { 0.0097627, 0.04303787 };
			lstm.GatesForLayers.BiasInputLayer = new [] { 0.0097627, 0.04303787 };
			lstm.GatesForLayers.BiasOutputLayer = new [] { 0.0097627, 0.04303787 };
			lstm.GatesForLayers.BiasTanhLayer = new [] { 0.0097627, 0.04303787 };*/

			//lstm.BaseLayerLstm.Forget = new Vector(2, () => rnd.NextDouble());
			//lstm.BaseLayerLstm.ForgetFromPreviousLayer = new Vector(2, () => rnd.NextDouble());
			//lstm.BaseLayerLstm.OutputFromPreviousLayer = new Vector(2, () => rnd.NextDouble());
			var learn = new Vector[] {
				new [] { 0.018088, 0.01591 },
				new [] { 0.0248, -0.00912 },
				new [] { -0.013727, 0.00502 },
				new [] { -0.023491, 0.007678 },
				new [] { -0.011982, 0.025521 },
				new [] { 0.00835, -0.0316 },
				new [] { 0.041049, -0.041505 },
				new [] { 0.050914, -0.046292 },
				/*new [] { 0.076138, -0.106684 },
				new [] { 0.131035, -0.092031 },
				new [] { 0.206694, -0.209201 },*/
			};

			var ideal1 = new Vector[] {
				new [] { 0.0248, -0.00912 },
				new [] { -0.013727, 0.00502 },
				new [] { -0.023491, 0.007678 },
				new [] { -0.011982, 0.025521 },
				new [] { 0.00835, -0.0316 },
				new [] { 0.041049, -0.041505 },
				new [] { 0.050914, -0.046292 },
				new [] { 0.076138, -0.106684 },
				/*new [] { 0.131035, -0.092031 },
				new [] { 0.206694, -0.209201 },
				new [] { 0.168238, -0.211099 }*/
			};
			var ideal = new Vector[] {
				new [] { -0.4 },
				new [] { 0.1 },
				new [] { 0.4 }
			};
			var input = new Vector[] {
				new [] { -0.3, -0.1 },
				new [] { 0.7, 0.8 },
				new [] { 0.2, 0.1 }
			};
			var errorCommon = 0.0;
			var minError = double.MaxValue;
			var t = new TimeSpan(0);
			for (var i = 0; i < 10000000; i++) {
				var perf = new Stopwatch();
				
				perf.Start();
				var (outputs, errors) = lstm.Learn(input, ideal);
				perf.Stop();
				t += perf.Elapsed;
				if (outputs.ToList().Any(x => double.IsNaN(x[0]))) {
					Console.WriteLine("NaN");
					for (var h = 0; h < 5; h++) {
						Console.Beep(1000, 700);
						Thread.Sleep(100);
					}
					Console.ReadLine();
					Console.ReadLine();
					Console.ReadLine();
				}
				var error = errors.Sum(x => x[0]);
				if (error <= minError) {
					minError = error;
				}
				if (error < 0.000000000000000000001) {
					Console.WriteLine("End \t" + lstm.Epoch);
					Console.ReadLine();
				}
				/*else {
					Console.WriteLine("Increath");
					Console.ReadLine();
				}*/
				errorCommon += error;
				/*Console.WriteLine("Learn:\t" + i + "\t time = " + (t / (i + 1)));
				for (var j = 0; j < outputs.Length; j++) {
					Console.WriteLine("\tOutput:\t" + j);
					Console.WriteLine("\t\tI:\t" + ideal[j]);
					Console.WriteLine("\t\tO:\t" + outputs[j]);
					Console.WriteLine("\t\tE:\t" + errors[j]);
				}
				Console.WriteLine("Common error:\t" + errorCommon / lstm.Epoch);
				Console.WriteLine("Min error:\t" + minError);*/
				Console.WriteLine("Error:\t" + error);
				//Console.ReadKey();
			}
		}
	}
}
