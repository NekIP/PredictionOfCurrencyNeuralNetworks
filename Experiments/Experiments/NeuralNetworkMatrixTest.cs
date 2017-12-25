﻿using NeuralNetwork;
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
            var nt = new PerceptronNeuralNetwork(
                new PeceptronNeuralNetworkParameters(),
                new SigmoidActivation(),
                2, 3, 2);
            PrintMatrix(nt.Neurons);
            Console.WriteLine("Weights: ");
            PrintMatrix(nt.Weights);
        }

        private void PrintMatrix(double[][] matrix)
        {
            for (var i = 0; i < matrix.Length; i++)
            {
                for (var j = 0; j < matrix[i].Length; j++)
                {
                    Console.Write(matrix[i][j] + "\t");
                }
                Console.WriteLine();
            }
        }

        private void PrintMatrix(double[][][] matrix)
        {
            for (var i = 0; i < matrix.Length; i++)
            {
                Console.WriteLine("Layer " + i);
                for (var j = 0; j < matrix[i].Length; j++)
                {
                    for (var k = 0; k < matrix[i][j].Length; k++)
                    {
                        Console.Write(matrix[i][j][k] + "\t");
                    }
                    Console.WriteLine();
                }
            }
        }
    }
}
