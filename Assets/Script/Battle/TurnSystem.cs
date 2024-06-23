using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TurnSystem : MonoBehaviour
{
    public bool isMyTurn;
    public int myTurn;
    public PlayerDeck playerDeck;
    public Text turnText;
    public Text manaText;

    public GameObject myTurnImage;
    public GameObject enemyTurnImage;
    public GameObject startCombatImage;
    public Text turnCountText;

    public MonsterPatternHandler monsterPatternHandler;

    public List<Monster> monsters;
    //public Player player;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            if (isMyTurn)
            {
                EndMyTurn();
            }
            else
            {
                EndOpponentTurn();
            }
        }

        if(Input.GetKeyDown(KeyCode.K)) 
        {
            foreach(Monster monster in monsters)
            {
                monster.TakeDamage(300);
            }
        }

        if (isMyTurn)
        {
            turnText.text = "턴 종료";
        }
        else
        {
            turnText.text = "적의 턴";
        }
        //manaText.text = currentMana + "/" + maxMana;
    }


    public void StartBattle()
    {
        playerDeck = FindObjectOfType<PlayerDeck>();
        playerDeck.ClearDeck();
        
        playerDeck.ActiveCard();
        isMyTurn = true;
        myTurn = 1;
        //InitializeMonster();
        
        StartPlayerTurn();
        InitializeMonster();
        StartCoroutine(TurnUICo());
    }

    public void EndMyTurn()
    {
        isMyTurn = false;

        Debug.Log("1");
        if(GameManager.instance.isBaricate == false)
        {
            GameManager.instance.defense = 0;
        }
        Debug.Log("2");
        
        GameManager.instance.EndTurn();
        playerDeck.DiscardAll();

        StartCoroutine(TurnUICo());
        Debug.Log("3");
        StartMonsterTurn();
    }

    public void EndOpponentTurn()
    {
        isMyTurn = true;
        myTurn += 1;
        foreach(Monster monster in monsters)
        { 
            monster.EndTurn();
        }

        //monster.EndTurn();
        StartCoroutine(TurnUICo());
        StartPlayerTurn();
    }

    IEnumerator TurnUICo()
    {
        if (isMyTurn)
        {
            yield return new WaitForSeconds(0.5f);
            myTurnImage.SetActive(true);
            turnCountText.text = $"{myTurn}턴";
            yield return new WaitForSeconds(1.5f);
            myTurnImage.SetActive(false);
        }
        else
        {
            yield return new WaitForSeconds(0.5f);
            enemyTurnImage.SetActive(true);
            yield return new WaitForSeconds(1.5f);
            enemyTurnImage.SetActive(false);
        }
    }

    IEnumerator StartUICo()
    {

        startCombatImage.SetActive(true);
        
        yield return new WaitForSeconds(1.5f);
        startCombatImage.SetActive(false);

    }

    void StartPlayerTurn()
    {
        Debug.Log("Player's Turn Started");
        
        GameManager.instance.InitMana();
        playerDeck.DrawInitialHand();
        // 플레이어 턴 초기화 작업 추가
        //GameManager.instance.currentMana = 
    }

    void StartMonsterTurn()
    {
        //InitializeMonster();

        Debug.Log("Monster's Turn Started");
        
        foreach(Monster monster in monsters)
        {
            monster.Attack(GameManager.instance);
        }
        

        //monsterPatternHandler.ApplyPatterns();
        Invoke("EndOpponentTurn", 2.0f); // 2초 후 턴 전환
    }

    void InitializeMonster()
    {

        monsters = new List<Monster>(FindObjectsOfType<Monster>());

        foreach(Monster monster in monsters)
        {
            monster.OnMonsterDeath += HandleMonsterDeath;
        }



        if (monsters.Count == 0)
        {
            Debug.LogWarning("No monsters found in the scene!");
        }
        else
        {
            Debug.Log(monsters.Count + " monsters found in the scene.");
        }
    }

    public void UpdateManaText(int maxMana, int currentMana)
    {
        manaText.text = $"{currentMana}/{maxMana}";
    }

    private void HandleMonsterDeath(Monster monster)
    {
        monsters.Remove(monster);
        

        if(monsters.Count == 0)
        {
            GameManager.instance.MapStateMachine.ChangeState(new SelectableState());
        }


    }


}
