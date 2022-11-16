using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardCollider : MonoBehaviour
{
    Card card;
    DragController dragController;
    Pile correctPile;
    Card correctCard;
    static Vector3 verticalOffset;

    private void Awake()
    {
        card = GetComponent<Card>();
        dragController = GetComponent<DragController>();
        verticalOffset = Vector3.down * Column.cardVerticalDistance;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!card.hasChildCard && collision.TryGetComponent<Pile>(out Pile p) && p.ValidateCard(card))
        {  
            // Card has collided with a Pile where it can be placed
            dragController.isOnCorrectPile = true;
            dragController.pilePos = p.pileTop;
            dragController.cardPlacedOnPile += PlaceOnPile;
            correctPile = p;
        }
        // delete "correctCard == null" to have correctCard update to last hit
        else if (correctCard == null && collision.TryGetComponent<Card>(out Card c) && card.CanBePlacedOn(c)) 
        {
            // Card has collided with a Card on a column where it can be placed
            correctCard = c;
            dragController.isOnCorrectPile = true;
            dragController.pilePos = c.transform.position + verticalOffset;
            dragController.cardPlacedOnPile += PlaceOnCard;
        }
        else if(card.num == "K" && collision.TryGetComponent<Column>(out Column col) && col.isEmpty)
        {
            dragController.isOnCorrectPile = true;
            dragController.pilePos = col.transform.position + Vector3.back * 0.2f;
            dragController.cardPlacedOnPile += PlaceOnColumn;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (correctCard == null && collision.TryGetComponent<Card>(out Card c) && card.CanBePlacedOn(c))
        {
            OnTriggerEnter2D(collision);    // fixes double collision problems after exiting the second one
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
            dragController.cardPlacedOnPile -= PlaceOnCard;
            dragController.isOnCorrectPile = false;
            correctCard = null;
        }
        else if (card.num == "K" && collision.TryGetComponent<Column>(out Column col) && col.isEmpty)
        {
            dragController.isOnCorrectPile = false;
        }
    }

    void PlaceOnCard()
    {
        transform.SetParent(correctCard.transform);
        transform.position += Vector3.back;
        dragController.blockMovement = false;
        dragController.source = Source.COLUMN;
    }
    void PlaceOnPile()
    {
        correctPile.TryAddCard(card);
    }
    void PlaceOnColumn()
    {
        dragController.blockMovement = false;
        dragController.source = Source.COLUMN;
    }
}
