using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class CardManager : MonoBehaviour
{
    [SerializeField] GameObject cardPrefab;
    public List<GameObject> deck;
    public int curTurn;
    public List<GameObject> curPlayers;
    public List<GameObject> playedCards;
    private void Start()
    {
        StartNewGame();
    }
    void StartNewGame()
    {
        GenerateDeck();
        StartCoroutine(StartDeal());
        PlayCard(deck[0], deck);
    }
    void GenerateDeck()
    {
        for (int i = 0; i <= 14; i++)
        {
            for (int j = 0; j <= 3; j++)
            {
                GameObject cardToAdd = Instantiate(cardPrefab, transform);
                cardToAdd.GetComponent<Card>().suit = j;
                cardToAdd.GetComponent<Card>().cardLvl = i;
                deck.Add(cardToAdd);
                if (i <= 12) 
                {
                    GameObject duplicateCard = Instantiate(cardPrefab,transform);
                    duplicateCard.GetComponent<Card>().suit = j;
                    duplicateCard.GetComponent<Card>().cardLvl = i;
                    deck.Add(duplicateCard);
                }
            }
        }
        Shuffle();
    }
    IEnumerator StartDeal()
    {
        for (int i = 0; i <= 30; i++)
        {
            foreach (GameObject curPlayer in curPlayers)
            {
                Deal(curPlayer);
                yield return new WaitForSeconds(0.1f);
            }
        }
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
        deck.RemoveAt(0);
        cardToDeal.GetComponent<Card>().moveTo = player.transform.position;
        cardToDeal.GetComponent<Card>().moving = true;
        player.GetComponent<HandManager>().hand.Add(cardToDeal);
    }
    IEnumerator waitCardDeal(float time)
    {
        yield return new WaitForSeconds(time);
    }
    public void PlayCard(GameObject playCard, List<GameObject> list)
    {
        playedCards.Add(playCard);
        list.Remove(playCard);
        playCard.GetComponent<Card>().moveTo = new Vector2(transform.position.x + 2, transform.position.y);
        playCard.GetComponent<Card>().moving = true;
    }
    public bool CheckCardPlayabilty(GameObject playCard)
    {
        Debug.Log(playCard.GetComponent<Card>().cardLvl + " " + playCard.GetComponent<Card>().suit);
        Debug.Log(playedCards[playedCards.Count - 1].GetComponent<Card>().cardLvl + " " + playedCards[playedCards.Count - 1].GetComponent<Card>().suit);
        if (playCard.GetComponent<Card>().cardLvl >= 13) {Debug.Log("wildcard"); return true; }
        if (playCard.GetComponent<Card>().cardLvl == playedCards[playedCards.Count - 1].GetComponent<Card>().cardLvl) { Debug.Log("same lvl"); return true; }
        if (playCard.GetComponent<Card>().suit == playedCards[playedCards.Count - 1].GetComponent<Card>().suit) { Debug.Log("same suit"); return true; }
        return false;
    }
}
