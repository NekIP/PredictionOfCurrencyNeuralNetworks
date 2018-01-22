namespace NeuralNetwork {
	public abstract class Activation {
		public double ActivationCoefficient { get; set; }
		public abstract double Func(double x);
		public abstract double DeriveFunc(double x);
		public abstract double InverseFunc(double fx);
		public virtual Vector Func(Vector vector) {
			return Vector.Convert(vector, Func);
		}
		public virtual Vector DeriveFunc(Vector vector) {
			return Vector.Convert(vector, DeriveFunc);
		}
	}
}
