using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;


public class PlayerHand : MonoBehaviour
{
    public List<GameObject> handCards = new List<GameObject>();
    public int maxHandSize = 10;
    



    public void AddCard(GameObject card)
    {
        
           
        handCards.Add(card);
        Debug.Log(handCards.Count);
           
        //Debug.Log("Card added to hand: " + card.cardName);
        
    }

    public void RemoveCard(GameObject card)
    {
        if (handCards.Contains(card))
        {
            handCards.Remove(card);
            //Debug.Log("Card removed from hand: " + card.cardName);
        }
    }

    public void ClearHand()
    {
        handCards.Clear();
        Debug.Log("Hand cleared");
    }

    

}