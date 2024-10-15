using DG.Tweening.Core.Easing;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IState
{
    void Enter();
    void Execute();
    void Exit();
}

public class SelectableState : IState
{
    private GameManager gameManager;

    public void Enter()
    {
        
    }

    public void Execute()
    {
        //throw new System.NotImplementedException();
    }

    public void Exit()
    {
        //throw new System.NotImplementedException();
    }
}


public class CombatState : IState
{
    private GameManager gameManager;
    private List<GameObject> tempCanMoveList = new List<GameObject>();
    public CombatState(GameManager gameManager)
    {
        this.gameManager = gameManager;
    }

    public void Enter()
    {
        Debug.Log("Entered Combat State");

        




        gameManager.isCombat = true;
        // Ư�� UI ��Ȱ��ȭ
        gameManager.MapLoader.HideNonCombatUI();
        //gameManager.MapLoader.DownCanvas.SetActive(true);
        // ���� ����
        gameManager.SpawnMonsters();
        gameManager.InitMana();
        gameManager.turnSystem.StartBattle();
        foreach (GameObject canMoveObj in gameManager.mapManager.canMoveList)
        {
            
            tempCanMoveList.Add(canMoveObj);
            
        }

        // ���ŵ� ������Ʈ���� canMoveList���� ����
        foreach (GameObject removedObj in tempCanMoveList)
        {
            gameManager.mapManager.canMoveList.Remove(removedObj);
        }

        //���⼭ �ӽ÷� canMoveList��ŭ�� ����� ���� �ӽ� ����Ʈ�� ���� ������ ���Ҵٰ�
    }

    public void Execute()
    {
        //gameManager.mapManager.canMoveList;
        
        
    }

    public void Exit()
    {
        Debug.Log("Exited Combat State");

        foreach (GameObject removedObj in tempCanMoveList)
        {
            gameManager.mapManager.canMoveList.Add(removedObj);
        }

        gameManager.isCombat = false;
        gameManager.MapLoader.DownCanvas.SetActive(false);
        
        


        tempCanMoveList.Clear();

        //���� ���°� ���� �Ǹ� canMoveList�� �ӽ÷� ������ ���� ����Ʈ�� �ٽ� �����ش�.

        // ���� ���� ���� �� UI ���� �� �ʿ��� �۾� ����
    }

}

public class RandomState : IState
{
    private MapStateMachine stateMachine;
    private GameManager gameManager;
    public RandomState(MapStateMachine stateMachine, GameManager gameManager)
    {
        this.stateMachine = stateMachine;
        this.gameManager = gameManager;
    }

    public void Enter()
    {
        Debug.Log("Entered Random State");

        // ������ ����
        float randomValue = Random.Range(0f, 1f);

        // Ȯ���� ���� ���� ��ȯ
        if (randomValue <= 0.10f) // 10% Ȯ���� CombatState�� ��ȯ
        {
            stateMachine.ChangeState(new CombatState(gameManager));
        }
        else if (randomValue <= 0.15f) // 5% Ȯ���� ShopState�� ��ȯ
        {
            stateMachine.ChangeState(new ShopState());
        }
        else
        {
            // ������ ��� �ٸ� ���� ����
            Debug.Log("Remaining in Random State");
        }
    }

    public void Execute()
    {
        // ���� ���� ���� ����� �ڵ�
    }

    public void Exit()
    {
        Debug.Log("Exited Random State");
        // ���� ���¸� ������ �� ����� �ڵ�
    }
}

public class ShopState : IState
{
    public void Enter()
    {
        Debug.Log("Entered Shop State");
        // ���� ���¿� ������ �� ����� �ڵ�
    }

    public void Execute()
    {
        // ���� ���� ���� ����� �ڵ�
    }

    public void Exit()
    {
        Debug.Log("Exited Shop State");
        // ���� ���¸� ������ �� ����� �ڵ�
    }
}

public class RestState : IState
{
    public void Enter()
    {
        Debug.Log("Entered Rest State");
        // �޽� ���¿� ������ �� ����� �ڵ�
    }

