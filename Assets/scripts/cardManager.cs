using UnityEngine;
using System.Collections.Generic;

public class CardManager : MonoBehaviour
{
    [SerializeField] GameObject cardPrefab;
    public List<GameObject> deck;
    public int curTurn;
    public List<GameObject> curPlayers;
    private void Start()
    {
        GenerateDeck();
        for (int i = 0; i <= 7; i++)
        {
            foreach (GameObject curPlayer in curPlayers)
            {
                Deal(curPlayer);
            }
        }
    }
    void StartNewGame()
    {
        
    }
    void GenerateDeck()
    {
        for (int i = 0; i <= 14; i++)
        {
            for (int j = 0; j <= 3; j++)
            {
                GameObject cardToAdd = Instantiate(cardPrefab);
                cardToAdd.GetComponent<Card>().suit = j;
                cardToAdd.GetComponent<Card>().cardLvl = i;
                deck.Add(cardToAdd);
                if (i <= 12) { deck.Add(cardToAdd); }
            }
        }
        Shuffle();
    }

    void Update()
    {
        
    }
    public void Win(GameObject winnerObj)
    {

    }
    void Shuffle()
    {
        List<GameObject> shuffledDeck = new List<GameObject>();
        while (deck.Count > 0)
        {
            int cardNum = Random.Range(0, deck.Count);
            shuffledDeck.Add(deck[cardNum]);
            deck.RemoveAt(cardNum);
        }
        deck = shuffledDeck;
    }
    void Deal(GameObject player)
    {
        GameObject cardToDeal = deck[0];
        deck.Remove(cardToDeal);
        cardToDeal.GetComponent<Card>().moveTo = player.transform.position;
        cardToDeal.GetComponent<Card>().moving = true;
        player.GetComponent<HandManager>().hand.Add(cardToDeal);
    }
}
