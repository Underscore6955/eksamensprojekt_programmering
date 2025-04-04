using UnityEngine;
using System.Collections.Generic;
using Unity.VisualScripting;
using System;
using System.Linq;
using System.Collections;

public class HandManager : MonoBehaviour
{
    float radius = 8;
    public List<Card> hand = new List<Card>();
    public bool ai;
    GameManager gameManager;
    CardManager cardManager;
    bool hasPlayed = false;
    bool callingUno;
    public static bool openCards;
    private void Start()
    {
        gameManager = GameObject.Find("game Manager").GetComponent<GameManager>();
        cardManager = GameObject.Find("game Manager").GetComponent<CardManager>();
    }
    private void Update()
    {
        if (Card.choosingColor || !cardManager.finishedDealing) return; 
        if (Physics2D.OverlapPointAll(Camera.main.ScreenToWorldPoint(Input.mousePosition)).Any(collider => collider.gameObject == gameManager.gameObject) && Input.GetMouseButtonUp(0) && gameManager.curPlayers[gameManager.turn] == gameObject && !Card.choosingColor && !callingUno)
        {
            Card newCard = cardManager.Deal(gameObject);
            if (cardManager.CheckCardPlayabilty(newCard)){StartCoroutine(cardManager.PlayCard(newCard, hand)); }
            gameManager.NextPlayer();
        }
        if (ai && gameManager.curPlayers[gameManager.turn] == gameObject && !hasPlayed) { hasPlayed = true; StartCoroutine(AIPlay()); }
        if (hand.Count <= 0) gameManager.Win(gameObject);
        hand = SortHand(hand);
    }
    void LateUpdate()
    {
        BuildHand();
        if (!ai && gameManager.curPlayers[gameManager.turn] == gameObject) HighlightCard();
    }
    private List<Card> SortHand(List<Card> hand)
    {
        return hand.OrderBy(card => card.cardLvl == 13 || card.cardLvl == 14 ? 1 : 0)
                    .ThenBy(card => card.cardLvl == 13 || card.cardLvl == 14 ? 0 : card.suit) 
                    .ThenBy(card => card.cardLvl) 
                    .ToList();
    }
    void BuildHand()
    {
        float angle = Mathf.Min(75, hand.Count*5);
        float startAngle = -angle / 2f;
        float angleStep = angle / Mathf.Max(1, hand.Count - 1);

        for (int i = 0; i < hand.Count; i++)
        {
            Card card = hand[i];
            card.sr.color = new Color(0.5f, 0.5f, 0.5f);
            if (card.moving) return;
            if (!card.selected) card.sr.sortingOrder = i;
            float curAngle = startAngle + i * angleStep;
            if (ai) card.sr.gameObject.transform.localScale = Vector2.one * 0.3f;
            float radian = curAngle * Mathf.Deg2Rad;
            Vector3 cardPosition = transform.position + new Vector3(Mathf.Sin(radian) * (radius -Convert.ToInt32(ai)*0.7f*radius), Mathf.Cos(radian) * (radius- Convert.ToInt32(ai) * 0.7f*radius) - (radius- Convert.ToInt32(ai) * 0.7f * radius), 0);
            float cardRotation = -curAngle;
            card.transform.position = cardPosition;
            card.transform.rotation = Quaternion.Euler(0, 0, cardRotation);
        }
    }
    void HighlightCard()
    {
        GameObject selectedCard = null;
        foreach (var curObj in Physics2D.OverlapPointAll(Camera.main.ScreenToWorldPoint(Input.mousePosition)))
        {
            if (hand.Contains(curObj.gameObject.GetComponent<Card>()))
            {
                if (selectedCard == null || curObj.GetComponent<Card>().sr.sortingOrder > selectedCard.GetComponent<Card>().sr.sortingOrder)
                {
                    selectedCard = curObj.gameObject;
                }
            }
        }
        if (selectedCard == null) return;
        Card cardScript = selectedCard.GetComponent<Card>();
        cardScript.sr.sortingOrder = hand.Count + 1;
        cardScript.sr.gameObject.transform.localScale = cardScript.srSize * 1.4f;
        cardScript.sr.gameObject.transform.position += cardScript.sr.gameObject.transform.up * 0.3f;
        if (!cardManager.CheckCardPlayabilty(cardScript)) cardScript.sr.color = new Color(0.7f, 0.7f, 0.7f); 
        else 
        {
            if (Input.GetMouseButtonUp(0)) { StartCoroutine(cardManager.PlayCard(cardScript, hand)); if (hand.Count == 1) StartCoroutine(CallUno()); gameManager.NextPlayer(); }
            cardScript.sr.color = new Color(1f, 1f, 1f); 
        }
        cardScript.selected = true;
    }
    IEnumerator AIPlay()
    {
        yield return new WaitForSeconds(1);
        Card selectedCard = null;
        foreach (Card curCard in hand)
        {
            if (!cardManager.CheckCardPlayabilty(curCard)) { continue; }
            int nextTurn = (gameManager.turn + gameManager.direction) % gameManager.curPlayers.Count;
            if (nextTurn < 0) nextTurn += gameManager.curPlayers.Count;
            if (gameManager.curPlayers[nextTurn].GetComponent<HandManager>().hand.Count < 2)
            {
                if (curCard == null) selectedCard = curCard;
                switch (curCard.cardLvl)
                {
                    case 14:
                        selectedCard = curCard;
                        break;

                    case int n when (n > 9 && n < 13):
                        if (selectedCard == null || selectedCard.cardLvl < 10 || selectedCard.cardLvl == 13 || curCard.cardLvl > selectedCard.cardLvl)
                        {
                            selectedCard = curCard;
                        }
                        break;

                    case 13:
                        if (selectedCard == null || selectedCard.cardLvl < 10 || selectedCard.cardLvl == 13)
                        {
                            selectedCard = curCard;
                        }
                        break;

                    case int n when (n < 10):
                        if (selectedCard == null || (selectedCard.cardLvl < 10 && curCard.cardLvl > selectedCard.cardLvl))
                        {
                            selectedCard = curCard;
                        }
                        break;
                }
            }
            else
            {
                switch (curCard.cardLvl)
                {
                    case int n when (n < 10):
                        if (selectedCard == null || (selectedCard.cardLvl < 10 && curCard.cardLvl > selectedCard.cardLvl) || selectedCard.cardLvl > 9)
                        {
                            selectedCard = curCard;
                        }
                        break;

                    case int n when (n > 9 && n < 13):
                        if (selectedCard == null || selectedCard.cardLvl >= 13 || curCard.cardLvl > selectedCard.cardLvl && selectedCard.cardLvl > 9 && selectedCard.cardLvl < 13)
                        {
                            selectedCard = curCard;
                        }
                        break;

                    case 13:
                        if (selectedCard == null || selectedCard.cardLvl == 14)
                        {
                            selectedCard = curCard;
                        }
                        break;

                    case 14:
                        if (selectedCard == null)
                        {
                            selectedCard = curCard;
                        }
                        break;
                }
            }
        }
        if (selectedCard == null) { Card newCard = cardManager.Deal(gameObject); if (cardManager.CheckCardPlayabilty(newCard)) { selectedCard = newCard; }  }
        if (selectedCard != null) { StartCoroutine(cardManager.PlayCard(selectedCard,hand)); if (hand.Count == 1) StartCoroutine(CallUno()); }
        gameManager.NextPlayer();
        hasPlayed = false;
    }
    IEnumerator CallUno()
    {
        callingUno = true;
        if (ai) 
        {
            if (UnityEngine.Random.Range(0, 3) == 0)
            {
                float timer = 0f;
                while (timer < 2)
                {
                    yield return null;

                    if (Input.GetKeyDown(KeyCode.Space))
                    {
                        cardManager.Deal(gameObject);
                        yield return new WaitForSeconds(0.2f);
                        cardManager.Deal(gameObject);
                        callingUno = false;
                        yield break;
                    }

                    timer += Time.deltaTime;
                }
            }
            GameObject unoCalledObj = Instantiate(cardManager.unoPrefab, new Vector2(0, 0), Quaternion.identity);
            yield return new WaitForSeconds(0.5f);
            Destroy(unoCalledObj);
        }
        else
        {
            float timer = 0f;
            while (timer < 2)
            {
                yield return null;

                if (Input.GetKeyDown(KeyCode.U))
                {
                    GameObject unoCalledObj = Instantiate(cardManager.unoPrefab, new Vector2(0, 0), Quaternion.identity);
                    yield return new WaitForSeconds(0.5f);
                    Destroy(unoCalledObj);
                    callingUno = false;
                    yield break;
                }
                timer += Time.deltaTime;
            }
            cardManager.Deal(gameObject);
            yield return new WaitForSeconds(0.2f);
            cardManager.Deal(gameObject);
        }
        callingUno = false;
    }
}
