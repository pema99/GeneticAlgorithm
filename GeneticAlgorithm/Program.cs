using System;

namespace GeneticAlgorithm
{
    class Program
    {
        static void Main(string[] args)
        {
            Network n = new Network(2, 5, 1);

            var gen = n.ExtractGenome();
            Console.WriteLine(gen);

            n.InsertGenome(gen);
            Console.WriteLine(gen);

            Console.ReadKey();
        }
    }
}
