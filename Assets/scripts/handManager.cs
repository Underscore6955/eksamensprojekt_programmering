using UnityEngine;
using System.Collections.Generic;
using Unity.VisualScripting;
using System;
using System.Linq;

public class HandManager : MonoBehaviour
{
    float radius = 8;
    public List<Card> hand = new List<Card>();
    public int playerNum;
    public bool ai;
    private void Update()
    {
        if (hand.Count <= 0) GameObject.Find("game Manager").GetComponent<CardManager>().Win(gameObject);
        hand = SortHand(hand);
    }
    void LateUpdate()
    {
        BuildHand();
        if (!ai) HighlightCard();
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
        if (selectedCard == null)
            return;
        Card cardScript = selectedCard.GetComponent<Card>();
        CardManager cardManager = GameObject.Find("game Manager").GetComponent<CardManager>();
        cardScript.sr.sortingOrder = hand.Count + 1;
        cardScript.sr.gameObject.transform.localScale = cardScript.srSize * 1.4f;
        cardScript.sr.gameObject.transform.position += cardScript.sr.gameObject.transform.up * 0.3f;
        if (!cardManager.CheckCardPlayabilty(cardScript)) cardScript.sr.color = new Color(0.7f, 0.7f, 0.7f); 
        else 
        {
            if (Input.GetMouseButtonUp(0)) { cardManager.PlayCard(cardScript,hand); }
            cardScript.sr.color = new Color(1f, 1f, 1f); 
        }
        cardScript.selected = true;
    }
}
