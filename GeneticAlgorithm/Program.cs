using System;
using System.Collections.Generic;
using System.Linq;

namespace GeneticAlgorithm
{
    class Program
    {
        static void Main(string[] args)
        {
            new Program().Start();
        }

        public void Start()
        {
            //Setup vars
            int popSize = 200;
            int survivors = 70;

            //Init network, population
            Network n = new Network(2, 3, 3, 1);

            List<(double fitness, Genome genome)> population = new List<(double, Genome)>();
            for (int i = 0; i < popSize; i++)
            {
                population.Add((0, new Genome(2, 3, 3, 1)));
            }

            //Fitness func 0-4
            Func<Network, double> fitness = (Network net) =>
            {
                double score = 0;

                score -= Util.MeanSquaredLoss(new double[] { 0 }, n.Eval(new double[] { 0, 0 })) * 5000;
                score -= Util.MeanSquaredLoss(new double[] { 1 }, n.Eval(new double[] { 0, 1 })) * 5000;
                score -= Util.MeanSquaredLoss(new double[] { 1 }, n.Eval(new double[] { 1, 0 })) * 5000;
                score -= Util.MeanSquaredLoss(new double[] { 0 }, n.Eval(new double[] { 1, 1 })) * 5000;

                return score;
            };

            //Start training
            int epoch = 0;
            double bestScore = double.MinValue;
            while (bestScore < -0.05)
            {
                for (int i = 0; i < population.Count; i++)
                {
                    //Get fitness of each
                    n.InsertGenome(population[i].genome);
                    population[i] = (fitness(n), population[i].genome);
                }

                //Order by fitness, kill off
                population = population.OrderByDescending(x => x.fitness).Take(survivors).ToList();
                bestScore = population[0].fitness;

                Console.WriteLine("Gen {0}, best fit {1}", epoch, bestScore);

                //Parent selection
                List<(double, Genome)> offspring = new List<(double, Genome)>();
                while (offspring.Count < popSize - survivors)
                {
                    Genome[] parents = new Genome[2];
                    do
                    {
                        //Tournament           
                        for (int i = 0; i < 2; i++)
                        {
                            parents[i] = 
                                population.OrderBy(x => Util.Rand.Next()).
                                Take(7)
                                .OrderByDescending(x => x.fitness)
                                .First().genome;
                        }
                    }
                    while (parents[0] == parents[1]);

                    //Breed and mutate
                    var children = parents[0].CrossoverUniform(parents[1]);

                    var childA = children.childA;
                    var childB = children.childB;

                    childA = Util.Rand.NextDouble() <= 0.1 
                        ? childA.MutationDelta(3, 0.3, 0.3) 
                        : childA;
                    offspring.Add((0, childA));

                    childB = Util.Rand.NextDouble() <= 0.1
                        ? childB.MutationDelta(3, 0.3, 0.3)
                        : childB;
                    offspring.Add((0, childB));
                }

                //Add offspring
                population.AddRange(offspring);

                epoch++;
            }

            TestXOR(n);

            Console.ReadKey();
        }

        public void TestXOR(Network n)
        {
            var test1 = n.Eval(new double[] { 0, 0 })[0];
            Console.WriteLine("Input: 0 0, Got: {0}, Expected: 0", test1);
            var test2 = n.Eval(new double[] { 0, 1 })[0];
            Console.WriteLine("Input: 0 1, Got: {0}, Expected: 1", test2);
            var test3 = n.Eval(new double[] { 1, 0 })[0];
            Console.WriteLine("Input: 1 0, Got: {0}, Expected: 1", test3);
            var test4 = n.Eval(new double[] { 1, 1 })[0];
            Console.WriteLine("Input: 1 1, Got: {0}, Expected: 0", test4);
        }
    }
}
