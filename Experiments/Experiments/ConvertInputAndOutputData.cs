using System;

namespace Experiment {
	public class ConvertInputAndOutputDataExperimental : Experiment {
		public override void Run() {
			//TestSigmoid(0.005); // оптимальный коэффициент для типа double при функции активации simoid охват [-10000, 7025]
			//TestHyperbolicTangens(0.005);
			TestSigmoid(0.005);
		}

		private void TestSigmoid(double k) {
			TestActivationFunction(k, ActivationSigmoid, ActivationSigmoidReverse);

			double ActivationSigmoid(double x, double koef = 1) {
				return 1 / (1 + Math.Exp(-koef * x));
			}

			double ActivationSigmoidReverse(double gx, double koef = 1) {
				return -Math.Log(1 / gx - 1) / koef;
			}
		}

		private void TestHyperbolicTangens(double k) {
			TestActivationFunction(k, ActivationHyperbolic, ActivationHyperbolicReverse);

			double ActivationHyperbolic(double x, double koef = 1) {
				var exp = Math.Exp(koef * 2 * x);
				return (exp - 1) / (exp + 1);
			}

			double ActivationHyperbolicReverse(double gx, double koef = 1) {
				return Math.Log((1 + gx) / (1 - gx)) / (2 * koef);
			}
		}

		private void TestActivationFunction(double k,
			Func<double, double, double> activation,
			Func<double, double, double> activationReverse) {
			for (var i = -1d; i < 1; i += 0.01) {
				var activationResult = activation(i, k);
				var activationReverseResult = activationReverse(activationResult, k);
				Console.WriteLine(i + "\t" + activationResult + "\t" + activationReverseResult);
			}
		}
	}
}
