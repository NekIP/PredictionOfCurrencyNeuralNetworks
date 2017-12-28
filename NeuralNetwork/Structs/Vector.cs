﻿using System;
using System.Collections.Generic;
using System.Text;

namespace NeuralNetwork {
	public class Vector {
		public int Length => Values.Length;

		private double[] Values { get; set; }

		public Vector(double[] values) {
			Values = values;
		}

		public Vector(int length) {
			Values = new double[length];
		}

		public double this[int index] {
			get {
				return Values[index];
			}
			set {
				Values[index] = value;
			}
		}

		public static Vector operator +(Vector vector1, Vector vector2) =>
			Combine(vector1, vector2, (x1, x2) => x1 + x2);

		public static Vector operator -(Vector vector1, Vector vector2) =>
			Combine(vector1, vector2, (x1, x2) => x1 - x2);

		public static Vector operator *(Vector vector, double scalar) =>
			Convert(vector, x => x * scalar);

		public static Vector operator -(Vector item) => 
			Convert(item, x => -x);

		public static Vector Combine(Vector vector1, Vector vector2, 
			Func<double, double, double> converter) {
			if (vector1.Length != vector2.Length) {
				throw new ArithmeticException("Length of vectors must be equals");
			}
			var result = new Vector(vector1.Length);
			for (var i = 0; i < vector1.Length; i++) {
				result[i] = converter(vector1[i], vector2[i]);
			}
			return result;
		}

		public static Vector Convert(Vector vector,
			Func<double, double> converter) {
			var result = new Vector(vector.Length);
			for (var i = 0; i < vector.Length; i++) {
				result[i] = converter(vector[i]);
			}
			return result;
		}

		public static implicit operator Vector(double[] vector) {
			return new Vector(vector);
		}
	}
}
