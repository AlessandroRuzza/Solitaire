using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Column : MonoBehaviour
{
    [SerializeField] int cardNum;
    public static float cardOffset = 0.6f;
    [SerializeField] GameObject cardPrefab;
    Card[] cards;
    [SerializeField] int turnedCards;
    public bool isEmpty
    {   get{
            return turnedCards > cardNum;
        }
    }
    public bool canKBePlaced=false;
    [SerializeField] bool debug_DoNotSpawnCards;

    private void Awake()
    {
        cards = new Card[cardNum];
        turnedCards = 1;
    }

    private void Start()
    {
        if (debug_DoNotSpawnCards) return;

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
        c.dragController.cardPlacedOnCard += TurnCard;
        c.dragController.cardPlacedOnCol += TurnCard;
        return c;
    }

    void TurnCard()
    {
        turnedCards++;
        if (turnedCards <= cardNum)
        {
            cards[cardNum - turnedCards + 1].dragController.cardPlacedOnPile -= TurnCard;
            cards[cardNum - turnedCards + 1].dragController.cardPlacedOnCard -= TurnCard;
            cards[cardNum - turnedCards + 1].dragController.cardPlacedOnCol -= TurnCard;
            Card c = cards[cardNum - turnedCards];
            c.transform.rotation = Quaternion.identity;
            c.dragController.blockMovement = false;
        }
        else if (turnedCards == cardNum + 1) // on last card leaving the column
            canKBePlaced = true; ;
    }
}
