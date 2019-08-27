using System;

namespace GeneticAlgorithm
{
    public class Layer
    {
        public Neuron[] Neurons { get; set; }

        public Layer(int neurons, int prevLayerNeurons)
        {
            this.Neurons = new Neuron[neurons];
            for (int i = 0; i < neurons; i++)
            {
                this.Neurons[i] = new Neuron(Util.Rand.NextDoubleArray(prevLayerNeurons));
            }
        }

        public Layer(params Neuron[] neurons)
        {
            this.Neurons = neurons;
        }
    }
}
