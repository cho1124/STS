using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public CharacterData playerData;
    public PlayerUIManager playerUIManager;



    
    //public MonsterData monsterData;

    private int currentHealth;
    private int currentGold;
    private int currentFloor;

    private void Awake()
    {
        // �ٸ� ������ ������ ������Ʈ�� �̹� �ִٸ� ���� �������� �ʵ��� ó��
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // ���� ����� �� �ı����� �ʵ��� ����
        }
        else
        {
            Destroy(gameObject); // �̹� �ִ� ��� �� ������Ʈ�� �ı�
        }
    }

    private void Start()
    {
        currentHealth = playerData.maxHealth;
        currentGold = playerData.startingGold;

        

    }

    private void Update()
    {
        UpdateAllUI();
    }

    void UpdateAllUI()
    {
        playerUIManager.UpdateHealthText(currentHealth, playerData.maxHealth);
        playerUIManager.UpdateGoldText(currentGold);
        playerUIManager.UpdateFloorText(currentFloor);
    }


}
