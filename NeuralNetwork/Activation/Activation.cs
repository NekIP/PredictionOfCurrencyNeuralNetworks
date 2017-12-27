namespace NeuralNetwork {
	public interface Activation {
		double ActivationCoefficient { get; set; }
		double Func(double x);
		double DeriveFunc(double x);
		double InverseFunc(double fx);
	}
}
