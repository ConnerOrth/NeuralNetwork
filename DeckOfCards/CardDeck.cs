using Helpers;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace DeckOfCards
{
    public class CardDeck
    {
        protected ICollection<Card> Cards = new List<Card>();

        public int NumberOfCards = 52;

        public CardDeck()
        {
            for (int i = 1; i <= Enum.GetValues(typeof(Suit)).Length; i++)
            {
                for (int j = 1; j <= Enum.GetValues(typeof(Rank)).Length; j++)
                {
                    Cards.Add(new Card() { Rank = (Rank)j, Suit = (Suit)i });
                }
            }
            Shuffle();
        }

        public void Shuffle()
        {
            Cards = Cards.Shuffle().ToList();
        }

        public Card DrawCard() => DrawCards(1).SingleOrDefault();

        public IEnumerable<Card> DrawCards(int amount)
        {
            IEnumerable<Card> drawnCards = Cards.Where(c => !c.Drawn).Take(amount);
            foreach (Card card in drawnCards)
            {
                card.Drawn = true;
            }
            return drawnCards;
        }

    }

    public class PokerHand
    {
        readonly CardDeck Deck = new CardDeck();

        public readonly IList<Card> CardsInHand = new List<Card>();
        public int MaxNumberOfCards = 5;
        public Result Result;

        public PokerHand()
        {
            CardsInHand = Deck.DrawCards(MaxNumberOfCards).ToList();
        }
        public PokerHand(IList<Card> cards)
        {
            CardsInHand = cards;
        }
    }

    public class Card
    {
        public Guid Id = Guid.NewGuid();
        public int Number => Math.Abs(((int)Suit - 1) * (int)Rank) + (int)Rank;
        public string Name { get { return $"{Rank.GetName()} of {Suit.GetName()}"; } }
        public bool Drawn;
        public Rank Rank;
        public Suit Suit;        
    }

    public enum Result
    {
        /// <summary>
        /// Nothing in hand; not a recognized poker hand
        /// </summary>
        Nothing = 0,
        /// <summary>
        /// One pair; one pair of equal ranks within five cards
        /// </summary>
        OnePair = 1,
        /// <summary>
        /// Two pairs; two pairs of equal ranks within five cards
        /// </summary>
        TwoPair = 2,
        /// <summary>
        /// Three of a kind; three equal ranks within five cards
        /// </summary>
        ThreeOfAKind = 3,
        /// <summary>
        /// Straight; five cards, sequentially ranked with no gaps
        /// </summary>
        Straight = 4,
        /// <summary>
        /// Flush; five cards with the same suit
        /// </summary>
        Flush = 5,
        /// <summary>
        /// Full house; pair + different rank three of a kind
        /// </summary>
        FullHouse = 6,
        /// <summary>
        /// Four of a kind; four equal ranks within five cards
        /// </summary>
        FourOfAKind = 7,
        /// <summary>
        /// Straight flush; straight + flush
        /// </summary>
        StraightFlush = 8,
        /// <summary>
        /// Royal flush; {Ace, King, Queen, Jack, Ten } + flush
        /// </summary>
        RoyalFlush = 9,
    }

    public enum Rank
    {
        Ace = 1,
        Two = 2,
        Three = 3,
        Four = 4,
        Five = 5,
        Six = 6,
        Seven = 7,
        Eight = 8,
        Nine = 9,
        Ten = 10,
        Jack = 11,
        Queen = 12,
        King = 13,
    }

    public enum Suit
    {
        Hearts = 1,
        Spades = 2,
        Diamonds = 3,
        Clubs = 4
    }
}
