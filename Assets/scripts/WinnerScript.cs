using TMPro;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WinnerScript : MonoBehaviour
{
    [SerializeField] TMP_Text winnerText;
    public static string text;
    private void Start()
    {
        winnerText.text = text;
    }
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && Physics2D.OverlapPoint(Camera.main.ScreenToWorldPoint(Input.mousePosition)).gameObject.name == "MainMenu")
        {
            SceneManager.LoadScene("menuScene");
        }
    }
}
