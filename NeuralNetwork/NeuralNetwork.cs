namespace NeuralNetwork
{
    public abstract class NeuralNetwork
    {
        /// <summary>
        /// Run a neural network for input values
        /// </summary>
        public abstract double[] Run(double[] input);

        public abstract NeuralNetworkLearnResult Learn(double[] input, double[] ideal);
    }
}
