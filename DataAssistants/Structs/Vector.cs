﻿using Newtonsoft.Json;
using System;
using System.Linq;

namespace DataAssistants.Structs {
    public class Vector {
        [JsonIgnore]
		public int Length => Values.Length;
        [JsonProperty]
        public double[] Values { get; private set; }

		public Vector(double[] values) {
			Values = values;
		}

		public Vector(int length, Func<double> initializer = null) {
			Values = new double[length];
			if (!(initializer is null)) {
				for (var i = 0; i < Values.Length; i++) {
					Values[i] = initializer();
				}
			}
		}

		protected Vector() { }

		public double this[int index] {
			get {
				return Values[index];
			}
			set {
				Values[index] = value;
			}
		}

		public Vector Copy() {
			var result = new double[Length];
			for (var i = 0; i < result.Length; i++) {
				result[i] = Values[i];
			}
			return result;
		}

		public Vector Skip(int bias) => bias > 0 
			? new Vector(Values.Skip(bias).ToArray())
			: new Vector(Values.SkipLast(-bias).ToArray());

		public Vector Take(int length) => length > 0
			? new Vector(Values.Take(length).ToArray())
			: new Vector(Values.TakeLast(-length).ToArray());

		public override string ToString() {
			var result = "[";
			for (var i = 0; i < Values.Length - 1; i++) {
				result += Values[i] + ", ";
			}
			return result + Values.Last() + "]";
		}

		public static Vector operator +(Vector vector1, Vector vector2) =>
			Combine(vector1, vector2, (x1, x2) => x1 + x2);

		public static Vector operator -(Vector vector1, Vector vector2) =>
			Combine(vector1, vector2, (x1, x2) => x1 - x2);

		public static Vector operator *(Vector vector1, Vector vector2) =>
			Combine(vector1, vector2, (x1, x2) => x1 * x2);

		public static Vector operator /(Vector vector1, Vector vector2) =>
			Combine(vector1, vector2, (x1, x2) => x1 / x2);

        public static Vector operator /(Vector vector, double scalar) =>
            Convert(vector, x => x / scalar);

        public static Vector operator +(Vector vector, double scalar) =>
            Convert(vector, x => x + scalar);

        public static Vector operator +(double scalar, Vector vector) =>
            vector + scalar;

        public static Vector operator -(Vector vector, double scalar) =>
            Convert(vector, x => x - scalar);

        public static Vector operator -(double scalar, Vector vector) =>
            Convert(vector, x => scalar - x);

        public static Vector operator *(Vector vector, double scalar) =>
			Convert(vector, x => x * scalar);

		public static Vector operator *(double scalar, Vector vector) => 
			vector * scalar;

		public static Vector operator ^(Vector vector, double scalar) =>
			Convert(vector, x => Math.Pow(x, scalar));

		public static Vector operator -(Vector item) => 
			Convert(item, x => -x);

		public static Vector Union(Vector vector1, Vector vector2) {
			var resultLength = vector1.Length + vector2.Length;
			var result = new Vector(resultLength);
			for (var i = 0; i < resultLength; i++) {
				result[i] = i < vector1.Length 
					? vector1[i] 
					: vector2[i - vector1.Length];
			}
			return result;
		}

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

		public static Vector CreateLikeA(Vector vector, Func<double> initializer = null) => 
			new Vector(vector.Length, initializer);

		public static implicit operator Vector(double[] vector) => new Vector(vector);

		public static implicit operator double[](Vector vector) => vector.Values;
	}
}
