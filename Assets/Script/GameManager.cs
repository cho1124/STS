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
        // 다른 씬에서 동일한 오브젝트가 이미 있다면 새로 생성되지 않도록 처리
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // 씬이 변경될 때 파괴되지 않도록 설정
        }
        else
        {
            Destroy(gameObject); // 이미 있는 경우 이 오브젝트는 파괴
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
