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
				ActivationCoefficient = 1,
				LengthOfOutputSequence = 3,
				LengthOfInput = 3,
				LengthOfOutput = 2
			});

			/*lstm.GatesForLayers.ForgetLayer = new double[][] {
				new [] { 0.0097627, 0.04303787, 0.02055268, 0.00897664, -0.01526904 },
				new [] { 0.02917882, -0.01248256, 0.0783546, 0.09273255, -0.0233117 }
			};
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

			var ideal = new Vector[] {
				new [] { -0.5, 0.2 },
				new [] { 0.1, 0.2 },
				new [] { 0.4, 0.1 }
			};
			var input = new Vector[] {
				new [] { 0.60276938, 0.54488318, 0.4236548 },
				new [] { 0.64589411, 0.43758721, 0.8911773 },
				new [] { 0.96366276, 0.38344152, 0.79172504 }
			};
			var t = new TimeSpan(0);
			for (var i = 0; i < 1000; i++) {
				var perf = new Stopwatch();
				perf.Start();
				var (outputs, errors) = lstm.Learn(input, ideal);
				perf.Stop();
				t += perf.Elapsed;
				Console.WriteLine("Learn:\t" + i + "\t time = " + (t / (i + 1)));
				for (var j = 0; j < outputs.Length; j++) {
					Console.WriteLine("\tOutput:\t" + j);
					Console.WriteLine("\t\tI:\t" + ideal[j]);
					Console.WriteLine("\t\tO:\t" + Vector.Convert(outputs[j], x => Math.Round(x, 1)));
					Console.WriteLine("\t\tE:\t" + errors[j]);
				}
			}
		}
	}
}
