using System;
using System.IO;


namespace NN
{

    class Program
    {

        static void Main(string[] args)
        {
            string inPath = Path.Join(AppDomain.CurrentDomain.BaseDirectory, "Data", "inputs.txt");
            string outPath = Path.Join(AppDomain.CurrentDomain.BaseDirectory, "Data", "outputs.txt");
            Trainer Xor = new Trainer(inPath, outPath);
            Xor.startTraning();

        }
    }
}
