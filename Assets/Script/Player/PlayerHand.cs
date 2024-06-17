using System.Collections.Generic;
using UnityEngine;

public class PlayerHand : MonoBehaviour
{
    public List<Card> handCards = new List<Card>();
    public int maxHandSize = 10;

    public void AddCard(Card card)
    {
        if (handCards.Count < maxHandSize)
        {
            handCards.Add(card);
            Debug.Log("Card added to hand: " + card.cardName);
        }
        else
        {
            Debug.Log("Hand is full!");
        }
    }

    public void RemoveCard(Card card)
    {
        if (handCards.Contains(card))
        {
            handCards.Remove(card);
            Debug.Log("Card removed from hand: " + card.cardName);
        }
    }

    public void ClearHand()
    {
        handCards.Clear();
        Debug.Log("Hand cleared");
    }
}