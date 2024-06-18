using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class PlayerDeck : MonoBehaviour
{
    
    public List<GameObject> Deck = new List<GameObject>();
    public int startDeckSize;

    public PlayerHand playerHand;
    public PlayerDiscard playerDiscard;

    public GameObject cardPrefab;

    public Transform leftHandTr;
    public Transform RightHandTr;

    public int baseDraw = 5;

    public Text DeckCountText;
    



    void Start()
    {

        //PlayerDeckManager.Instance.InitializeDeck();
        //Deck.Add(CardDatabase.cardList[1]); // 예시로 두 번째 카드를 마지막으로 추가

        

        MakeCard();
        Shuffle();
        DrawInitialHand();
    }


    private void Update()
    {
        if(Input.GetKeyDown("a"))
        {
            DrawCard();
        }
        if(Input.GetKeyDown("s"))
        {

            if(playerHand.handCards.Count > 0)
            {
                DiscardCard(playerHand.handCards[playerHand.handCards.Count - 1]);

            }
            else
            {
                Debug.Log("ㅋ");
            }
        }
        UpdateDeckCount();
        playerDiscard.UpdatediscardCount();
    }

    public void MakeCard()
    {
        for (int i = 0; i < PlayerDeckManager.Instance.Deck.Count; i++)
        {
            GameObject cardObject = Instantiate(cardPrefab , transform);
            cardObject.GetComponent<ThisCard>().thisId = PlayerDeckManager.Instance.Deck[i].id;

            
            cardObject.SetActive(false); // 카드 오브젝트를 비활성화
            Deck.Add(cardObject);
        }
    }

    void UpdateDeckCount()
    {
        DeckCountText.text = $"{Deck.Count}";
    }


    public void ActiveCard()
    {

    }

    public void NotActivecard()
    {

    }


    public void Shuffle()
    {
        for (int i = 0; i < Deck.Count; i++)
        {
            int randomIndex = Random.Range(i, Deck.Count); // Generate a random index between i and Deck.Count - 1
            GameObject temp = Deck[randomIndex];
            Deck[randomIndex] = Deck[i];
            Deck[i] = temp;
        }
    }
    public void DrawCard()
    {
        if (Deck.Count == 0)
        {
            ReshuffleDiscardPileIntoDeck();
        }

        if (Deck.Count > 0)
        {
            Transform playerHandTransform = playerHand.transform;

            // Assuming drawnCard is the GameObject representing the card
            GameObject drawnCard = Deck[0]; // Get the card GameObject from Deck list
            drawnCard.SetActive(true);
            Deck.RemoveAt(0);
            playerHand.AddCard(drawnCard);

            //playerHand.
            //playerHand
            Debug.Log(playerHand.transform.rotation);
            drawnCard.transform.SetParent(playerHandTransform); // Use false to keep the local position unchanged
            //drawnCard.transform.DOMove(playerHand.transform.position, 1f);
            //drawnCard.transform.DORotate(playerHand.transform.rotation, 1f);
            // Set the parent to playerHandTransform

            cardAllignment();


            // Set the position of the card relative to playerHandTransform

            //drawnCard.SetActive(true); // Activate the card
        }
        else
        {
            Debug.Log("Deck is empty and discard pile has no cards!");
        }
    }

    void cardAllignment()
    {
        List<Transform> orginalTransform = new List<Transform>();

        orginalTransform = RoundAlignment(leftHandTr, RightHandTr, playerHand.handCards.Count, 0.5f);


        for (int i = 0; i < playerHand.handCards.Count; i++)
        {
            GameObject targetCard = playerHand.handCards[i];
            targetCard.transform.position = orginalTransform[i].position;
            targetCard.transform.rotation = orginalTransform[i].rotation;

            targetCard.transform.DOMove(targetCard.transform.position, 1f);
            targetCard.transform.DORotateQuaternion(targetCard.transform.rotation, 1f);


        }



    }
   
  
    List<Transform> RoundAlignment(Transform righthand, Transform lefthand, int objcount, float height)
    {
        float[] objLerps = new float[objcount];
        List<Transform> results = new List<Transform>(objcount);


        switch (objcount)
        {
            case 1: objLerps = new float[] { 0.5f }; break;
            case 2: objLerps = new float[] { 0.27f, 0.73f }; break;
            case 3: objLerps = new float[] { 0.1f, 0.5f, 0.9f }; break;
            default:
                float interval = 1f / (objcount - 1);
                for (int i = 0; i < objcount; i++)
                {
                    objLerps[i] = interval * i;
                }
                break;

        }

        for (int i = 0; i < objcount; i++)
        {
            var targetPos = Vector2.Lerp(lefthand.position, righthand.position, objLerps[i]);
            var targetRot = Quaternion.identity;

            if (objcount >= 4)
            {
                float curve = Mathf.Sqrt(Mathf.Pow(height, 2) - Mathf.Pow(objLerps[i] - 0.5f, 2));
                curve = height >= 0 ? curve : -curve;
                targetPos.y += curve;
                targetRot = Quaternion.Slerp(lefthand.rotation, righthand.rotation, objLerps[i]);
            }


            Transform newTransform = new GameObject("Object" + i).transform;
            newTransform.position = targetPos;
            newTransform.rotation = targetRot;

            results.Add(newTransform);


        }

        return results;




    }



    public void DrawInitialHand()
    {
        for (int i = 0; i < baseDraw; i++)
        {
            DrawCard();
            cardAllignment();
        }
    }

    public void DiscardCard(GameObject card)
    {
        if(playerHand.handCards.Count == 0)
        {
            
            return;
        }

        playerHand.RemoveCard(card);
        playerDiscard.AddToDiscardPile(card);
        card.SetActive(false);
        cardAllignment();
    }

    private void ReshuffleDiscardPileIntoDeck()
    {
        if (playerDiscard.discardedCards.Count > 0)
        {
            Deck.AddRange(playerDiscard.discardedCards);
            playerDiscard.ClearDiscardPile();
            Shuffle();
            Debug.Log("Reshuffled discard pile into deck.");
        }
    }




}
