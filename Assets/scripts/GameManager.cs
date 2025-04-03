using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public int turn;
    public int direction;
    public List<GameObject> curPlayers;
    private void Start()
    {
        CardManager cardManager = GameObject.Find("game Manager").GetComponent<CardManager>();
    }
    public void NextPlayer()
    {
        turn = (turn + direction)% curPlayers.Count;
        if (turn < 0) { turn += curPlayers.Count; }
    }
    public void Update()
    {
        
    }
}
