using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
    void OnDestroy()
    {
        usedCards.Clear();    
    }

    void OnMouseDown()
    {
        if (currentCard != null && !usedCards.Contains(currentCard.ToStruct()))
        {
            deck.Enqueue(currentCard.ToStruct());
            Destroy(currentCard.gameObject);
        }

        AddToText(-1);
        if(intText == 0) { ResetText(); }   // can reset to 0 if there are no cards left
        if(intText < 0) { SetTextZero(); } // if subtracting 1 makes it negative (meaning Deck was empty), set to 0

        if (deck.Count == 0 && usedCards.Count >= deckSize) // if deck is empty and all cards were used, don't draw
        {
            currentCard = null;
            return;
        }
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
    void SetTextZero(){
        textCount.text = "0";
        textCount.SetMaterialDirty();
    }
}

