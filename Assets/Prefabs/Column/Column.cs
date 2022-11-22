using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Column : MonoBehaviour
{
    [SerializeField] int cardNum;
    [SerializeField] float cardOffset;
    public static float cardVerticalDistance;
    [SerializeField] GameObject cardPrefab;
    Card[] cards;
    [SerializeField] int turnedCards;
    public bool isEmpty
    {   get{
            return turnedCards > cardNum;
        }
    }

    private void Awake()
    {
        cards = new Card[cardNum];
        turnedCards = 1;
        cardVerticalDistance = cardOffset;

        Card c=null;
        for (int i = 0; i < cardNum; i++)
        {
            c = SpawnCard(i * cardOffset);
            if (i < cardNum - 1)
            {
                c.transform.Rotate(0, 180, 0);
                c.dragController.blockMovement = true;
            }
            cards[i] = c;
        }
        cards[cardNum-1].transform.rotation = Quaternion.identity;
    }

    Card SpawnCard(float offs)
    {
        Card c = Instantiate(cardPrefab).GetComponent<Card>();
        c.transform.position = transform.position + Vector3.down * offs + Vector3.back*(offs+0.1f)*2;
        // transform is spawn position

        do
        {
            c.Init(Source.COLUMN); 
        } while (Deck.usedCards.Contains(c.ToStruct()));

        Deck.usedCards.Add(c.ToStruct());
        c.dragController.cardPlacedOnPile += TurnCard;
        return c;
    }

    void TurnCard()
    {
        turnedCards++;
        if (turnedCards <= cardNum)
        {
            cards[cardNum - turnedCards+1].dragController.cardPlacedOnPile -= TurnCard;
            Card c = cards[cardNum - turnedCards];
            c.transform.rotation = Quaternion.identity;
            c.dragController.blockMovement = false;
        }
    }
}
