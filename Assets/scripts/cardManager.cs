using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class CardManager : MonoBehaviour
{
    [SerializeField] GameObject cardPrefab;
    public List<Card> deck;
    public List<Card> playedCards;
    [SerializeField] public GameObject unoPrefab;
    [SerializeField] GameManager gameManager;
    private void Start()
    {
        gameManager = gameObject.GetComponent<GameManager>();
    }
    public void StartNewGame()
    {
        GenerateDeck();
        StartCoroutine(StartDeal());
        StartCoroutine(PlayCard(deck[0], deck));
    }
    void GenerateDeck()
    {
        for (int i = 0; i <= 14; i++)
        {
            for (int j = 0; j <= 3; j++)
            {
                GameObject cardToAdd = Instantiate(cardPrefab, transform.position,Quaternion.identity);
                cardToAdd.transform.parent = null;
                cardToAdd.GetComponent<Card>().suit = j;
                cardToAdd.GetComponent<Card>().cardLvl = i;
                deck.Add(cardToAdd.GetComponent<Card>());
                if (i <= 12) 
                {
                    GameObject duplicateCard = Instantiate(cardPrefab, transform.position, Quaternion.identity);
                    duplicateCard.transform.parent = null;
                    duplicateCard.GetComponent<Card>().suit = j;
                    duplicateCard.GetComponent<Card>().cardLvl = i;
                    deck.Add(duplicateCard.GetComponent<Card>());
                }
            }
        }
        Shuffle();
    }
    IEnumerator StartDeal()
    {
        for (int i = 0; i <= 5; i++)
        {
            foreach (GameObject curPlayer in gameManager.curPlayers)
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
        List<Card> shuffledDeck = new List<Card>();
        while (deck.Count > 0)
        {
            int cardNum = Random.Range(0, deck.Count);
            shuffledDeck.Add(deck[cardNum]);
            deck.RemoveAt(cardNum);
        }
        deck = shuffledDeck;
    }
    public Card Deal(GameObject player)
    {
        Card cardToDeal = deck[0];
        deck.RemoveAt(0);
        cardToDeal.moveTo = player.transform.position;
        cardToDeal.moving = true;
        player.GetComponent<HandManager>().hand.Add(cardToDeal);
        return cardToDeal;
    }
    public IEnumerator PlayCard(Card playCard, List<Card> list)
    {
        playedCards.Add(playCard);
        list.Remove(playCard);
        try { playCard.sr.sortingOrder = playedCards.IndexOf(playCard); } catch { }
        playCard.moveTo = new Vector2(transform.position.x + 2 + Random.Range(-0.1f,0.1f), transform.position.y + Random.Range(-0.1f, 0.1f));
        playCard.moving = true;
        StartCoroutine(playCard.SpecialAbility());
        while (playCard.moving) { yield return null; }
        playCard.gameObject.transform.rotation = Quaternion.Euler(0, 0, Random.Range(-12, 13));
    }
    public bool CheckCardPlayabilty(Card playCard)
    {
        if (playCard.cardLvl >= 13) {return true; }
        if (playCard.cardLvl == playedCards[playedCards.Count - 1].cardLvl) { return true; }
        if (playCard.suit == playedCards[playedCards.Count - 1].suit) { return true; }
        return false;
    }
}
