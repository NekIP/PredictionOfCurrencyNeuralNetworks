using System;
using System.Collections.Generic;
using System.Text;

namespace NeuralNetwork.Helper {
	public class MathHelper {
		public T[][] Transpose<T>(T[][] matrix) {
			var result = new T[matrix[0].Length][];
			for (var i = 0; i < matrix[0].Length; i++) {
				result[i] = new T[matrix.Length];
				for (var j = 0; j < matrix.Length; j++) {
					result[i][j] = matrix[j][i];
				}
			}
			return result;
		}

		public double[] Mull(double[][] matrix, double[] vector, Activation activation) {
			var result = new double[matrix.Length];
			for (int i = 0; i < matrix.Length; i++) {
				for (int j = 0; j < vector.Length; j++) {
					result[i] += matrix[i][j] * vector[j];
				}
				result[i] = activation.Func(result[i]);
			}
			return result;
		}

		public double[] MullWithTransposeMatrix(double[][] matrix, double[] vector, Activation activation) {
			var result = new double[matrix.Length];
			for (int j = 0; j < matrix.Length; j++) {
				for (int i = 0; i < vector.Length; i++) {
					result[i] += matrix[i][j] * vector[j];
				}
				result[j] = activation.Func(result[j]);
			}
			return result;
		}

		public double[][] CreateMatrix(int countRow, int countColumn, bool isRandom) {
			var rnd = new Random();
			var result = new double[countRow][];
			for (var i = 0; i < countRow; i++) {
				result[i] = new double[countColumn];
				for (var j = 0; j < countColumn; j++) {
					result[i][j] = isRandom ? rnd.NextDouble() : 0;
				}
			}
			return result;
		}
	}
}
