using System;
using System.Collections.Generic;
using System.Linq;

namespace GeneticAlgorithm
{
    public class Network
    {
        public Layer[] Layers { get; set; }

        public Network(params int[] layers)
        {
            if (layers.Length < 2)
            {
                throw new ArgumentOutOfRangeException("Network must have a minimum of 2 layers.");
            }

            Layers = new Layer[layers.Length];
            for (int i = 0; i < layers.Length; i++)
            {
                if (i == 0)
                {
                    Layers[i] = new Layer(layers[i], 0);
                }
                else
                {
                    Layers[i] = new Layer(layers[i], layers[i - 1]);
                }
            }
        }

        public Network(params Layer[] layers)
        {
            this.Layers = layers;
        }

        public double[] Eval(double[] inputs)
        {
            if (inputs.Length != Layers[0].Neurons.Length)
            {
                return null;
            }

            for (int i = 0; i < Layers.Length; i++)
            {
                for (int j = 0; j < Layers[i].Neurons.Length; j++)
                {
                    Neuron n = Layers[i].Neurons[j];
                    
                    //Set input layer
                    if (i == 0)
                    {
                        n.Value = inputs[j];
                    }
                    //Calculate neuron values
                    else
                    {
                        n.Value = 0;
                        for (int k = 0; k < Layers[i - 1].Neurons.Length; k++)
                        {
                            n.Value += Layers[i - 1].Neurons[k].Value * n.Weights[k];
                        }
                        n.Value = Util.Sigmoid(n.Value + n.Bias);
                    }
                }
            }

            List<double> results = new List<double>();
            for (int i = 0; i < Layers.Last().Neurons.Length; i++)
            {
                results.Add(Layers.Last().Neurons[i].Value);
            }
            return results.ToArray();
        }

        public Genome ExtractGenome()
        {
            List<double> weights = new List<double>();
            List<double> biases = new List<double>();

            foreach (Layer l in Layers)
            {
                foreach (Neuron n in l.Neurons)
                {
                    foreach (double weight in n.Weights)
                    {
                        weights.Add(weight);
                    }
                }
                foreach (Neuron n in l.Neurons)
                {
                    biases.Add(n.Bias);
                }
            }

            return new Genome(weights.ToArray(), biases.ToArray());
        }

        public void InsertGenome(Genome genome)
        {
            int ptrW = 0, ptrB = 0;
            foreach (Layer l in Layers)
            {
                //Start of genome is weights
                foreach (Neuron n in l.Neurons)
                {
                    for (int i = 0; i < n.Weights.Length; i++)
                    {
                        n.Weights[i] = genome.Weights[ptrW++];
                    }
                }
                //End is biases
                foreach (Neuron n in l.Neurons)
                {
                    n.Bias = genome.Biases[ptrB++];
                }
            }
        }
    }
}
