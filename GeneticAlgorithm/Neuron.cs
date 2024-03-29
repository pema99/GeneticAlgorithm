﻿using System;

namespace GeneticAlgorithm
{
    public class Neuron
    {
        public double[] Weights { get; set; }
        public double Bias { get; set; }
        public double Value { get; set; }

        public Neuron()
        {
            this.Weights = new double[] { };
            this.Bias = 0;
        }

        public Neuron(double[] weights, double bias = 0)
        {
            this.Weights = weights;
            this.Bias = bias;
        }
    }
}
