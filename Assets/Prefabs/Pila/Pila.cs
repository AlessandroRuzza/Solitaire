using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pila : MonoBehaviour
{
    static readonly List<string> assignedSeeds = new(4);
    [SerializeField] string seed = string.Empty;
    [SerializeField] int topNum = 0;
    Card c = null;
    static bool update=false;

    public bool ValidateCard(Card c)    // card is valid if pile is empty or seed is equal and number is next
    {
        bool seedCanBeAssigned = (seed == string.Empty && !assignedSeeds.Contains(c.seed) && c.intNum==1);  // first card must be "A"
        bool seedAndNumberAreCorrect = (c.seed == seed && c.intNum == topNum + 1);
        return (seedCanBeAssigned || seedAndNumberAreCorrect);
    }

    public bool TryAddCard(Card c)
    {
        if (ValidateCard(c)) 
        {
            topNum = c.intNum;
            seed = c.seed;
            if(!assignedSeeds.Contains(c.seed)) 
                assignedSeeds.Add(seed);
            return true;
        }
        else
            return false;
    }

    void PopCard()
    {
        if(topNum > 0)
            topNum--;
        if(topNum == 0)
        {
            assignedSeeds.Remove(seed);
            seed = string.Empty;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        update = false;
        if (collision.TryGetComponent<Card>(out c) && TryAddCard(c)) 
        {
            c.dragController.isOnCorrectPile = true;
            c.dragController.pilePos = transform.position + Vector3.back * topNum * 2;
        }            
        else
            c = null;

    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if(update && c == null) 
        {
            OnTriggerEnter2D(collision);
            update = false;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (c != null)  // card was validated and added to pile, must remove it
        {
            PopCard(); 
            c.dragController.isOnCorrectPile = false;
            update = true;
        }
        c = null;       // reset card to null in any case
    }
}
