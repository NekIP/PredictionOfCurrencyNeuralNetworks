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
	}
}
