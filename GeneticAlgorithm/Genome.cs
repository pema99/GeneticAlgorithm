using System;
using System.Linq;

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

        public Genome(params int[] layers)
        {
            if (layers.Length < 2)
            {
                throw new ArgumentOutOfRangeException("Network must have a minimum of 2 layers.");
            }

            int sumW = 0, sumB = 0;
            for (int i = 1; i < layers.Length; i++)
            {
                sumW += layers[i] * layers[i - 1];
                sumB += layers[i];
            }

            this.Weights = Util.Rand.NextDoubleArray(sumW);
            this.Biases = new double[sumB];
        }

        public (Genome childA, Genome childB) WholeArithmeticCrossover(Genome other, double bias = 0.5)
        {
            (Genome childA, Genome childB) result = (new Genome(new double[Weights.Length], new double[Biases.Length]), new Genome(new double[Weights.Length], new double[Biases.Length]));

            for (int i = 0; i < Weights.Length; i++)
            {
                result.childA.Weights[i] = bias * Weights[i] + (1.0 - bias) * other.Weights[i];
                result.childA.Weights[i] = bias * other.Weights[i] + (1.0 - bias) * Weights[i];
            }

            for (int i = 0; i < Biases.Length; i++)
            {
                result.childA.Biases[i] = bias * Biases[i] + (1.0 - bias) * other.Biases[i];
                result.childA.Biases[i] = bias * other.Biases[i] + (1.0 - bias) * Biases[i];
            }

            return result;
        }

        public (Genome childA, Genome childB) UniformCrossover(Genome other, double bias = 0.5)
        {
            (Genome childA, Genome childB) result = (new Genome(new double[Weights.Length], new double[Biases.Length]), new Genome(new double[Weights.Length], new double[Biases.Length]));

            for (int i = 0; i < Weights.Length; i++)
            {
                result.childA.Weights[i] = alpha * Weights[i] + (1.0 - alpha) * other.Weights[i];
                result.childA.Weights[i] = alpha * other.Weights[i] + (1.0 - alpha) * Weights[i];
            }

            for (int i = 0; i < Biases.Length; i++)
            {
                result.childA.Biases[i] = alpha * Biases[i] + (1.0 - alpha) * other.Biases[i];
                result.childA.Biases[i] = alpha * other.Biases[i] + (1.0 - alpha) * Biases[i];
            }

            return result;
        }

        public override string ToString()
        {
            string result = "Weights: { ";
            foreach (double weight in Weights)
            {
                result += string.Format("{0:N4}", weight) + "; ";
            }
            result += "}\n";
            result += "Biases: { ";
            foreach (double bias in Biases)
            {
                result += string.Format("{0:N4}", bias) + "; ";
            }
            result += "}";
            return result;
        }
    }
}
