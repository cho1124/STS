using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerDeck : MonoBehaviour
{
    public List<Card> Deck = new List<Card>();
    public int startDeckSize;

    public PlayerHand playerHand;
    public PlayerDiscard playerDiscard;

    public int baseDraw = 5;

    void Start()
    {

        for (int i = 0; i < 5; i++)
        {
            Deck.Add(CardDatabase.cardList[0]); // 예시로 첫 번째 카드를 5번 추가
        }

        for (int i = 5; i < 9; i++)
        {
            Deck.Add(CardDatabase.cardList[10]); // 예시로 열 번째 카드를 4번 추가
        }

        Deck.Add(CardDatabase.cardList[1]); // 예시로 두 번째 카드를 마지막으로 추가

        Shuffle();
        DrawInitialHand();
    }

    

    public void Shuffle()
    {
        // Create a new random number generator
        System.Random rng = new System.Random();

        int n = Deck.Count;
        while (n > 1)
        {
            n--;
            int k = rng.Next(n + 1); // Generate a random index between 0 and n (inclusive)
                                     // Swap elements at indices k and n
            Card temp = Deck[k];
            Deck[k] = Deck[n];
            Deck[n] = temp;
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
            Card drawnCard = Deck[0];
            Deck.RemoveAt(0);
            playerHand.AddCard(drawnCard);
        }
        else
        {
            Debug.Log("Deck is empty and discard pile has no cards!");
        }
    }

    public void DrawInitialHand()
    {
        for (int i = 0; i < baseDraw; i++)
        {
            DrawCard();
        }
    }

    public void DiscardCard(Card card)
    {
        playerHand.RemoveCard(card);
        playerDiscard.AddToDiscardPile(card);
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
