using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IState
{
    void Enter();
    void Execute();
    void Exit();
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

        // Ư�� UI ��Ȱ��ȭ
        gameManager.MapLoader.HideNonCombatUI();

        // ���� ����
        gameManager.SpawnMonsters();

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
        
        // ���� ���� ���� ����� �ڵ�
    }

    public void Exit()
    {
        Debug.Log("Exited Combat State");

        foreach (GameObject removedObj in tempCanMoveList)
        {
            gameManager.mapManager.canMoveList.Add(removedObj);
        }
        

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
    public void Enter()
    {
        Debug.Log("Entered Elite State");
        // ����Ʈ ���¿� ������ �� ����� �ڵ�
    }

    public void Execute()
    {
        // ����Ʈ ���� ���� ����� �ڵ�
    }

    public void Exit()
    {
        Debug.Log("Exited Elite State");
        // ����Ʈ ���¸� ������ �� ����� �ڵ�
    }
}

public class BossState : IState
{
    public void Enter()
    {
        Debug.Log("Entered Boss State");
        // ���� ���¿� ������ �� ����� �ڵ�
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

    public void Update()
    {
        if (currentState != null)
        {
            currentState.Execute();
        }
    }
}





public class MapInterface : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
