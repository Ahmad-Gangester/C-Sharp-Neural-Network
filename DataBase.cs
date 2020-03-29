using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace NN
{
    public class DataBase
    {
        #region Constructer
        public DataBase(string i, string o)
        {
            inputFilePath = i;
            outputFilePath = o;
        }
        #endregion

        #region Variables
        private string inputFilePath { get; set; }
        private string outputFilePath { get; set; }
        private List<string> lines { get; set; }
        private int numberOfTestCases { get; set; }
        #endregion

        #region Functions

        public void Read()
        {
            if (new FileInfo(inputFilePath).Length == 0)
            {
                throw new InvalidOperationException("input file is Empty.");
            }
            lines = File.ReadAllLines(inputFilePath).ToList();
        }

        public List<int> Topology()
        {
            if (!lines.First().Contains("Topology"))
            {
                throw new InvalidDataException("Data Format is Wrong");
            }
            List<int> t = new List<int>();
            string[] seperators = { "Topology:", " " };
            string[] values = lines.First().Split(seperators, StringSplitOptions.RemoveEmptyEntries);
            foreach (var v in values)
            {
                t.Add(int.Parse(v));
            }
            numberOfTestCases = t.Last();
            t.RemoveAt(t.Count - 1);
            lines.RemoveAt(0);
            return t;
        }

        public List<double> nextInputs()
        {
            if (!lines.First().Contains("In"))
            {
                throw new InvalidDataException("Data Format is Wrong");
            }
            List<double> inputs = new List<double>();
            string[] seperators = { "In:", " " };
            string[] values = lines.First().Split(seperators, StringSplitOptions.RemoveEmptyEntries);
            foreach (var v in values)
            {
                inputs.Add(double.Parse(v));
            }
            lines.RemoveAt(0);
            return inputs;
        }

        public List<double> nextTargets()
        {
            if (!lines.First().Contains("Target"))
            {
                throw new InvalidDataException("Data Format is Wrong");
            }
            List<double> target = new List<double>();
            string[] seperators = { "Target:", " " };
            string[] values = lines.First().Split(seperators, StringSplitOptions.RemoveEmptyEntries);
            foreach (var v in values)
            {
                target.Add(double.Parse(v));
            }
            lines.RemoveAt(0);
            return target;
        }

        public void Create(List<int> Topology, int testCaseNumbers)
        {
            StringBuilder str = new StringBuilder();
            Random random = new Random();
            str.Append("Topology: ");
            foreach (int t in Topology)
            {
                str.Append(t).Append(" ");
            }
            str.Append(testCaseNumbers);
            str.AppendLine();
            for (int i = 0; i < testCaseNumbers; i++)
            {
                int t = 0;
                for (int j = 0; j < Topology[0]; j++)
                {
                    int n = random.Next(2);
                    t = t ^ n;
                    str.Append("In: ")
                        .Append(n)
                        .Append(".0 ");
                }
                str.AppendLine();
                str.Append("Target: ")
                    .Append(t)
                    .Append(".0 ")
                    .AppendLine();
            }
            File.WriteAllText(inputFilePath, str.ToString());
        }

        public void Write(StringBuilder str)
        {
            File.WriteAllText(outputFilePath, str.ToString());
        }


        #endregion

        #region Gets & Sets
        public int NumberOfTestCases { get => numberOfTestCases; private set {; } }
        #endregion

    }
}
