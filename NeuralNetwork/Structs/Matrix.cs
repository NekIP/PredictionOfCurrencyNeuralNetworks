using System;
using System.Linq;

namespace NeuralNetwork {
	public class Matrix {
		public Vector[] Vectors { get; private set; }
		public int RowCount => Vectors.Length;
		public int ColumnCount => Vectors.First().Length;

		public Matrix(double[][] values) {
			var сountElementsInFirstVectors = values.First().Length;
			Vectors = new Vector[values.GetLength(0)];
			for (var i = 0; i < Vectors.Length; i++) {
				Vectors[i] = new Vector(values[i]);
				if (values[i].Length != сountElementsInFirstVectors) {
					throw new Exception("Matrix must be rectangular");
				}
			}
		}

		public Matrix(int countRow, int countColumn, Func<double> initializer = null) {
			Vectors = new Vector[countRow];
			for (var i = 0; i < Vectors.Length; i++) {
				Vectors[i] = new Vector(countColumn, initializer);
			}
		}

		protected Matrix() { }

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

		public Matrix GetTransposed() {
			var result = new Matrix(ColumnCount, RowCount);
			for (var i = 0; i < RowCount; i++) {
				for (var j = 0; j < ColumnCount; j++) {
					result[j][i] = Vectors[i][j];
				}
			}

			return result;
		}

		public override string ToString() {
			var result = "[";
			for (var i = 0; i < Vectors.Length - 1; i++) {
				result += Vectors[i] + ", ";
			}
			return result + Vectors.Last() + "]";
		}

		public static Matrix operator *(Matrix matrix1, Matrix matrix2) {
			if (matrix1.ColumnCount != matrix2.RowCount) {
				throw new ArithmeticException("Count column of matrix1 and count row of matrix2 must be equals");
			}
			var result = new Matrix(matrix1.RowCount, matrix1.ColumnCount);
			for (int i = 0; i < matrix1.RowCount; i++) {
				for (int j = 0; j < matrix2.ColumnCount; j++) {
					for (int k = 0; k < matrix2.RowCount; k++) {
						result[i, j] += matrix1[i, k] * matrix2[k, j];
					}
				}
			}
			return result;
		}

		public static Vector operator *(Matrix matrix, Vector vector) {
			if (matrix.ColumnCount != vector.Length) {
				throw new ArithmeticException("Count column of matrix and length of vector must be equals");
			}
			var result = new Vector(matrix.RowCount);
			for (int i = 0; i < matrix.RowCount; i++) {
				for (int j = 0; j < matrix.ColumnCount; j++) {
					result[i] += matrix[i, j] * vector[j];
				}
			}
			return result;
		}

		public static Vector operator *(Vector vector, Matrix matrix) {
			if (matrix.RowCount != vector.Length) {
				throw new ArithmeticException("Count row of matrix and length of vector must be equals");
			}
			var result = new Vector(matrix.ColumnCount);
			for (int i = 0; i < matrix.ColumnCount; i++) {
				for (int j = 0; j < matrix.RowCount; j++) {
					result[i] += matrix[j, i] * vector[j];
				}
			}
			return result;
		}

		public static Matrix operator -(Matrix matrix) => Convert(matrix, x => -x);

		public static Matrix operator +(Matrix matrix1, Matrix matrix2) =>
			Combine(matrix1, matrix2, (x1, x2) => x1 + x2);

		public static Matrix operator -(Matrix matrix1, Matrix matrix2) =>
			Combine(matrix1, matrix2, (x1, x2) => x1 - x2);

		public static Matrix Combine(Matrix matrix1, Matrix matrix2,
			Func<double, double, double> converter) {
			if (matrix1.RowCount != matrix2.RowCount || matrix1.ColumnCount != matrix2.ColumnCount) {
				throw new ArithmeticException("Count row and column of matrix must be equals");
			}
			var result = new Matrix(matrix1.RowCount, matrix1.ColumnCount);
			for (var i = 0; i < matrix1.RowCount; i++) {
				result[i] = Vector.Combine(matrix1[i], matrix2[i], converter);
			}
			return result;
		}

		public static Matrix Convert(Matrix matrix,
			Func<double, double> converter) {
			var result = new Matrix(matrix.RowCount, matrix.ColumnCount);
			for (var i = 0; i < matrix.RowCount; i++) {
				result[i] = Vector.Convert(matrix[i], converter);
			}
			return result;
		}

		public static Matrix Outer(Vector vector1, Vector vector2) {
			var matrix = new Matrix();
			matrix.Vectors = new Vector[vector1.Length];
			for (var i = 0; i < vector1.Length; i++) {
				matrix[i] = vector1[i] * vector2;
			}
			return matrix;
		}

		public static Matrix CreateLikeA(Matrix matrix, Func<double> initializer = null) =>
			new Matrix(matrix.RowCount, matrix.ColumnCount, initializer);

		public static implicit operator Matrix(double[][] matrix) => new Matrix(matrix);

		public static implicit operator Vector[](Matrix matrix) => matrix.Vectors;
	}
}
