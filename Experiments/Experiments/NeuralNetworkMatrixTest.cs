using NeuralNetwork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Experiment
{
    public class NeuralNetworkMatrixTest : Experiment
    {
        public override void Run()
        {
            var nt = new MultilayerPerceptron(
                new MultilayerPerceptronParameters(),
                new SigmoidActivation(),
                2, 3, 2);
            //Helper.PrintMatrix((double[][])nt.Neurons);
            Console.WriteLine("Weights: ");
			//Helper.PrintMatrix(nt.Weights);
        }
    }
}
