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
        // �ν��Ͻ��� ������ ���� ��ü�� �ν��Ͻ��� ����
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // �� ��ȯ �� �ı����� �ʵ��� ����
        }
        else
        {
            // �̹� �ν��Ͻ��� �ִ� ���, �ߺ��� ��ü�� ���� �ʵ��� ���� ��ü�� �ı�
            Debug.LogWarning("Trying to instantiate a second instance of singleton class PlayerDeckManager.");
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        InitializeDeck(); // ���� ���� �� �� �ʱ�ȭ
    }


    // �÷��̾� �� �ʱ�ȭ�ϴ� �޼���
    public void InitializeDeck()
    {
        Deck.Clear();

        // ���⼭ �÷��̾� ���� �ʱ�ȭ�ϰų� �ҷ����� �ڵ带 �ۼ�
        // ���� ���, Deck.Add(CardDatabase.cardList[0]); ���� �ڵ���� ���⼭ ����� �� ����
        for (int i = 0; i < 5; i++)
        {
            Deck.Add(CardDatabase.cardList[0]);
        }

        for (int i = 5; i < 9; i++)
        {
            Deck.Add(CardDatabase.cardList[10]);
        }

        Deck.Add(CardDatabase.cardList[1]);
    }

    // ���� ��ȯ�ϴ� �޼���
    public List<Card> GetDeck()
    {
        return Deck;
    }

    // �߰������� �ʿ��� �ٸ� �޼������ ������ �� ����

    private void OnDestroy()
    {
        if (instance == this)
        {
            instance = null;
        }
    }
}
