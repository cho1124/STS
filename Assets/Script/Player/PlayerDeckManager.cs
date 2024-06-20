using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDeckManager : MonoBehaviour
{
    private static PlayerDeckManager instance;
    public static PlayerDeckManager Instance { get { return instance; } }

    public List<Card> Deck = new List<Card>();

    private void Awake()
    {
        // 인스턴스가 없으면 현재 객체를 인스턴스로 설정
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // 씬 전환 시 파괴되지 않도록 설정
        }
        else
        {
            // 이미 인스턴스가 있는 경우, 중복된 객체가 되지 않도록 현재 객체는 파괴
            Debug.LogWarning("Trying to instantiate a second instance of singleton class PlayerDeckManager.");
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        InitializeDeck(); // 게임 시작 시 덱 초기화
    }


    // 플레이어 덱 초기화하는 메서드
    public void InitializeDeck()
    {
        Deck.Clear();
        int a = Random.Range(0, 23);

        // 여기서 플레이어 덱을 초기화하거나 불러오는 코드를 작성
        // 예를 들어, Deck.Add(CardDatabase.cardList[0]); 같은 코드들을 여기서 사용할 수 있음
        for (int i = 0; i < 5; i++)
        {
            a = Random.Range(0, 23);
            Deck.Add(CardDatabase.cardList[a]);
        }

        for (int i = 0; i < 5; i++)
        {
            Deck.Add(CardDatabase.cardList[10]);
        }
        for(int i = 0; i < 5; i++)
        {
            Deck.Add(CardDatabase.cardList[3]);
        }

        Deck.Add(CardDatabase.cardList[1]);
    }

    // 덱을 반환하는 메서드
    public List<Card> GetDeck()
    {
        return Deck;
    }

    // 추가적으로 필요한 다른 메서드들을 구현할 수 있음

    private void OnDestroy()
    {
        if (instance == this)
        {
            instance = null;
        }
    }
}
