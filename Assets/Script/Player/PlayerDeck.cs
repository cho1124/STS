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

    public GameObject arrowPrefab;
    public GameObject selectedCard;
    public Canvas CombatCanvas;
    bool isMyCardDrag;

    public GameObject dotPrefab; // 작은 이미지(점) 프리팹
    public float dotSpacing = 0.5f; // 점 간의 간격

    private List<GameObject> dots = new List<GameObject>(); // 생성된 점들





    void Start()
    {

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
            RectTransform playerHandTransform = playerHand.GetComponent<RectTransform>();

            GameObject drawnCard = Deck[0]; // Get the card GameObject from Deck list
            Deck.RemoveAt(0);
            drawnCard.SetActive(true);
            playerHand.AddCard(drawnCard);

            if (playerHand.handCards.Count > playerHand.maxHandSize)
            {
                DiscardCard(drawnCard);
                return;
            }

            RectTransform drawnCardRectTransform = drawnCard.GetComponent<RectTransform>();
            drawnCardRectTransform.SetParent(playerHandTransform);

            cardAllignment();
        }
        else
        {
            // Deck is empty and discard pile has no cards
        }
    }

    void cardAllignment()
    {
        List<RectTransform> originalRectTransforms = RoundAlignmentUI(RightHandTr, leftHandTr, playerHand.handCards.Count, 0.5f);

        for (int i = 0; i < playerHand.handCards.Count; i++)
        {
            GameObject targetCard = playerHand.handCards[i];
            RectTransform targetRectTransform = targetCard.GetComponent<RectTransform>();

            // 카드 위치와 회전 설정
            targetRectTransform.anchoredPosition = originalRectTransforms[i].anchoredPosition;
            targetRectTransform.localRotation = originalRectTransforms[i].localRotation;

            // 애니메이션 적용
            targetRectTransform.DOAnchorPos(targetRectTransform.anchoredPosition, 1f).OnComplete(() =>
            {
                //Debug.Log("Card movement complete!");
                // 이동이 완료된 후에 다른 작업을 수행하거나 필요한 처리를 여기에 추가할 수 있습니다.
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
                targetPos.y += curve; // 아래쪽으로 배치되는 대신 위쪽으로 배치되도록 음수로 변경
                targetRot = Quaternion.Euler(0, 0, Mathf.Lerp(-20, 20, objLerps[i])); // 카드 회전 각도를 반대로 변경
            }

            // 기존에 생성된 RectTransform 사용
            RectTransform existingRectTransform = playerHand.handCards[i].GetComponent<RectTransform>();
            existingRectTransform.anchoredPosition = targetPos;
            existingRectTransform.localRotation = targetRot;

            results.Add(existingRectTransform);
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
            
            foreach(GameObject Discard in playerDiscard.discardedCards)
            {
                Discard.transform.SetParent(transform);
            }


            playerDiscard.ClearDiscardPile();
            Shuffle();
            Debug.Log("Reshuffled discard pile into deck.");
        }
    }

    #region MyCard

    public void EnlargeCard(GameObject cardObject, Vector3 originalScale,  Vector3 originalPosition,  Vector3 positionOffset, float scaleMultiplier,  RectTransform rectTransform)
    {

        selectedCard = cardObject;
        cardObject.transform.localScale = originalScale * scaleMultiplier; // 1.2f
        cardObject.transform.localPosition = originalPosition + positionOffset; // y로 150f , z로 -10f
        cardObject.transform.localRotation = Quaternion.identity;
        rectTransform.SetAsLastSibling();



    }

    // 카드 복원 메소드
    public void RestoreCard(GameObject cardObject, Vector3 originalScale, Vector3 originalPosition, Quaternion originalRotation, int originalSiblingIndex, RectTransform rectTransform)
    {
        selectedCard = null;
        cardObject.transform.localScale = originalScale;
        cardObject.transform.localPosition = originalPosition;
        cardObject.transform.localRotation = originalRotation;
        rectTransform.SetSiblingIndex(originalSiblingIndex);
    }

    public void CardMouseDown()
    {
        isMyCardDrag = false;
    }

    public void CardMouseUp()
    {
        isMyCardDrag = true;
    }



    #endregion


}
