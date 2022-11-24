using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pile : MonoBehaviour
{
    [SerializeField] SpriteRenderer seedSpriteRenderer;
    [SerializeField] SeedWrap.Seed seed;
    string seedString { get { return SeedWrap.getSeedFromEnum(seed);  } }
    int topNum = 0;
    public Card topCard=null;

    public Vector3 pileTop
    {   get{
            // return transform.position + Vector3.back * (topNum+1) * 0.3f; //pileTop with incrementing offset
            return transform.position + Vector3.back * 0.3f;         // pileTop with static offset
        }
    }

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
        bool valid = ValidateCard(c);
        if (valid) 
        {
            AddCard(c);
        }
        return valid;
    }

    public void AddCard(Card c)
    {
        topNum = c.intNum;
        seed = SeedWrap.getEnumFromSeed(c.seed);
        c.transform.SetParent(transform);
        if (topCard != null)
        {
            if (!Deck.usedCards.Contains(topCard.ToStruct())) 
                Deck.usedCards.Add(topCard.ToStruct());
            Destroy(topCard.gameObject);
        }
        topCard = c;
    }

    public void PopCard()
    {
        if (topNum > 0)
            topNum--;
    }    
}
