using DeckOfCards;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeckOfCardsTest
{
    [TestClass]
    public class DeckOfCardsTest
    {
        double[][] data;

        [TestInitialize]
        public void Setup()
        {
            string rootfolder = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            string dir = "NeuralNetworkDataSets\\pokerhand-set";
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

            data = new double[dataDict.Count][];
            foreach (var item in dataDict)
            {
                data[item.Key] = item.Value;
            }
        }

        [TestMethod]
        public void ConstructorTest()
        {
            CardDeck deck = new CardDeck();
            var x = deck.DrawCards(10);
            var y = deck.DrawCards(50);
            var z = x.Concat(y);
            string cards = "";
            foreach (var card in z)
            {
                cards += $"{card.Name}{Environment.NewLine}";
            }
            PokerHand pokerHand = new PokerHand();

            var cardsInHand = pokerHand.CardsInHand;
        }

        [TestMethod]
        public void PremadePokerHandTest()
        {
            //PokerHand pokerHand = new PokerHand();
            IList<PokerHand> PokerHands = new List<PokerHand>();
            IList<Card> cards = new List<Card>();
            
            var hands = data.Take(10).ToList();
            foreach (var hand in hands)
            {
                var handEnumerator = hand.GetEnumerator();
                cards.Clear();
                for (int i = 0; i < 5; i++)
                {
                    handEnumerator.MoveNext();
                    Suit suit = (Suit)Enum.Parse(typeof(Suit), handEnumerator.Current.ToString());
                    handEnumerator.MoveNext();
                    Rank rank = (Rank)Enum.Parse(typeof(Rank), handEnumerator.Current.ToString());

                    cards.Add(new Card() { Suit = (Suit)suit, Rank = (Rank)rank });
                }
                PokerHands.Add(new PokerHand(cards));
            }

        }

        [TestMethod]
        public void GetCardNumberTest()
        {
            CardDeck cardDeck = new CardDeck();
            var card = cardDeck.DrawCard();

        }
    }
}
