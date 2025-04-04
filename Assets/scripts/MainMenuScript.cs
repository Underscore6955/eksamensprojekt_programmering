using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuScript : MonoBehaviour
{
    int AIamount = 1;
    [SerializeField] TMP_Text AIAmountText;
    bool openCards;
    private void Update()
    {
        if (Input.GetMouseButtonUp(0))
        {
            try
            {
                switch (Physics2D.OverlapPoint(Camera.main.ScreenToWorldPoint(Input.mousePosition)).gameObject.name)
                {
                    case "MoreAI":
                        AIamount++;
                        break;
                    case "LessAI":
                        AIamount--;
                        break;
                    case "Start":
                        GameManager.AIAmount = AIamount;CardManager.openCards = openCards; SceneManager.LoadScene("playingScene");
                        break;
                    case "OpenCard":
                        openCards = !openCards;
                        if (openCards) { Physics2D.OverlapPoint(Camera.main.ScreenToWorldPoint(Input.mousePosition)).gameObject.GetComponent<SpriteRenderer>().color = Color.green; } else Physics2D.OverlapPoint(Camera.main.ScreenToWorldPoint(Input.mousePosition)).gameObject.GetComponent<SpriteRenderer>().color = Color.red;
                        break;
                }
                AIamount = Mathf.Clamp(AIamount, 1, 4);
            }
            catch{ };
        }
        AIAmountText.text = AIamount.ToString();
    }
}
