using UnityEngine;
using TMPro;

public class StartGame : MonoBehaviour
{
    [SerializeField] GameObject menu;
    void Start()
    {
        menu.GetComponent<Canvas>().renderMode = RenderMode.WorldSpace;
    }

    void Update()
    {
        
    }
}
