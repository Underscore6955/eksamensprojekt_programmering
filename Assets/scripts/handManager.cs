using UnityEngine;
using System.Collections.Generic;
using Unity.VisualScripting;
using System;

public class HandManager : MonoBehaviour
{
    float radius = 8;
    public List<GameObject> hand = new List<GameObject>();
    public int playerNum;
    public bool ai;
    private void Update()
    {
        if (hand.Count <= 0) GameObject.Find("game Manager").GetComponent<CardManager>().Win(gameObject);
    }
    void LateUpdate()
    {
        BuildHand();
        if (!ai) HighlightCard();
    }
    void BuildHand()
    {
        float angle = Mathf.Min(75, hand.Count*5);
        float startAngle = -angle / 2f;
        float angleStep = angle / Mathf.Max(1, hand.Count - 1);

        for (int i = 0; i < hand.Count; i++)
        {
            GameObject card = hand[i];
            card.GetComponent<Card>().sr.color = new Color(0.5f, 0.5f, 0.5f);
            if (card.GetComponent<Card>().moving) return;
            if (!card.GetComponent<Card>().selected) card.GetComponent<Card>().sr.sortingOrder = i;
            float curAngle = startAngle + i * angleStep;
            if (ai) card.GetComponent<Card>().sr.gameObject.transform.localScale = Vector2.one * 0.3f;
            float radian = curAngle * Mathf.Deg2Rad;
            Vector3 cardPosition = transform.position + new Vector3(Mathf.Sin(radian) * (radius -Convert.ToInt32(ai)*0.7f*radius), Mathf.Cos(radian) * (radius- Convert.ToInt32(ai) * 0.7f*radius) - (radius- Convert.ToInt32(ai)) * 0.7f*radius, 0);
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
            if (hand.Contains(curObj.gameObject))
            {
                if (selectedCard == null || curObj.GetComponent<Card>().sr.sortingOrder > selectedCard.GetComponent<Card>().sr.sortingOrder)
                {
                    selectedCard = curObj.gameObject;
                }
            }
        }
        if (selectedCard == null)
            return;
        Card cardComponent = selectedCard.GetComponent<Card>();
        cardComponent.sr.sortingOrder = hand.Count + 1;
        cardComponent.sr.gameObject.transform.localScale = cardComponent.srSize * 1.4f;
        CardManager cardManager = GameObject.Find("game Manager").GetComponent<CardManager>();
        selectedCard.GetComponent<Card>().sr.gameObject.transform.position += selectedCard.GetComponent<Card>().sr.gameObject.transform.up * 0.275f;
        if (!cardManager.CheckCardPlayabilty(selectedCard)) cardComponent.sr.color = new Color(0.7f, 0.7f, 0.7f); else { cardComponent.sr.color = new Color(1f, 1f, 1f); }
        cardComponent.selected = true;
    }
}
