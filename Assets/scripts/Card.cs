using UnityEngine;
using UnityEngine.XR;
using static UnityEngine.GraphicsBuffer;
using System.Linq;
using System;
using System.Collections;

public class Card : MonoBehaviour
{
    public SpriteRenderer sr;
    public int cardLvl;
    public int suit;
    static string[] cardLvlTranlate = { "Reverse", "Skip", "Draw", "Wild", "Wild_Draw" } ;
    static string[] cardSuitTranslate = { "Blue", "Red", "Green", "Yellow"};
    static Color[] colorTranlate = { Color.blue, Color.red, Color.green, Color.yellow };
    public Vector2 moveTo;
    public bool selected;
    public bool moving;
    public Vector3 srSize;
    [SerializeField] Sprite backside;
    Sprite frontside;
    static GameManager gameManager;
    static CardManager cardManager;
    public static bool choosingColor;
    static int nextTurn;
    public bool turned;
    [SerializeField] GameObject colorSelectPrefab;
    void Awake()
    {
        gameManager = GameObject.Find("game Manager").GetComponent<GameManager>();
        cardManager = GameObject.Find("game Manager").GetComponent<CardManager>();
        sr = transform.GetChild(0).GetComponent<SpriteRenderer>();
        srSize = sr.gameObject.transform.localScale;
    }
    private void Start()
    {
        if (cardLvl <= 9) { frontside = Resources.Load<Sprite>(new string(cardSuitTranslate[suit] + "_" + cardLvl)); }
        else if (cardLvl <= 12) { frontside = Resources.Load<Sprite>(new string(cardSuitTranslate[suit] + "_" + cardLvlTranlate[cardLvl - 10])); }
        else { frontside = Resources.Load<Sprite>(new string(cardLvlTranlate[cardLvl - 10])); }
    }
    private void Update()
    {
        if (turned && !CardManager.openCards) { sr.sprite = backside; } else { sr.sprite = frontside; }
        sr.gameObject.transform.localPosition = Vector2.zero;
        sr.gameObject.transform.localScale = srSize;
        if (sr.color != Color.blue && sr.color != Color.red && sr.color != Color.green && sr.color != Color.yellow) sr.color = Color.white;
        selected = false;
    }
    void FixedUpdate()
    {
        if (moving) MoveToLoc(moveTo);
    }
    public void MoveToLoc(Vector2 moveLoc)
    {
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        Vector2 direction = (moveLoc - (Vector2)transform.position).normalized;
        rb.linearVelocity = direction * 30f;
        transform.rotation = Quaternion.Euler(0, 0, Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg + 270);
        if (Vector2.Distance(transform.position, moveLoc) < 1f)
        {
            moving = false;
            rb.linearVelocity = Vector2.zero;
            transform.position = moveLoc;
        }
    }
    public void SpecialAbility()
    {
        nextTurn = (gameManager.turn + gameManager.direction) % gameManager.curPlayers.Count;
        if (nextTurn < 0) nextTurn += gameManager.curPlayers.Count;
        switch (cardLvl)
        {
            case 10:
                gameManager.direction *= -1;
                break;
            case 11:
                gameManager.NextPlayer();
                break;
            case 12:
                StartCoroutine(AddCards(2));
                break;
            case 13:
                StartCoroutine(WildCard());
                break;
            case 14:
                StartCoroutine(WildCard());
                StartCoroutine(AddCards(4));
                break;
        }
    }
    public static IEnumerator AddCards(int cardAmount)
    {
        for (int i = 0; i < cardAmount; i++)
        {
            cardManager.Deal(gameManager.curPlayers[nextTurn]);
            yield return new WaitForSeconds(0.2f);
        }
    }
    private IEnumerator WildCard()
    {
        GameObject colorSelect = null;
        if (gameManager.curPlayers[gameManager.turn].GetComponent<HandManager>().ai) 
        {
            int[] suitCount = { 0,0,0,0}; 
            foreach (Card curCard in gameManager.curPlayers[gameManager.turn].GetComponent<HandManager>().hand)
            {
                if (curCard.cardLvl < 13) suitCount[curCard.suit]++;
            }
            suit = Array.IndexOf(suitCount, suitCount.Max());
            sr.color = colorTranlate[suit];
        }
        else
        {
            choosingColor = true;
            colorSelect = Instantiate(colorSelectPrefab, new Vector2(0, 0), Quaternion.identity);
        }
        while (choosingColor) 
        {
            if (Input.GetMouseButtonDown(0))
            {
                foreach (var curObj in Physics2D.OverlapPointAll(Camera.main.ScreenToWorldPoint(Input.mousePosition)))
                {
                    if (cardSuitTranslate.Any(obj => obj == curObj.gameObject.tag)) 
                    { 
                        suit = Array.IndexOf(cardSuitTranslate, curObj.gameObject.tag);
                        sr.color = colorTranlate[suit];
                        choosingColor = false; 
                    }
                }
            }
            yield return null; 
        }
        Destroy(colorSelect);
    }
}
