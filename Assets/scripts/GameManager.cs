using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public int turn;
    public int direction;
    public static int AIAmount;
    public List<GameObject> curPlayers;
    [SerializeField] GameObject playerPrefab;
    private void Start()
    {
        CardManager cardManager = GameObject.Find("game Manager").GetComponent<CardManager>();
        GameObject newPlayer;
        switch (AIAmount)
        {
            case 0:
                newPlayer = Instantiate(playerPrefab, new Vector2(0, -2.5f),Quaternion.identity);
                curPlayers.Add(newPlayer);
                break;
            case 1:
                newPlayer = Instantiate(playerPrefab, new Vector2(-7, -1.5f),Quaternion.identity);
                newPlayer.GetComponent<HandManager>().ai = true;
                curPlayers.Add(newPlayer);
                goto case 0;
            case 2:
                newPlayer = Instantiate(playerPrefab, new Vector2(-5, 2.5f), Quaternion.identity);
                newPlayer.GetComponent<HandManager>().ai = true;
                curPlayers.Add(newPlayer);
                goto case 1;
            case 3:
                newPlayer = Instantiate(playerPrefab, new Vector2(5, 2.5f), Quaternion.identity);
                newPlayer.GetComponent<HandManager>().ai = true;
                curPlayers.Add(newPlayer);
                goto case 2;
            case 4:
                newPlayer = Instantiate(playerPrefab, new Vector2(7, -1.5f), Quaternion.identity);
                newPlayer.GetComponent<HandManager>().ai = true;
                curPlayers.Add(newPlayer);
                goto case 3;
        }
        turn = curPlayers.Count - 1;
        cardManager.StartNewGame();
    }
    public void NextPlayer()
    {
        turn = (turn + direction)% curPlayers.Count;
        if (turn < 0) { turn += curPlayers.Count; }
    }
    public void Win(GameObject winner)
    {
        if (winner.GetComponent<HandManager>().ai) { WinnerScript.text = "An AI has won"; } else WinnerScript.text = "You won";
        SceneManager.LoadScene("winMenu");
    }
}
