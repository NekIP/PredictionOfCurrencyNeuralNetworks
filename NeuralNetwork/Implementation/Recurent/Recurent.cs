using DataAssistants.Structs;
using System.Linq;

namespace NeuralNetwork {
	public abstract class Recurent : NeuralNetwork {
		protected Vector[] ConvertDataToSequence(NeuralNetworkData data) {
			CheckConditionOnException(data.DataType != NeuralNetworkDataType.SequenceOfVectors, "Recurrent neural network implements the 'sequence to sequence' approach, " +
				"so the input shape must contain only a sequence of vectors. For example: " +
				"nt.Run(new NeuralNetworkData(new Vector[] { new [] { 0.0, 0.3 }, new [] { 0.4, 0.7 } }))");
			return data.Shape.Values.First();
		}
	}
}
