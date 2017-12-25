using System;
using System.Collections.Generic;
using System.Text;

namespace NeuralNetwork.Math
{
    public class MathHelper
    {
        public T[][] Transpose<T>(T[][] matrix)
        {
            var result = new T[matrix.GetLength(1)][];
            for (var i = 0; i < matrix.GetLength(1); i++)
            {
                result[i] = new T[matrix.GetLength(0)];
                for (var j = 0; j < matrix.GetLength(0); j++)
                {
                    result[i][j] = matrix[j][i];
                }
            }
            return result;
        }
    }
}
