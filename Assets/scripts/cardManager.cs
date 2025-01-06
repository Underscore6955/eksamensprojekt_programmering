using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;

public class cardManager : MonoBehaviour
{
    public List<Card> deck;
    void Start()
    {
        
    }

    void Update()
    {
        
    }
    void Shuffle()
    {
        List<Card> shuffledDeck = new List<Card>();
        foreach (Card card in deck) { int cardNum = Random.Range(0,deck.Count); shuffledDeck.Add(deck[cardNum]); deck.RemoveAt(cardNum); }
        deck = shuffledDeck;
    }
    void Deal(int player)
    {

    }
}
