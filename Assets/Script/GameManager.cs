using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public CharacterData playerData;
    public PlayerUIManager playerUIManager;

    public Player player;
    public BattleManager battleManager;

    private int currentHealth;
    private int currentGold;
    private int currentFloor;

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

        player = FindObjectOfType<Player>();
        //player.data = playerData;

        UpdateAllUI();
    }

    private void Update()
    {
        UpdateAllUI();
    }

    void SpawnPlayer()
    {
        GameObject playerObject = Instantiate(playerData.playerPrefab, Vector3.zero, Quaternion.identity);
        playerObject.name = "Player";
        player = playerObject.GetComponent<Player>();

        // 플레이어 데이터 할당
        //player.data = playerData;
    }

    public void UpdateAllUI()
    {
        //playerUIManager.UpdateHealthText(player.GetCurrentHealth(), playerData.maxHealth);
        playerUIManager.UpdateGoldText(currentGold);
        playerUIManager.UpdateFloorText(currentFloor);
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
