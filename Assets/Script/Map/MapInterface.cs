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
        // 특정 UI 비활성화
        gameManager.MapLoader.HideNonCombatUI();
        //gameManager.MapLoader.DownCanvas.SetActive(true);
        // 몬스터 생성
        gameManager.SpawnMonsters();
        gameManager.InitMana();
        gameManager.turnSystem.StartBattle();
        foreach (GameObject canMoveObj in gameManager.mapManager.canMoveList)
        {
            
            tempCanMoveList.Add(canMoveObj);
            
        }

        // 제거된 오브젝트들을 canMoveList에서 삭제
        foreach (GameObject removedObj in tempCanMoveList)
        {
            gameManager.mapManager.canMoveList.Remove(removedObj);
        }

        //여기서 임시로 canMoveList만큼의 사이즈를 가진 임시 리스트를 만들어서 저장해 놓았다가
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

        //전투 상태가 종료 되면 canMoveList에 임시로 저장해 놓은 리스트를 다시 돌려준다.

        // 전투 상태 종료 시 UI 복구 등 필요한 작업 수행
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

        // 난수를 생성
        float randomValue = Random.Range(0f, 1f);

        // 확률에 따라 상태 전환
        if (randomValue <= 0.10f) // 10% 확률로 CombatState로 전환
        {
            stateMachine.ChangeState(new CombatState(gameManager));
        }
        else if (randomValue <= 0.15f) // 5% 확률로 ShopState로 전환
        {
            stateMachine.ChangeState(new ShopState());
        }
        else
        {
            // 나머지 경우 다른 로직 실행
            Debug.Log("Remaining in Random State");
        }
    }

    public void Execute()
    {
        // 랜덤 상태 동안 실행될 코드
    }

    public void Exit()
    {
        Debug.Log("Exited Random State");
        // 랜덤 상태를 종료할 때 실행될 코드
    }
}

public class ShopState : IState
{
    public void Enter()
    {
        Debug.Log("Entered Shop State");
        // 상점 상태에 진입할 때 실행될 코드
    }

    public void Execute()
    {
        // 상점 상태 동안 실행될 코드
    }

    public void Exit()
    {
        Debug.Log("Exited Shop State");
        // 상점 상태를 종료할 때 실행될 코드
    }
}

public class RestState : IState
{
    public void Enter()
    {
        Debug.Log("Entered Rest State");
        // 휴식 상태에 진입할 때 실행될 코드
    }

    public void Execute()
    {
        // 휴식 상태 동안 실행될 코드
    }

    public void Exit()
    {
        Debug.Log("Exited Rest State");
        // 휴식 상태를 종료할 때 실행될 코드
    }
}

public class ChestState : IState
{
    public void Enter()
    {
        Debug.Log("Entered Chest State");
        // 상자 상태에 진입할 때 실행될 코드
    }

    public void Execute()
    {
        // 상자 상태 동안 실행될 코드
    }

    public void Exit()
    {
        Debug.Log("Exited Chest State");
        // 상자 상태를 종료할 때 실행될 코드
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

        // 특정 UI 비활성화
        gameManager.isCombat = true;
        gameManager.MapLoader.HideNonCombatUI();

        // 몬스터 생성
        gameManager.SpawnEliteMonsters();
        gameManager.InitMana();
        gameManager.turnSystem.StartBattle();

        foreach (GameObject canMoveObj in gameManager.mapManager.canMoveList)
        {

            tempCanMoveList.Add(canMoveObj);

        }

        // 제거된 오브젝트들을 canMoveList에서 삭제
        foreach (GameObject removedObj in tempCanMoveList)
        {
            gameManager.mapManager.canMoveList.Remove(removedObj);
        }

        //여기서 임시로 canMoveList만큼의 사이즈를 가진 임시 리스트를 만들어서 저장해 놓았다가
    }

    public void Execute()
    {
        // 엘리트 상태 동안 실행될 코드
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
        // 엘리트 상태를 종료할 때 실행될 코드

        
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
        // 특정 UI 비활성화
        gameManager.MapLoader.HideNonCombatUI();

        // 몬스터 생성
        gameManager.SpawnBossMonsters();
        gameManager.InitMana();
        gameManager.turnSystem.StartBattle();


        foreach (GameObject canMoveObj in gameManager.mapManager.canMoveList)
        {

            tempCanMoveList.Add(canMoveObj);

        }

        // 제거된 오브젝트들을 canMoveList에서 삭제
        foreach (GameObject removedObj in tempCanMoveList)
        {
            gameManager.mapManager.canMoveList.Remove(removedObj);
        }

        //여기서 임시로 canMoveList만큼의 사이즈를 가진 임시 리스트를 만들어서 저장해 놓았다가
    }

    public void Execute()
    {
        // 보스 상태 동안 실행될 코드
    }

    public void Exit()
    {
        
        

        Debug.Log("Exited Boss State");
        // 보스 상태를 종료할 때 실행될 코드
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
