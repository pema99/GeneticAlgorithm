using System;
using System.Collections.Generic;

namespace GeneticAlgorithm
{
    public static class Util
    {
        public static Random Rand = new Random();

        public static double[] NextDoubleArray(this Random rand, int length)
        {
            List<double> result = new List<double>();
            for (int i = 0; i < length; i++)
            {
                result.Add(rand.NextDouble());
            }
            return result.ToArray();
        }

        public static double Sigmoid(double input)
        {
            return 1f / (1f + Math.Pow(Math.E, -input));
        }

        public static double SigmoidDerivative(double input)
        {
            double f = Sigmoid(input);
            return f * (1 - f);
        }

        public static double MeanSquaredLoss(double[] expected, double[] actual)
        {
            double result = 0;
            for (int i = 0; i < expected.Length; i++)
            {
                result += Math.Pow(expected[i] - actual[i], 2);
            }
            result /= expected.Length;
            return result;
        }
    }
}
