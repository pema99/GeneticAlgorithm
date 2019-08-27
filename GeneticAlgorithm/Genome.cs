using System;

namespace GeneticAlgorithm
{
    public class Genome
    {
        public double[] Weights { get; set; }
        public double[] Biases { get; set; }

        public Genome(double[] weights, double[] biases)
        {
            this.Weights = weights;
            this.Biases = biases;
        }

        public override string ToString()
        {
            string result = "Weights: { ";
            foreach (double weight in Weights)
            {
                result += string.Format("{0:N4}", weight) + ", ";
            }
            result += "}\n";
            result += "Biases: { ";
            foreach (double bias in Biases)
            {
                result += string.Format("{0:N4}", bias) + ", ";
            }
            result += "}";
            return result;
        }
    }
}
