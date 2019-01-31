using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Markov;

namespace DiscordMarkov
{
    public static class MarkovHandler
    {
        static MarkovChain<string> Chain = new MarkovChain<string>(1);
        static readonly Random _random = new Random();

        private static string FilePath = Directory.GetParent(Environment.CurrentDirectory).Parent.FullName + @"\SavedChains.txt";

        public static void InitializeChain()
        {
            if (!File.Exists(FilePath))
            {
                File.CreateText(FilePath);
            }
            else
            {
                string[] TextLines = System.IO.File.ReadAllLines(FilePath);

                Chain.Add(TextLines);
            }
        }

        public static void AddString(string Message)
        {
            string[] CutMessage = Message.Split(null);

            Chain.Add(CutMessage);

            using (StreamWriter sw = File.AppendText(FilePath))
            {
                foreach (string MessageString in CutMessage)
                {
                    sw.WriteLine(MessageString);
                }
            }
        }

        public static string ReturnMessage()
        {
            string Sentence = string.Join(" ", Chain.Chain(_random));

            return Sentence;
        }

    }
}
