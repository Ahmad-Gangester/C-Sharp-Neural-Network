using System;
using System.Collections.Generic;
using System.Linq;

namespace NN
{
    using Layer = List<Neuron>;
    class Network
    {
        #region Constructer
        /// <summary>
        /// Creats a new Network based on a certain topology
        /// </summary>
        /// <param name="topology">the number of layers and how many Neuron in each layer</param>
        public Network(List<int> topology)// 2,4,1
        {
            int numLayers = topology.Count;
            for (int layerNum = 0; layerNum < numLayers; layerNum++)
            {
                Layers.Add(new Layer());
                int numOutputs = layerNum == topology.Count - 1 ? 0 : topology[layerNum + 1];

                for (int neuronNum = 0; neuronNum < topology[layerNum]; neuronNum++)
                {
                    Layers.Last().Add(new Neuron(numOutputs, neuronNum));
                }
            }
        }
        #endregion

        #region Variables
        private List<Layer> Layers = new List<Layer>();
        private double Error;
        private double averageError;
        private static double numberToAverage = 100;
        #endregion

        #region Public Functions
        /// <summary>
        /// Loops to the end of the network to get results.
        /// </summary>
        /// <param name="inputValues">the first layer inputs</param>
        public void feedForward(List<double> inputValues)
        {
            //check for error
            if (inputValues.Count != Layers[0].Count)
            {
                throw new InvalidOperationException("The number of inputs doesn't macth the number of neurons in first layer");
            }
            //-------------------------------

            for (int i = 0; i < inputValues.Count; i++)
            {
                Layers[0][i].OutPutvalue = inputValues[i];
            }

            for (int i = 1; i < Layers.Count; i++)
            {
                Layer perviousLayer = Layers[i - 1];
                for (int j = 0; j < Layers[i].Count; j++)
                {
                    Layers[i][j].feedForward(perviousLayer);
                }
            }
        }
        public void backPropagation(List<double> targetValues)
        {
            Layer outputLayer = Layers.Last();
            #region Error Calc
            //calculye the average error of the last 100 traning cases for data perposes (doesn't affect the program at all)
            Error = 0.0;
            for (int i = 0; i < outputLayer.Count; i++)
            {
                double delta = targetValues[i] - outputLayer[i].OutPutvalue;
                Error = delta * delta;
            }
            Error /= outputLayer.Count;
            Error = Math.Sqrt(Error);

            averageError = (averageError * numberToAverage + Error) / (numberToAverage + 1.0);
            //----------------------------------------------------------------------------------------------------------------
            #endregion

            for (int i = 0; i < outputLayer.Count; i++)
            {
                outputLayer[i].calculateOutputGradients(targetValues[i]);
            }

            for (int layerNumber = Layers.Count - 2; layerNumber > 0; layerNumber--)
            {
                Layer hiddenLayer = Layers[layerNumber];
                Layer nextLayer = Layers[layerNumber + 1];

                foreach (Neuron neuron in hiddenLayer)
                {
                    neuron.calculateHiddenGradients(nextLayer);
                }
            }

            for (int layerNumber = Layers.Count - 1; layerNumber > 0; layerNumber--)
            {
                Layer layer = Layers[layerNumber];
                Layer perviousLayer = Layers[layerNumber - 1];
                foreach (Neuron neuron in layer)
                {
                    neuron.updateWeights(perviousLayer);
                }

            }
        }
        public List<double> getResults()
        {
            List<double> results = new List<double>();
            foreach (Neuron neuron in Layers.Last())
            {
                results.Add(neuron.OutPutvalue);
            }
            return results;
        }
        #endregion

        #region Gets & Sets
        public double AverageError { get => averageError; private set {; } }
        #endregion
    }

}
