using NeuralNetwork;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Experiment {
	public class LstmLearningTest : Experiment {
		public override void Run() {
			var rnd = new Random();
			var lstm = new Lstm(new RecurentParameters {
				LearnSpeed = 0.4,
				LengthOfInput = 2,
				LengthOfOutput = 2,
				Cells = new RecurentCellParameters[] {
					new RecurentCellParameters(2, 4),
					new RecurentCellParameters(4, 4),
					new RecurentCellParameters(4, 2),
				}
			});

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
				new [] { 0.076138, -0.106684 },
				new [] { 0.131035, -0.092031 },
				new [] { 0.206694, -0.209201 },
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
				new [] { 0.131035, -0.092031 },
				new [] { 0.206694, -0.209201 },
				new [] { 0.168238, -0.211099 }
			};
			var ideal = new Vector[] {
				new [] { -0.5 },
				new [] { 0.1 },
				new [] { 0.4 }
			};
			var input = new Vector[] {
				new [] { -0.3, -0.1 },
				new [] { 0.7, 0.8 },
				new [] { 0.2, 0.1 }
			};
			var t = new TimeSpan(0);
			for (var i = 0; i < 50000; i++) {
				var perf = new Stopwatch();
				perf.Start();
				var (outputs, errors) = lstm.Learn(learn, ideal1);
				perf.Stop();
				t += perf.Elapsed;
				Console.WriteLine("Learn:\t" + i + "\t time = " + (t / (i + 1)));
				for (var j = 0; j < outputs.Length; j++) {
					Console.WriteLine("\tOutput:\t" + j);
					Console.WriteLine("\t\tI:\t" + ideal1[j]);
					Console.WriteLine("\t\tO:\t" + lstm.ConvertOutput(outputs[j]));
					Console.WriteLine("\t\tE:\t" + errors[j]);
				}
			}
		}
	}
}
