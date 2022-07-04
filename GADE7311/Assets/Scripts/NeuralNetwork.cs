using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;

public class NeuralNetwork : MonoBehaviour
{

    //variables for the neural network
    private int[] layers;
    private float[][] neurons;
    private float[][] biases;
    private float[][][] weights;
    private int[] activations;

    public float fitness = 0;

    public NeuralNetwork(int[] layers)
    {
        this.layers = new int[layers.Length];
        for (int i = 0; i < layers.Length; i++)
        {
            this.layers[i] = layers[i];
        }
        InitNeurons();
        InitBiases();
        InitWeights();
    }

    /// <summary>
    /// create empty storage array for the neurons in the network.
    /// </summary>
    private void InitNeurons()
    {
        List<float[]> neuronsList = new List<float[]>();
        for (int i = 0; i < layers.Length; i++)
        {
            neuronsList.Add(new float[layers[i]]);
        }
        neurons = neuronsList.ToArray();
    }

    /// <summary>
    /// initializes and populates array for the biases being held within the network.
    /// </summary>
    private void InitBiases()
    {
        List<float[]> biasList = new List<float[]>();
        for (int i = 0; i < layers.Length; i++)
        {
            float[] bias = new float[layers[i]];
            for (int j = 0; j < layers[i]; j++)
            {
                bias[j] = UnityEngine.Random.Range(-0.5f, 0.5f);
            }
            biasList.Add(bias);
        }
        biases = biasList.ToArray();
    }

    /// <summary>
    /// initializes random array for the weights being held in the network.
    /// </summary>
    private void InitWeights()
    {
        List<float[][]> weightsList = new List<float[][]>();
        for (int i = 0; i < layers.Length; i++)
        {
            List<float[]> layerWeightsList = new List<float[]>();
            int neuronsInPreviousLayer = layers[i - 1];
            for (int j = 0; j < neurons[i].Length; j++)
            {
                float[] neuronsWeights = new float[neuronsInPreviousLayer];
                for (int k = 0; k < neuronsInPreviousLayer; k++)
                {
                    neuronsWeights[k] = UnityEngine.Random.Range(-0.5f, 0.5f);
                }
                layerWeightsList.Add(neuronsWeights);
            }
            weightsList.Add(layerWeightsList.ToArray());
        }
        weights = weightsList.ToArray();
    }

    public float activate(float value)
    {
        return (float)Math.Tanh(value);
    }

    /// <summary>
    /// feed forward, inputs >==> outputs.
    /// </summary>
    /// <param name="inputs"></param>
    /// <returns></returns>
    public float[] FeedForward(float[] inputs)
    {
        for (int i = 0; i < inputs.Length; i++)
        {
            neurons[0][i] = inputs[i];
        }
        for (int i = 1; i < layers.Length; i++)
        {
            int layer = i - 1;
            for (int j = 0; j < neurons[i].Length; j++)
            {
                float value = 0f;
                for (int k = 0; k < neurons[i - 1].Length; k++)
                {
                    value += weights[i - 1][j][k] * neurons[i - 1][k];
                }
                neurons[i][layer] = activate(value + biases[i][j]);
            }
        }
        return neurons[neurons.Length - 1];
    }

    /// <summary>
    /// Comparing For NeuralNetworks performance
    /// </summary>
    /// <param name="other"></param>
    /// <returns></returns>
    public int CompareTo(NeuralNetwork other)
    {
        if (other == null)
            return 1;
        if (fitness > other.fitness)
            return 1;
        else if (fitness < other.fitness)
            return -1;
        else
            return 0;
    }

    /// <summary>
    /// this loads the biases and weights from within a file into the neural network.
    /// </summary>
    /// <param name="path"></param>
    public void Load(string path)
    {
        TextReader tr = new StreamReader(path);
        int NumberOfLines = (int)new FileInfo(path).Length;
        string[] ListLines = new string[NumberOfLines];
        int index = 1;
        for (int i = 1; i < NumberOfLines; i++)
        {
            ListLines[i] = tr.ReadLine();
        }
        tr.Close();
        if (new FileInfo(path).Length > 0)
        {
            for (int i = 0; i < biases.Length; i++)
            {
                for (int j = 0; j < biases[i].Length; j++)
                {
                    biases[i][j] = float.Parse(ListLines[index]);
                    index++;
                }
            }
            for (int i = 0; i < weights.Length; i++)
            {
                for (int j = 0; j < weights[i].Length; j++)
                {
                    for (int k = 0; k < weights[i][j].Length; k++)
                    {
                        weights[i][j][k] = float.Parse(ListLines[index]);
                        index++;
                    }
                }
            }
        }
    }

    public void Mutate(int chance, float val)
    {
        for (int i = 0; i < biases.Length; i++)
        {
            for (int j = 0; j < biases[i].Length; j++)
            {
                biases[i][j] = (UnityEngine.Random.Range(0f, chance) <= 5) ?
                    biases[i][j] += UnityEngine.Random.Range(-val, val) : biases[i][j];
            }
        }

        for (int i = 0; i < weights.Length; i++)
        {
            for (int j = 0; j < weights[i].Length; j++)
            {
                for (int k = 0; k < weights[i][j].Length; k++)
                {
                    weights[i][j][k] = (UnityEngine.Random.Range(0f, chance) <= 5)
                        ? weights[i][j][k] += UnityEngine.Random.Range(-val, val) :
                        weights[i][j][k];
                }
            }
        }
    }


}
