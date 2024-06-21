using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public CharacterData playerData;
    public PlayerUIManager playerUIManager;
    public PlayerDeckManager playerDeckManager;
    public CreateNode mapManager;
    public MonsterManager monsterManager;
    public SceneLoader MapLoader;

    public Player player;

    private MapStateMachine mapStateMachine;
    public MapStateMachine MapStateMachine => mapStateMachine;
    private int currentHealth;
    private int currentGold;
    private int currentFloor;
    private int currentDeckCount;
    private int maxMana = 3;
    public int currentMana;


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
        


        player = FindObjectOfType<Player>();
        //player.data = playerData;
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

    void SpawnPlayer()
    {
        GameObject playerObject = Instantiate(playerData.playerPrefab, new Vector3(-5, -1.5f, 0), Quaternion.identity);
        playerObject.name = "Player";
        player = playerObject.GetComponent<Player>();

        // 플레이어 데이터 할당
        //player.data = playerData;
    }
    public void SpawnMonsters()
    {
        //monsterManager.
    }


    public void UpdateAllUI()
    {
        //playerUIManager.UpdateHealthText(player.GetCurrentHealth(), playerData.maxHealth);
        playerUIManager.UpdateGoldText(currentGold);
        playerUIManager.UpdateFloorText(currentFloor);
        playerUIManager.UpdateDeckText(currentDeckCount);
        playerUIManager.UpdateHealthText(currentHealth, playerData.maxHealth);
        playerUIManager.UpdateManaText(maxMana, currentMana);
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

    

}
