using UnityEngine;
using System.Collections.Generic;

public class HandManager : MonoBehaviour
{
    float angle = 100f;
    float radius = 1;
    public List<GameObject> hand = new List<GameObject>();
    public int playerNum;
    public bool ai;
    void LateUpdate()
    {
        if (hand.Count <= 0) GameObject.Find("game Manager").GetComponent<CardManager>().Win(gameObject);
        BuildHand();
    }
    void BuildHand()
    {
        float startAngle = -angle / 2f;
        float angleStep = angle / hand.Count;
        int i = 0;
        foreach (GameObject card in hand)
        {
            if (card.GetComponent<Card>().moving == true) { return; }
            float curAngle = startAngle + i * angleStep;
            Vector3 cardPosition = transform.position + new Vector3(Mathf.Sin(curAngle*Mathf.Deg2Rad) * radius, -Mathf.Cos(curAngle*Mathf.Deg2Rad) * radius,0);
            hand[i].transform.position = cardPosition;
            hand[i].transform.rotation = Quaternion.Euler(0, 0, -curAngle);
            Debug.Log(curAngle);
            i++;
        } 
    }
}