    public void Execute()
    {
        // �޽� ���� ���� ����� �ڵ�
    }

    public void Exit()
    {
        Debug.Log("Exited Rest State");
        // �޽� ���¸� ������ �� ����� �ڵ�
    }
}

public class ChestState : IState
{
    public void Enter()
    {
        Debug.Log("Entered Chest State");
        // ���� ���¿� ������ �� ����� �ڵ�
    }

    public void Execute()
    {
        // ���� ���� ���� ����� �ڵ�
    }

    public void Exit()
    {
        Debug.Log("Exited Chest State");
        // ���� ���¸� ������ �� ����� �ڵ�
    }
}

public class EliteState : IState
{
    private GameManager gameManager;
    private List<GameObject> tempCanMoveList = new List<GameObject>();
    public EliteState(GameManager gameManager)
    {
        this.gameManager = gameManager;
    }
    public void Enter()
    {
        Debug.Log("Entered Combat State");

        // Ư�� UI ��Ȱ��ȭ
        gameManager.isCombat = true;
        gameManager.MapLoader.HideNonCombatUI();

        // ���� ����
        gameManager.SpawnEliteMonsters();
        gameManager.InitMana();
        gameManager.turnSystem.StartBattle();

        foreach (GameObject canMoveObj in gameManager.mapManager.canMoveList)
        {

            tempCanMoveList.Add(canMoveObj);

        }

        // ���ŵ� ������Ʈ���� canMoveList���� ����
        foreach (GameObject removedObj in tempCanMoveList)
        {
            gameManager.mapManager.canMoveList.Remove(removedObj);
        }

        //���⼭ �ӽ÷� canMoveList��ŭ�� ����� ���� �ӽ� ����Ʈ�� ���� ������ ���Ҵٰ�
    }

    public void Execute()
    {
        // ����Ʈ ���� ���� ����� �ڵ�
    }

    public void Exit()
    {
        Debug.Log("Exited Elite State");
        
        foreach (GameObject removedObj in tempCanMoveList)
        {
            gameManager.mapManager.canMoveList.Add(removedObj);
        }

        gameManager.isCombat = false;
        gameManager.MapLoader.DownCanvas.SetActive(false);
        tempCanMoveList.Clear();
        // ����Ʈ ���¸� ������ �� ����� �ڵ�

        
    }
}






public class BossState : IState
{
    private GameManager gameManager;
    private List<GameObject> tempCanMoveList = new List<GameObject>();
    public BossState(GameManager gameManager)
    {
        this.gameManager = gameManager;
    }
    public void Enter()
    {
        Debug.Log("Entered Combat State");

        gameManager.isCombat = true;
        // Ư�� UI ��Ȱ��ȭ
        gameManager.MapLoader.HideNonCombatUI();

        // ���� ����
        gameManager.SpawnBossMonsters();
        gameManager.InitMana();
        gameManager.turnSystem.StartBattle();


        foreach (GameObject canMoveObj in gameManager.mapManager.canMoveList)
        {

            tempCanMoveList.Add(canMoveObj);

        }

        // ���ŵ� ������Ʈ���� canMoveList���� ����
        foreach (GameObject removedObj in tempCanMoveList)
        {
            gameManager.mapManager.canMoveList.Remove(removedObj);
        }

        //���⼭ �ӽ÷� canMoveList��ŭ�� ����� ���� �ӽ� ����Ʈ�� ���� ������ ���Ҵٰ�
    }

    public void Execute()
    {
        // ���� ���� ���� ����� �ڵ�
    }

    public void Exit()
    {
        
        

        Debug.Log("Exited Boss State");
        // ���� ���¸� ������ �� ����� �ڵ�
    }
}

public class MapStateMachine
{
    private IState currentState;

    public void ChangeState(IState newState)
    {
        if (currentState != null)
        {
            currentState.Exit();
        }

        currentState = newState;

        if (currentState != null)
        {
            currentState.Enter();
        }
    }

}




public class MapInterface : MonoBehaviour
{
     
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
