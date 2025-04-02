using UnityEngine;
using UnityEngine.XR;
using static UnityEngine.GraphicsBuffer;

public class Card : MonoBehaviour
{
    public SpriteRenderer sr;
    public int cardLvl;
    public int suit;
    static string[] cardLvlTranlate = { "Draw", "Reverse", "Skip", "Wild", "Wild_Draw" } ;
    static string[] cardSuitTranslate = { "Blue", "Red", "Green", "Yellow"};
    public Vector2 moveTo;
    public bool selected;
    public bool moving;
    public Vector3 srSize;
    [SerializeField] Sprite backside;
    void Start()
    {
        sr = transform.GetChild(0).GetComponent<SpriteRenderer>();
        srSize = sr.gameObject.transform.localScale;
        if (cardLvl <= 9) { sr.sprite = Resources.Load<Sprite>(new string(cardSuitTranslate[suit] + "_" + cardLvl)); }
        else if (cardLvl <= 12) { sr.sprite = Resources.Load<Sprite>(new string(cardSuitTranslate[suit] + "_" + cardLvlTranlate[cardLvl - 10])); }
        else { sr.sprite = Resources.Load<Sprite>(new string(cardLvlTranlate[cardLvl-10])); }
    }
    private void Update()
    {
        sr.gameObject.transform.localPosition = Vector2.zero;
        sr.gameObject.transform.localScale = srSize;
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

        if (Vector2.Distance(transform.position, moveLoc) < 1f)
        {
            moving = false;
            rb.linearVelocity = Vector2.zero;
        }
    }
}
