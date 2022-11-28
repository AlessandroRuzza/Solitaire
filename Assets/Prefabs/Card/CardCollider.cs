using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardCollider : MonoBehaviour
{
    Card card;
    DragController dragController;
    Pile correctPile;
    Card correctCard;
    Column correctColumn;
    static Vector3 verticalOffset;

    private void Start()
    {
        card = GetComponent<Card>();
        dragController = GetComponent<DragController>();
        verticalOffset = Vector3.down * Mathf.Max(Column.cardVerticalDistance, 0.55f);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!card.hasChildCard && collision.TryGetComponent<Pile>(out Pile p) && p.ValidateCard(card))
        {  
            // Card has collided with a Pile where it can be placed
            correctPile = p;
            dragController.isOnCorrectPile = true;
            dragController.pilePos = p.pileTop;
            dragController.cardPlacedOnPile += PlaceOnPile;
        }
        // delete "correctCard == null" to have correctCard update to last hit
        else if (correctCard == null && collision.TryGetComponent<Card>(out Card c) && card.CanBePlacedOn(c)) 
        {
            // Card has collided with a Card on a column where it can be placed
            correctCard = c;
            dragController.isOnCorrectCard = true;
            dragController.cardPos = c.transform.position + verticalOffset;
            dragController.cardPlacedOnCard += PlaceOnCard;
        }
        else if(card.num == "K" && collision.TryGetComponent<Column>(out Column col) && col.canKBePlaced)
        {
            correctColumn = col;
            dragController.isOnCorrectCol = true;
            dragController.colPos = col.transform.position + Vector3.back * 0.2f;
            dragController.cardPlacedOnCol += PlaceOnColumn;
            col.canKBePlaced = false;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (card.num != "K" && correctCard == null && collision.TryGetComponent<Card>(out Card c) && card.CanBePlacedOn(c))
        {
            OnTriggerEnter2D(collision);    // fixes double collision problems after exiting the second one
        }
        else if(card.num == "K" && correctColumn == null && collision.TryGetComponent<Column>(out Column col) && col.canKBePlaced)
        {
            OnTriggerEnter2D(collision);    // fixes double collision problems for columns
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.TryGetComponent<Pile>(out Pile p) && p == correctPile)
        {
            dragController.cardPlacedOnPile -= PlaceOnPile;
            card.dragController.isOnCorrectPile = false;
            correctPile = null;
        }
        else if (collision.TryGetComponent<Card>(out Card c) && c == correctCard)
        {
            dragController.cardPlacedOnCard -= PlaceOnCard;
            dragController.isOnCorrectCard = false;
            correctCard = null;
        }
        else if (card.num == "K" && collision.TryGetComponent<Column>(out Column col) && col == correctColumn)
        {
            correctColumn.canKBePlaced = true;
            dragController.cardPlacedOnCol -= PlaceOnColumn;
            dragController.isOnCorrectCol = false;
            correctColumn = null;
        }
    }

    void PlaceOnCard()
    {
        transform.SetParent(correctCard.transform);
        transform.position += Vector3.back;
        PlaceOnColumn();    // does the same things, only difference is setting parent and offsetting position
    }
    void PlaceOnPile()
    {
        if (dragController.source == Source.DECK) Deck.usedCards.Add(card.ToStruct());
        correctPile.AddCard(card);
    }
    void PlaceOnColumn()
    {
        if (dragController.source == Source.DECK) Deck.usedCards.Add(card.ToStruct());
        dragController.source = Source.COLUMN;
    }
}
