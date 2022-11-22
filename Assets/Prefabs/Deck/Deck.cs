using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Deck : MonoBehaviour
{
    [SerializeField] GameObject cardPrefab;
    [SerializeField] Transform cardSpawnTransform;
    [SerializeField] TextMeshPro textCount;
    int intText {
        get{ return int.Parse(textCount.text); }
    }
     
    Card currentCard=null;
    readonly Queue<CardStruct> deck = new Queue<CardStruct>(deckSize);   // initialize deck to target capacity
    public static readonly List<CardStruct> usedCards = new List<CardStruct>(deckSize);
    const int deckSize = 52;

    private void Start()
    {
        ResetText();
    }

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

        AddToText(-1);
        if(intText == 0) { Debug.Log("Finished turning Deck. Next card is: " + deck.Peek().ToString()); }
        if(intText < 0) { ResetText(); }

        if (deck.Count == 0 && usedCards.Count >= deckSize) // if deck is empty and all cards were used, don't draw
        {
            currentCard = null;
            return;
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

    void ResetText()
    {
        textCount.text = (deckSize - usedCards.Count).ToString();
        textCount.SetMaterialDirty();
    }
    void AddToText(int n)
    {
        textCount.text = (int.Parse(textCount.text) + n).ToString();
        textCount.SetMaterialDirty();
    }
}

