using UnityEngine;
using UnityEngine.UIElements;

public class GameManager : Character
{
    public static GameManager instance;

    public CharacterData playerData;
    public PlayerUIManager playerUIManager;
    public PlayerDeckManager playerDeckManager;
    public CreateNode mapManager;
    public MonsterManager monsterManager;
    public SceneLoader MapLoader;
    public TurnSystem turnSystem;

    

    private MapStateMachine mapStateMachine;
    public MapStateMachine MapStateMachine => mapStateMachine;
    
    private int currentGold;
    private int currentFloor;
    private int currentDeckCount;
    private int maxMana = 3;
    public int currentMana;
    public bool isCombat = false;

    public GameObject healthTextPrefab; // 체력 텍스트 프리팹

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        
        
        currentHealth = playerData.maxHealth;
        currentGold = playerData.startingGold;
        currentFloor = 0;
        currentMana = maxMana;
        currentDeckCount = playerDeckManager.Deck.Count;
        mapStateMachine = new MapStateMachine();
       
        SpawnPlayer();


        UpdateAllUI();
    }

    private void Update()
    {
        UpdateAllUI();

        if(Input.GetKeyDown(KeyCode.L))
        {
            monsterManager.InitMonster(2);
        }

    }

    public int GetCurrentFloor()
    {
        return currentFloor;
    }


    void SpawnPlayer()
    {
        GameObject playerObject = Instantiate(playerData.playerPrefab, new Vector3(-5, -1.5f, 0), Quaternion.identity);
        playerObject.name = "Player";
        

        // 플레이어 데이터 할당
        //player.data = playerData;
    }
    public void SpawnMonsters()
    {
        //monsterManager.
        int monsterCount = monsterManager.monsterPool.Count;

        
        monsterManager.InitMonster(Random.Range(0, monsterCount));



    }
    public void SpawnEliteMonsters()
    {
        int monsterCount = monsterManager.elitemonsterPool.Count;

        monsterManager.InitEliteMonster(Random.Range(0, monsterCount));
    }

    public void SpawnBossMonsters()
    {
        int monsterCount = monsterManager.bossmonsterPool.Count;

        monsterManager.InitBossMonster(Random.Range(0, monsterCount));
    }
    



    public void UpdateAllUI()
    {
        //playerUIManager.UpdateHealthText(player.GetCurrentHealth(), playerData.maxHealth);
        playerUIManager.UpdateGoldText(currentGold);
        playerUIManager.UpdateFloorText(currentFloor);
        playerUIManager.UpdateDeckText(currentDeckCount);
        playerUIManager.UpdateHealthText(currentHealth, playerData.maxHealth);
        turnSystem.UpdateManaText(maxMana, currentMana);
    }

    public void InitMana()
    {
        currentMana = maxMana;
    }

    public void IncreaseGold(int amount)
    {
        currentGold += amount;
        UpdateAllUI();
    }

    public void AdvanceFloor()
    {
        currentFloor++;
        UpdateAllUI();
    }

    public override void Attack(Character target)
    {
        //
    }

    public override void TakeDamage(int damage)
    {
        currentHealth -= (int)(damage * damageTakenMultiplier);
        Debug.Log($"{characterName} takes {damage * damageTakenMultiplier} damage. Current health: {currentHealth}");
        if (currentHealth <= 0)
        {
            Die();
        }
    }
    private void Die()
    {
        Debug.Log($"{characterName} has died.");


        

        //게임 오버 씬 출력
    }






}
