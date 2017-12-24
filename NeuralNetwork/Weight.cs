namespace NeuralNetwork
{
    public class Weight
    {
        /// <summary>
        /// Weight
        /// </summary>
        public double W { get; set; }

        /// <summary>
        /// Delta needed for learning
        /// </summary>
        public double D { get; set; }

        public override string ToString()
        {
            return W.ToString();
        }
    }
}
