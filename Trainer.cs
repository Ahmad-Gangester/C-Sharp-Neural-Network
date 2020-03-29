using System.Collections.Generic;
using System.Text;

namespace NN
{
    public class Trainer
    {
        #region Constructer
        public Trainer(string In, string Out)
        {
            inputPath = In;
            outputPath = Out;
        }
        #endregion

        #region Variables
        private string inputPath;
        private string outputPath;
        private readonly StringBuilder Str = new StringBuilder();
        private List<int> topology = new List<int>();//topology of our Network
        private List<double> inputValues = new List<double>();
        private List<double> targetValues = new List<double>();
        private List<double> results = new List<double>();
        #endregion
        public void startTraning()
        {
            DataBase Data = new DataBase(inputPath, outputPath);
            Data.Read();
            topology = Data.Topology();
            Network net = new Network(topology);

            for (int i = 0; i < Data.NumberOfTestCases; i++)
            {
                Str.AppendLine()
                    .Append("Pass: ")
                    .Append(i + 1)
                    .AppendLine();
                inputValues.Clear();
                targetValues.Clear();
                inputValues = Data.nextInputs();
                targetValues = Data.nextTargets();
                Str.Append("Inputs: ");
                foreach (double inputs in inputValues) { Str.Append(inputs).Append(" "); }
                Str.AppendLine();
                net.feedForward(inputValues);
                results = net.getResults();
                Str.Append("Results: ");
                foreach (double r in results) { Str.Append(r).Append(" "); }
                Str.AppendLine();
                Str.Append("Targets: ");
                foreach (double target in targetValues) { Str.Append(target).Append(" "); }
                Str.AppendLine();
                Str.Append("Error Avg: ")
                   .Append(net.AverageError)
                   .AppendLine();
                net.backPropagation(targetValues);
            }
            Data.Write(Str);
        }
    }
}
