using System.Linq;

namespace NeuralNetwork {
	public class Shape {
		public Vector[][] Values { get; set; }

		public Shape(Vector value) {
			Values = new Vector[][] { new Vector[] { value } };
		}

		public Shape(Vector[] value) {
			Values = new Vector[][] { value };
		}

		public Shape(Vector[][] value) {
			Values = value;
		}

		public Shape(Matrix matrix) {
			Values = new Vector[][] { matrix };
		}

		public override string ToString() {
			var result = "[";
			for (var i = 0; i < Values.Length; i++) {
				result += "[";
				for (var j = 0; j < Values[i].Length - 1; j++) {
					result += Values[i][j] + ", ";
				}
				result += Values[i].Last() + "]";
				if (i != Values.Length - 1) {
					result += ", ";
				}
			}
			return result;
		}

		public Vector[] this[int i] {
			get {
				return Values[i];
			}
			set {
				Values[i] = value;
			}
		}

		public Vector this[int i, int j] {
			get {
				return Values[i][j];
			}
			set {
				Values[i][j] = value;
			}
		}

		public double this[int i, int j, int k] {
			get {
				return Values[i][j][k];
			}
			set {
				Values[i][j][k] = value;
			}
		}
	}
}
