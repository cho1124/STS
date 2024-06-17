using System.Collections.Generic;
using UnityEngine;

public class PlayerDiscard : MonoBehaviour
{
    public List<Card> discardedCards = new List<Card>();

    public void AddToDiscardPile(Card card)
    {
        discardedCards.Add(card);
        Debug.Log("Card added to discard pile: " + card.cardName);
    }

    public void ClearDiscardPile()
    {
        discardedCards.Clear();
        Debug.Log("Discard pile cleared");
    }
}
