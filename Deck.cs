using System;
using System.Collections;
using System.Linq;

public class Deck
{
    public IList<Card> Cards = new IList<Card>();
    public int NumberOfCards = 52;

    public Deck(bool shuffleDeck = true)
	{
        for (int i = 1; i <= Enum.GetValues(Suit).Length; i++)
        {
            for (int j = 1; j <= Enum.GetValues(Rank).Length; j++)
            {
                Cards.Add(new Card() { Rank = (Rank)j, Suit = (Suit)i });
            }
        }
        if (shuffleDeck)
        {
            Cards.Shuffle().ToList();
        }
	}
}

public class PokerHand
{

}


public class Card
{
    public Rank Rank;
    public Suit Suit;

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
