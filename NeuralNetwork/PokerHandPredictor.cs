using DeckOfCards;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

// This is an enhanced neural network. It is fully-connected
// and feed-forward. The training algorithm is back-propagation
// with momentum and weight decay. The input data is normalized
// so training is quite fast.

namespace NeuralNetwork
{
    public class PokerHandNN
    {
        private static readonly Random random = new Random();

        private static double RandomNumberBetween(double minValue, double maxValue)
        {
            var next = random.NextDouble();

            return minValue + (next * (maxValue - minValue));
        }
        public static void Start()
        {
            PokerHand pokerHand = new PokerHand();
            var cards = pokerHand.CardsInHand;


            string rootfolder = @"D:\";
            string dir = @"NeuralNetworkDatasets\pokerhand-set";
            string filename = "poker-hand-training-true.data";
            string path = Path.Combine(rootfolder, dir, filename);

            List<string> lines = new List<string>();
            if (File.Exists(path))
            {
                // Open the file to read from.
                lines = File.ReadAllLines(path).ToList();
            }

            IDictionary<int, double[]> dataDict = new Dictionary<int, double[]>();



            for (int i = 0; i < lines.Count; i++)
            {
                List<string> temp = lines[i].Split(',').ToList();
                double[] d = new double[20];
                for (int j = 0; j < temp.Count; j++)
                {
                    double x = Convert.ToDouble(temp[j]);
                    if (j == temp.Count - 1)
                    {
                        double[] tx = new double[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
                        //j = 9
                        tx.SetValue(1, (int)x);
                        Array.Copy(tx, 0, d, d.Length - tx.Length, tx.Length);
                    }
                    else
                    {
                        d[j] = x;
                    }
                }

                dataDict.Add(i, d);
                //Console.WriteLine(line);
            }

            double[][] data = new double[dataDict.Count][];
            foreach (var item in dataDict)
            {
                data[item.Key] = item.Value;
            }

            Console.WriteLine("\nFirst 6 rows of entire 150-item data set:");
            ShowMatrix(data, 6, 1, true);

            Console.WriteLine("Creating 80% training and 20% test data matrices");
            double[][] trainData = null;
            double[][] testData = null;
            MakeTrainTest(data, out trainData, out testData);

            Console.WriteLine("\nFirst 5 rows of training data:");
            ShowMatrix(trainData, 5, 1, true);
            Console.WriteLine("First 3 rows of test data:");
            ShowMatrix(testData, 3, 1, true);

            Normalize(trainData, new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 });
            Normalize(testData, new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 });

            Console.WriteLine("\nFirst 5 rows of normalized training data:");
            ShowMatrix(trainData, 5, 1, true);
            Console.WriteLine("First 3 rows of normalized test data:");
            ShowMatrix(testData, 3, 1, true);


            const int numInput = 10;
            const int numHidden = 5;
            const int numOutput = 10;

            Console.WriteLine($"\nCreating a {numInput}-input, {numHidden}-hidden, {numOutput}-output neural network");
            Console.Write("Hard-coded tanh function for input-to-hidden and softmax for ");
            Console.WriteLine("hidden-to-output activations");

            NeuralNetwork nn = new NeuralNetwork(numInput, numHidden, numOutput);

            Console.WriteLine("\nInitializing weights and bias to small random values");
            nn.InitializeWeights();

            int maxEpochs = 2000;
            double learnRate = 0.05;
            double momentum = 0.01;
            double weightDecay = 0.0001;
            double meanSquaredErrorStoppingCondition = 0.020;
            //double learnRate = RandomNumberBetween(0.01,0.12);
            //double momentum = RandomNumberBetween(0.01, 0.5);
            //double weightDecay = RandomNumberBetween(0.0001, 0.0005);
            //double meanSquaredErrorStoppingCondition = RandomNumberBetween(0.01, 0.05);
            Console.WriteLine($"Setting maxEpochs = {maxEpochs}, learnRate = {learnRate}, momentum = {momentum}, weightDecay = {weightDecay}");
            Console.WriteLine($"Training has 'hard-coded' mean squared error < {meanSquaredErrorStoppingCondition} stopping condition");

            Console.WriteLine("\nBeginning training using incremental back-propagation\n");
            //Parallel.For(0, 4, (index) => { nn.Train(trainData.Skip(index * 1000).ToArray(), maxEpochs, learnRate, momentum, weightDecay); });
            nn.Train(trainData, maxEpochs, learnRate, momentum, weightDecay);
            Console.WriteLine("Training complete");

            double[] weights = nn.GetWeights();
            Console.WriteLine("Final neural network weights and bias values:");
            ShowVector(weights, 10, 3, true);

