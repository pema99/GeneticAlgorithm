using System;

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
            Network n = new Network(2, 5, 1);

            TestXOR(n);

            Func<Network, double> fitness = (Network net) =>
            {
                double score = 0;

                score += Math.Abs(1.0 / n.Eval(new double[] { 0, 0 })[0]);
                score += Math.Abs(1.0 / (n.Eval(new double[] { 0, 1 })[0] - 1));
                score += Math.Abs(1.0 / (n.Eval(new double[] { 1, 0 })[0] - 1));
                score += Math.Abs(1.0 / n.Eval(new double[] { 0, 0 })[0]);

                return score;
            };

            Console.WriteLine(fitness(n));//new Genome(2, 5, 1));

            //Console.WriteLine(n.ExtractGenome());

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
