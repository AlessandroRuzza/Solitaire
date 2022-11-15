using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Deck : MonoBehaviour
{
    [SerializeField] GameObject cardPrefab;
    [SerializeField] Transform cardSpawnTransform;
     
    Card currentCard=null;
    readonly Queue<CardStruct> deck = new Queue<CardStruct>(deckSize);   // initialize deck to target capacity
    public static readonly List<CardStruct> usedCards = new List<CardStruct>(deckSize/2);
    const int deckSize = 52;

    void OnMouseDown()
    {
        if (currentCard != null)
        {
            if (currentCard.transform.position == cardSpawnTransform.position)
            {
                deck.Enqueue(currentCard.ToStruct());
                Destroy(currentCard.gameObject);
            }
            else
                usedCards.Add(currentCard.ToStruct());
        }

        //Debug.Log(deck.Count);
        currentCard = Instantiate(cardPrefab).GetComponent<Card>();
        currentCard.transform.position = cardSpawnTransform.position;

        int cardsExtracted = deck.Count + usedCards.Count;
        if (cardsExtracted < deckSize)
        {
            do
            {
                currentCard.Init(); // keeps updating card until it finds a new card
                // TO DO: improve algorithm, random is bad?
            }while (deck.Contains(currentCard.ToStruct()) || usedCards.Contains(currentCard.ToStruct()) );
        }
        else
        {
            currentCard.Init(deck.Dequeue());
        }
    }
}

