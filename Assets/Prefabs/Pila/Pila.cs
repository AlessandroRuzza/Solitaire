using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pila : MonoBehaviour
{
    [SerializeField] SpriteRenderer seedSpriteRenderer;
    [SerializeField] SeedWrap.Seed seed;
    string seedString { get { return SeedWrap.getSeedFromEnum(seed);  } }
    int topNum = 0;
    Card c = null;

    private void Start()
    {
        seedSpriteRenderer.sprite = Card.staticRef.spriteSeed[(int)seed];
    }

    public bool ValidateCard(Card c)    // card is valid if seed is equal and number is next
    {
        return (c.seed == seedString && c.intNum == topNum + 1);
    }
    public bool TryAddCard(Card c)
    {
        if (ValidateCard(c)) 
        {
            topNum = c.intNum;
            seed = SeedWrap.getEnumFromSeed(c.seed);
            return true;
        }
        else
            return false;
    }

    void PopCard()
    {
        if(topNum > 0)
            topNum--;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent<Card>(out c) && TryAddCard(c))
        {
            c.dragController.isOnCorrectPile = true;
            c.dragController.pilePos = transform.position + Vector3.back * topNum * 2;
        }
        else
            c = null;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (c != null)  // card was validated and added to pile, must remove it
        {
            PopCard(); 
            c.dragController.isOnCorrectPile = false;
        }
        c = null;       // reset card to null in any case
    }
}
