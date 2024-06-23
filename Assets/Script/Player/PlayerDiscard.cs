using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class PlayerDiscard : MonoBehaviour
{
    public List<GameObject> discardedCards = new List<GameObject>();
    public Text discardText;
    public void AddToDiscardPile(GameObject card)
    {
        discardedCards.Add(card);
        //Debug.Log("Card added to discard pile: " + card.cardName);
    }

    public void ClearDiscardPile()
    {
        discardedCards.Clear();
        List<GameObject> objectsToDestroy = new List<GameObject>();

        foreach (Transform child in transform)
        {
            if (child.gameObject.name.Contains("Card"))
            {
                objectsToDestroy.Add(child.gameObject);
            }
        }

        foreach (GameObject obj in objectsToDestroy)
        {
            Destroy(obj);
        }
    }

    public void UpdatediscardCount()
    {
        discardText.text = $"{discardedCards.Count}";
    }

}
