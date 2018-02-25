using System;
using System.Collections.Generic;

namespace Experiment {
	public class Program {
		private static List<Experiment> Experiments = new List<Experiment>
		{
            //new ConvertInputAndOutputDataExperimental(),
            //new UnsafeReferenceMatrix(),
            //new NeuralNetworkMatrixTest(),
			//new PerceptronNeuralNetworkTest(),
			//new LstmTest(),
			//new LstmLearningTest(),
            new UsdToRubCurrencyExperiment(),
            //new InterpolationTesst()
		};

		public static void Main(string[] args) {
			foreach (var item in Experiments) {
				item.Run();
			}
			Console.ReadKey();
		}
	}
}
