using System;

namespace Experiment {
	public static class Helper {
		public static void PrintMatrix(double[][] matrix) {
			for (var i = 0; i < matrix.Length; i++) {
				for (var j = 0; j < matrix[i].Length; j++) {
					Console.Write(matrix[i][j] + "\t");
				}
				Console.WriteLine();
			}
		}

		public static void PrintMatrix(double[][][] matrix) {
			for (var i = 0; i < matrix.Length; i++) {
				Console.WriteLine("Layer " + i);
				for (var j = 0; j < matrix[i].Length; j++) {
					for (var k = 0; k < matrix[i][j].Length; k++) {
						Console.Write(matrix[i][j][k] + "\t");
					}
					Console.WriteLine();
				}
			}
		}
	}
}