            double trainAcc = nn.Accuracy(trainData);
            Console.WriteLine("\nAccuracy on training data = " + trainAcc.ToString("F4"));

            double testAcc = nn.Accuracy(testData);
            Console.WriteLine("\nAccuracy on test data = " + testAcc.ToString("F4"));
            int count = 0;
            while (count < 10)
            {
                double[] hand = new double[20];

                hand[0] = (double)Suit.Hearts; hand[1] = (double)Rank.Ace;
                hand[2] = (double)Suit.Spades; hand[3] = (double)Rank.Four;
                hand[4] = (double)Suit.Diamonds; hand[5] = (double)Rank.Four;
                hand[6] = (double)Suit.Clubs; hand[7] = (double)Rank.Four;
                hand[8] = (double)Suit.Hearts; hand[9] = (double)Rank.Nine;
                hand[14] = 1;


                var result = nn.ComputeResult(hand.Take(10).ToArray());
                var list = result.ToList();
                string winner = list.FindIndex(l => l == list.Max()).ToString();

                Console.WriteLine($"{winner} with an accuracy of {nn.Accuracy(new double[][] { hand }).ToString("N2")}");
                count++;
            }

            //Console.WriteLine("\nEnd Build 2013 neural network demo\n");
            Console.ReadLine();

        } // Main

        static void MakeTrainTest(double[][] allData, out double[][] trainData, out double[][] testData)
        {
            // split allData into 80% trainData and 20% testData
            Random rnd = new Random(0);
            int totRows = allData.Length;
            int numCols = allData[0].Length;

            int trainRows = (int)(totRows * 0.80); // hard-coded 80-20 split
            int testRows = totRows - trainRows;

            trainData = new double[trainRows][];
            testData = new double[testRows][];

            int[] sequence = new int[totRows]; // create a random sequence of indexes
            for (int i = 0; i < sequence.Length; ++i)
                sequence[i] = i;

            for (int i = 0; i < sequence.Length; ++i)
            {
                int r = rnd.Next(i, sequence.Length);
                int tmp = sequence[r];
                sequence[r] = sequence[i];
                sequence[i] = tmp;
            }

            int si = 0; // index into sequence[]
            int j = 0; // index into trainData or testData

            for (; si < trainRows; ++si) // first rows to train data
            {
                trainData[j] = new double[numCols];
                int idx = sequence[si];
                Array.Copy(allData[idx], trainData[j], numCols);
                ++j;
            }

            j = 0; // reset to start of test data
            for (; si < totRows; ++si) // remainder to test data
            {
                testData[j] = new double[numCols];
                int idx = sequence[si];
                Array.Copy(allData[idx], testData[j], numCols);
                ++j;
            }
        } // MakeTrainTest

        static void Normalize(double[][] dataMatrix, int[] cols)
        {
            // normalize specified cols by computing (x - mean) / sd for each value
            foreach (int col in cols)
            {
                double sum = 0.0;
                for (int i = 0; i < dataMatrix.Length; ++i)
                    sum += dataMatrix[i][col];
                double mean = sum / dataMatrix.Length;
                sum = 0.0;
                for (int i = 0; i < dataMatrix.Length; ++i)
                    sum += (dataMatrix[i][col] - mean) * (dataMatrix[i][col] - mean);
                // thanks to Dr. W. Winfrey, Concord Univ., for catching bug in original code
                double sd = Math.Sqrt(sum / (dataMatrix.Length - 1));
                for (int i = 0; i < dataMatrix.Length; ++i)
                    dataMatrix[i][col] = (dataMatrix[i][col] - mean) / sd;
            }
        }

        static void ShowVector(double[] vector, int valsPerRow, int decimals, bool newLine)
        {
            for (int i = 0; i < vector.Length; ++i)
            {
                if (i % valsPerRow == 0) Console.WriteLine("");
                Console.Write(vector[i].ToString("F" + decimals).PadLeft(decimals + 4) + " ");
            }
            if (newLine == true) Console.WriteLine("");
        }

        static void ShowMatrix(double[][] matrix, int numRows, int decimals, bool newLine)
        {
            for (int i = 0; i < numRows; ++i)
            {
                Console.Write(i.ToString().PadLeft(3) + ": ");
                for (int j = 0; j < matrix[i].Length; ++j)
                {
                    if (matrix[i][j] >= 0.0) Console.Write(" "); else Console.Write("-");
                    Console.Write(Math.Abs(matrix[i][j]).ToString("F" + decimals) + " ");
                }
                Console.WriteLine("");
            }
            if (newLine == true) Console.WriteLine("");
        }

    }

}