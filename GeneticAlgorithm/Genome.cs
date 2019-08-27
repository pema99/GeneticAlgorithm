using System;
using System.Linq;

namespace GeneticAlgorithm
{
    public class Genome
    {
        private double[][] values { get; set; }

        public double[] Weights
        {
            get { return values[0]; }
            set { values[0] = value; }
        }

        public double[] Biases
        {
            get { return values[1]; }
            set { values[1] = value; }
        }

        public Genome(double[] weights, double[] biases)
        {
            this.values = new double[2][];
            this.Weights = weights;
            this.Biases = biases;
        }

        public Genome(Genome copy)
        {
            this.values = new double[2][];
            this.Weights = copy.Weights;
            this.Biases = copy.Biases;
        }

        public Genome(params int[] layers)
        {
            if (layers.Length < 2)
            {
                throw new ArgumentOutOfRangeException("Network must have a minimum of 2 layers.");
            }

            this.values = new double[2][];

            int sumW = 0, sumB = 0;
            for (int i = 1; i < layers.Length; i++)
            {
                sumW += layers[i] * layers[i - 1];
                sumB += layers[i];
            }

            this.Weights = Util.Rand.NextDoubleArray(sumW);
            this.Biases = new double[sumB];
        }

        //TODO: refactor and generalize crossover and mutation methods
        #region Crossover operators
        public (Genome childA, Genome childB) CrossoverWholeArithmetic(Genome other, double bias = 0.5)
        {
            (Genome childA, Genome childB) result = 
                (new Genome(new double[Weights.Length], new double[Biases.Length]), new Genome(new double[Weights.Length], new double[Biases.Length]));

            for (int i = 0; i < 2; i++)
            {
                for (int j = 0; j < values[i].Length; j++)
                {
                    result.childA.values[i][j] = bias * values[i][j] + (1.0 - bias) * other.values[i][j];
                    result.childB.values[i][j] = bias * other.values[i][j] + (1.0 - bias) * values[i][j];
                }
            }

            return result;
        }

        public (Genome childA, Genome childB) CrossoverUniform(Genome other, double bias = 0.5)
        {
            (Genome childA, Genome childB) result = 
                (new Genome(new double[Weights.Length], new double[Biases.Length]), new Genome(new double[Weights.Length], new double[Biases.Length]));

            for (int i = 0; i < 2; i++)
            {
                for (int j = 0; j < values[i].Length; j++)
                {
                    double choice = Util.Rand.NextDouble();
                    result.childA.values[i][j] = choice >= bias ? other.values[i][j] : values[i][j];
                    result.childB.values[i][j] = choice >= bias ? values[i][j] : other.values[i][j];
                }
            }

            return result;
        }

        public (Genome childA, Genome childB) CrossoverSinglePoint(Genome other)
        {
            (Genome childA, Genome childB) result = 
                (new Genome(new double[Weights.Length], new double[Biases.Length]), new Genome(new double[Weights.Length], new double[Biases.Length]));

            double ratio = Util.Rand.NextDouble();

            for (int i = 0; i < 2; i++)
            {
                for (int j = 0; j < values[i].Length; j++)
                {
                    if (j >= (int)(ratio * values[i].Length))
                    {
                        result.childA.values[i][j] = values[i][j];
                    }
                    else
                    {
                        result.childB.values[i][j] = other.values[i][j];
                    }
                }
            }

            return result;
        }
        #endregion

        #region Mutation operators
        public Genome MutationInversion(int numMuts)
        {
            Genome result = new Genome(this);

            for (int i = 0; i < numMuts; i++)
            {
                //Mutate either weight or bias based on heuristic
                int target = Util.Rand.NextDouble() <= Weights.Length / (Weights.Length + Biases.Length) ? 0 : 1;

                int start = Util.Rand.Next(values[target].Length);
                int end = Util.Rand.Next(values[target].Length);
                    
                if (end < start)
                {
                    int temp = start;
                    start = end;
                    end = temp;
                }

                result.values[target] = 
                    result.values[target].Take(start) //First segment
                    .Concat(result.values[target].Skip(start).Take(end - start).Reverse()) //Reversed middle section
                    .Concat(result.values[target].Skip(end)) //Last segment
                    .ToArray();        
            }

            return result;
        }

        public Genome MutationScramble(int numMuts)
        {
            Genome result = new Genome(this);

            for (int i = 0; i < numMuts; i++)
            {
                //Mutate either weight or bias based on heuristic
                int target = Util.Rand.NextDouble() <= Weights.Length / (Weights.Length + Biases.Length) ? 0 : 1;

                int start = Util.Rand.Next(values[target].Length);
                int end = Util.Rand.Next(values[target].Length);

                if (end < start)
                {
                    int temp = start;
                    start = end;
                    end = temp;
                }

                result.values[target] =
                    result.values[target].Take(start) //First segment
                    .Concat(result.values[target].Skip(start).Take(end - start).OrderBy(x => Util.Rand.Next())) //Scrambled middle section
                    .Concat(result.values[target].Skip(end)) //Last segment
                    .ToArray();
            }

            return result;
        }

        public Genome MutationSwap(int maxMuts, double mutChance)
        {
            Genome result = new Genome(this);

            for (int i = 0; i < Util.Rand.Next(maxMuts); i++)
            {
                //Mutate either weight or bias based on heuristic
                if (Util.Rand.NextDouble() <= mutChance)
                {
                    int target = Util.Rand.NextDouble() <= Weights.Length / (Weights.Length + Biases.Length) ? 0 : 1;

                    int indexA = Util.Rand.Next(values[target].Length);
                    int indexB = Util.Rand.Next(values[target].Length);

                    double temp = result.values[target][indexA];
                    result.values[target][indexA] = result.values[target][indexB];
                    result.values[target][indexB] = temp;
                }
            }

            return result;
        }

        public Genome MutationDelta(int maxMuts, double mutChance, double maxWeightDelta, double maxBiasDelta)
        {
            Genome result = new Genome(this);

            for (int i = 0; i < Util.Rand.Next(maxMuts); i++)
            {
                if (Util.Rand.NextDouble() <= mutChance)
                {
                    //Mutate either weight or bias based on heuristic
                    if (Util.Rand.NextDouble() <= Weights.Length / (Weights.Length + Biases.Length))
                    {
                        result.Weights[Util.Rand.Next(Weights.Length)] += maxWeightDelta * ((Util.Rand.NextDouble() - 0.5) * 2);
                    }
                    else
                    {
                        result.Biases[Util.Rand.Next(Weights.Length)] += maxBiasDelta * ((Util.Rand.NextDouble() - 0.5) * 2);
                    }
                }
            }

            return result;
        }
        #endregion

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
