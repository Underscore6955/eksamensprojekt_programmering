using JetBrains.Annotations;
using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;

public class handManager : MonoBehaviour
{
    public List<Card> hand = new List<Card>();
    void Start()
    {
        
    }

    void Update()
    {
        if (hand.Count <= 0) Win(gameObject);
    }
    void Win(GameObject winnerObj)
    {

    }
}
