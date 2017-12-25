using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace NeuralNetwork.Helper {
	public class Matrix {
		public Vector[] Vectors { get; private set; }

		public int CountRow => Vectors.Length;
		public int CountColumn => Vectors.First().Length;

		public Matrix(double[][] values) {
			Vectors = new Vector[values.GetLength(0)];
			for (var i = 0; i < Vectors.Length; i++) {
				Vectors[i] = new Vector(values[i]);
			}
		}

		public Matrix(int countRow, int countColumn) {
			Vectors = new Vector[countRow];
			for (var i = 0; i < Vectors.Length; i++) {
				Vectors[i] = new Vector(countColumn);
			}
		}

		public Vector this[int i] {
			get {
				return Vectors[i];
			}
			set {
				Vectors[i] = value;
			}
		}

		public double this[int i, int j] {
			get {
				return Vectors[i][j];
			}
			set {
				Vectors[i][j] = value;
			}
		}

		public static Matrix operator +(Matrix item1, Matrix item2) {
			if (item1.CountRow != item2.CountRow || item1.CountColumn != item2.CountColumn) {
				throw new ArithmeticException("Count row and column of matrix must be equals");
			}
			var result = new Matrix(item1.CountRow, item1.CountColumn);
			for (var i = 0; i < item1.CountRow; i++) {
				result[i] = item1[i] + item2[i];
			}
			return result;
		}

		public static Matrix operator -(Matrix item1, Matrix item2) {
			return item1 + (-item2);
		}

		public static Matrix operator *(Matrix item1, Matrix item2) {
			if (item1.CountColumn != item2.CountRow) {
				throw new ArithmeticException("Count column of matrix1 and count row of matrix2 must be equals");
			}
			var result = new Matrix(item1.CountRow, item1.CountColumn);
			for (int i = 0; i < item1.CountRow; i++) {
				for (int j = 0; j < item2.CountColumn; j++) {
					for (int k = 0; k < item2.CountRow; k++) {
						result[i, j] += item1[i, k] * item2[k, j];
					}
				}
			}
			return result;
		}

		public static Vector operator *(Matrix item1, Vector item2) {
			if (item1.CountColumn != item2.Length) {
				throw new ArithmeticException("Count column of matrix1 and count row of matrix2 must be equals");
			}
			var result = new Vector(item2.Length);
			for (int i = 0; i < item1.CountRow; i++) {
				for (int j = 0; j < item2.Length; j++) {
					result[i] += item1[i, j] * item2[j];
				}
			}
			return result;
		}

		public static Matrix operator -(Matrix item1) {
			var result = new Matrix(item1.CountRow, item1.CountColumn);
			for (var i = 0; i < item1.CountRow; i++) {
				result[i] = -item1[i];
			}
			return result;
		}
	}
}
