using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Card : MonoBehaviour
{
    SpriteRenderer sr;
    public int cardLvl;
    public int suit;
    static string[] cardLvlTranlate = { "Draw", "Reverse", "Skip", "Wild", "Wild_Draw" } ;
    static string[] cardSuitTranslate = { "Blue", "Red", "Green", "Yellow"};
    public Vector2 moveTo;
    public bool moving;
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        if (cardLvl <= 9) { sr.sprite = Resources.Load<Sprite>(new string(cardSuitTranslate[suit] + "_" + cardLvl)); }
        else if (cardLvl <= 12) { sr.sprite = Resources.Load<Sprite>(new string(cardSuitTranslate[suit] + "_" + cardLvlTranlate[cardLvl - 10])); }
        else { sr.sprite = Resources.Load<Sprite>(new string(cardLvlTranlate[cardLvl-10])); }
    }

    void Update()
    {
        if (moving) MoveToLoc(moveTo);
    }
    private void MoveToLoc(Vector2 moveLoc)
    {
        Vector2 direction = (moveLoc - new Vector2(transform.position.x, transform.position.y)).normalized;
        GetComponent<Rigidbody2D>().linearVelocity = direction * 500 * Time.deltaTime;
        if (Vector2.Distance(transform.position, moveLoc) < 0.1f) { moving = false; GetComponent<Rigidbody2D>().linearVelocity = Vector2.zero; }
    }
}
