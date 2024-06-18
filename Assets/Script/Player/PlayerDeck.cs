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

    public RectTransform leftHandTr;
    public RectTransform RightHandTr;

    public int baseDraw = 5;

    public Text DeckCountText;
    
    


    void Start()
    {

        //PlayerDeckManager.Instance.InitializeDeck();
        //Deck.Add(CardDatabase.cardList[1]); // ���÷� �� ��° ī�带 ���������� �߰�


        
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
                DiscardCard(playerHand.handCards[0]);

            }
            else
            {
                Debug.Log("��");
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

            
            cardObject.SetActive(false); // ī�� ������Ʈ�� ��Ȱ��ȭ
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
            RectTransform playerHandTransform = playerHand.GetComponent<RectTransform>();

            // Assuming drawnCard is the GameObject representing the card
            GameObject drawnCard = Deck[0]; // Get the card GameObject from Deck list
            Deck.RemoveAt(0);
            drawnCard.SetActive(true);
            playerHand.AddCard(drawnCard);

            //playerHand.
            //playerHand
            RectTransform drawnCardRectTransform = drawnCard.GetComponent<RectTransform>();
            drawnCardRectTransform.SetParent(playerHandTransform);
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
        List<RectTransform> originalRectTransforms = RoundAlignmentUI(RightHandTr, leftHandTr, playerHand.handCards.Count, 0.5f);

        // ī�� �̵� �� ȸ�� �ִϸ��̼� ����
        for (int i = 0; i < playerHand.handCards.Count; i++)
        {
            GameObject targetCard = playerHand.handCards[i];
            RectTransform targetRectTransform = targetCard.GetComponent<RectTransform>();

            // ī�� ��ġ�� ȸ�� ����
            targetRectTransform.anchoredPosition = originalRectTransforms[i].anchoredPosition;
            targetRectTransform.localRotation = originalRectTransforms[i].localRotation;

            // �ִϸ��̼� ����
            targetRectTransform.DOAnchorPos(targetRectTransform.anchoredPosition, 1f).OnComplete(() =>
            {
                Debug.Log("Card movement complete!");

                // �̵��� �Ϸ�� �Ŀ� �ٸ� �۾��� �����ϰų� �ʿ��� ó���� ���⿡ �߰��� �� �ֽ��ϴ�.
            });

            targetRectTransform.DOLocalRotateQuaternion(targetRectTransform.localRotation, 1f);
        }
    }

    List<RectTransform> RoundAlignmentUI(RectTransform leftHandTr, RectTransform rightHandTr, int cardCount, float height)
    {
        float[] objLerps = new float[cardCount];
        List<RectTransform> results = new List<RectTransform>(cardCount);

        switch (cardCount)
        {
            case 1: objLerps = new float[] { 0.5f }; break;
            case 2: objLerps = new float[] { 0.27f, 0.73f }; break;
            case 3: objLerps = new float[] { 0.1f, 0.5f, 0.9f }; break;
            default:
                float interval = 1f / (cardCount - 1);
                for (int i = 0; i < cardCount; i++)
                {
                    objLerps[i] = interval * i;
                }
                break;
        }

        for (int i = 0; i < cardCount; i++)
        {
            var targetPos = Vector2.Lerp(leftHandTr.anchoredPosition, rightHandTr.anchoredPosition, objLerps[i]);
            var targetRot = Quaternion.identity;

            if (cardCount >= 4)
            {
                float curve = Mathf.Sqrt(Mathf.Pow(height, 2) - Mathf.Pow(objLerps[i] - 0.5f, 2));
                curve = height >= 0 ? curve : -curve;
                curve *= 200;
                targetPos.y += curve; // �Ʒ������� ��ġ�Ǵ� ��� �������� ��ġ�ǵ��� ������ ����
                targetRot = Quaternion.Euler(0, 0, Mathf.Lerp(-20, 20, objLerps[i])); // ī�� ȸ�� ������ �ݴ�� ����
            }

            RectTransform newRectTransform = new GameObject("Object" + i).AddComponent<RectTransform>();
            newRectTransform.anchoredPosition = targetPos;
            newRectTransform.localRotation = targetRot;

            results.Add(newRectTransform);
        }

        return results;
    }






    public void DrawInitialHand()
    {
        for (int i = 0; i < baseDraw; i++)
        {
            DrawCard();
            //cardAllignment();
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
        card.transform.SetParent(playerDiscard.transform);
        card.transform.localPosition = Vector3.zero;
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

    #region MyCard

    public void CardMouseOver(Card card)
    {
        print("CardMouseOver");
    }


    public void CardMouseExit(Card card)
    {
        print("CardMouseExit");
    }

    #endregion


}
