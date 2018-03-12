using DataAssistants.Structs;
using System;

namespace NeuralNetwork {
	public abstract class NeuralNetwork {
		/// <summary>
		/// Number of iterations of neural network training
		/// </summary>
		public int Epoch { get; private set; }

		public Activation Activation { get; protected set; }

        protected string DefaultPath => "NeuralNetworks/";

		public abstract NeuralNetworkResult Run(NeuralNetworkData inputData);

		public abstract NeuralNetworkLearnResult Learn(NeuralNetworkData inputData, NeuralNetworkData idealData);

		public abstract NeuralNetworkLearnResult Learn(NeuralNetworkData inputData);

        public abstract void Load(string nameOfNeuralNetwork);

        public abstract void Save(string nameOfNeuralNetwork);
    
        /// <summary>
        /// Converts the output values of a neural network using the inverse activation function
        /// </summary>
        public virtual Vector ConvertOutput(Vector output) => Vector.Convert(output, Activation.InverseFunc);

		protected void SetNextEpoch() => Epoch++;

		protected void CheckConditionOnException<TException>(bool conditionFunc, string message) 
			where TException: Exception, new() {
			if (conditionFunc) {
				throw new NeuralNetworkException(message, new TException());
			}
		}

		protected void CheckConditionOnException(bool conditionFunc, string message) {
			if (conditionFunc) {
				throw new NeuralNetworkException(message);
			}
		}
	}
}
