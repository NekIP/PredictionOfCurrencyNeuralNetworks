using NeuralNetwork;
using System;
using System.Collections.Generic;
using System.Text;

namespace Experiment {
	public class LstmLearningTest : Experiment {
		public override void Run() {
			var lstm = new Lstm(new RecurentParameters {
				ActivationCoefficient = 1,
				LengthOfOutputSequence = 3,
				LengthOfInput = 3,
				LengthOfOutput = 2
			});

			lstm.GatesForLayers.ForgetLayer = new double[][] {
				new double[] { 0.0097627, 0.04303787, 0.02055268, 0.00897664, -0.01526904 },
				new double[] { 0.02917882, -0.01248256, 0.0783546, 0.09273255, -0.0233117 }
			};
			lstm.GatesForLayers.InputLayer = new double[][] {
				new double[] { 0.0097627, 0.04303787, 0.02055268, 0.00897664, -0.01526904 },
				new double[] { 0.02917882, -0.01248256, 0.0783546, 0.09273255, -0.0233117 }
			};
			lstm.GatesForLayers.OutputLayer = new double[][] {
				new double[] { 0.0097627, 0.04303787, 0.02055268, 0.00897664, -0.01526904 },
				new double[] { 0.02917882, -0.01248256, 0.0783546, 0.09273255, -0.0233117 }
			};
			lstm.GatesForLayers.TanhLayer = new double[][] {
				new double[] { 0.0097627, 0.04303787, 0.02055268, 0.00897664, -0.01526904 },
				new double[] { 0.02917882, -0.01248256, 0.0783546, 0.09273255, -0.0233117 }
			};

			lstm.GatesForLayers.BiasForgetLayer = new double[] { 0.0097627, 0.04303787 };
			lstm.GatesForLayers.BiasInputLayer = new double[] { 0.0097627, 0.04303787 };
			lstm.GatesForLayers.BiasOutputLayer = new double[] { 0.0097627, 0.04303787 };
			lstm.GatesForLayers.BiasTanhLayer = new double[] { 0.0097627, 0.04303787 };

		}
	}
}
