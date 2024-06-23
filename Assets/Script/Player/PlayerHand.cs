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
        // handCards 리스트를 비웁니다.
        handCards.Clear();
        Debug.Log("Hand cleared");

        // 자식 오브젝트 중 이름이 "Card"인 오브젝트들을 제거합니다.
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



}