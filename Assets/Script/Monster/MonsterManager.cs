using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System;

[System.Serializable]
public class MonsterData
{
    public string name;
    public int maxhealth;
    public int minhealth;
    public int attack;
    public int type;
}

[System.Serializable]
public class Monsters
{
    public List<MonsterData> MonsterList;
}


public class MonsterManager : MonoBehaviour
{
    
    private MonsterLoader monsterLoader;

    public List<GameObject> monsterPool;

    public List<GameObject> elitemonsterPool;

    public List<GameObject> bossmonsterPool;

    public List<GameObject> inCombatList;


    void Start()
    {
        // MonsterLoader 컴포넌트를 가져옵니다
        monsterLoader = GetComponent<MonsterLoader>();
        monsterPool = new List<GameObject>();
        elitemonsterPool = new List<GameObject>();
        bossmonsterPool = new List<GameObject>();
        inCombatList = new List<GameObject>();
        // 몬스터 데이터를 불러옵니다
        Monsters monstersList = monsterLoader.LoadMonsterData();

        int countToExclude = 4;
        int validCount = monstersList.MonsterList.Count - countToExclude;
        List<MonsterData> validMonstersList = monstersList.MonsterList.Take(validCount).ToList();
        List<MonsterData> eliteMonsterList  = monstersList.MonsterList.GetRange(validCount, countToExclude - 1);
        List<MonsterData> bossMonsterList = monstersList.MonsterList.GetRange(monstersList.MonsterList.Count - 1, 1); //현재 보스 몬스터가 한개라 매직넘버 사용
        if (monstersList != null)
        {
            
            foreach (MonsterData data in validMonstersList)
            {
                SpawnMonster(data);
            }
            foreach(MonsterData data in eliteMonsterList)
            {
                SpawnEliteMonster(data);
            }
            foreach(MonsterData data in bossMonsterList)
            {
                SpawnBossMonster(data);
            }
        }
        else
        {
            Debug.Log("error!");
        }
    }

    private void SpawnBossMonster(MonsterData data)
    {
        string prefabName = data.name;
        GameObject prefab = Resources.Load<GameObject>("Prefabs/" + prefabName);

        if (prefab != null)
        {
            GameObject monsterObject = Instantiate(prefab, transform);
            Monster monsterComponent = monsterObject.GetComponent<Monster>();

            if (monsterComponent != null)
            {
                monsterComponent.Initialize(data); // 몬스터 데이터를 초기화합니다
                monsterObject.transform.SetParent(transform);
                monsterObject.SetActive(false); // 초기에 비활성화 상태로 설정합니다
                bossmonsterPool.Add(monsterObject);                           // 추가적으로 필요한 초기화 작업을 수행할 수 있습니다

            }
            else
            {
                Debug.LogError("Monster 스크립트를 프리팹에서 찾을 수 없습니다.");
                Destroy(monsterObject); // 오류 발생 시 인스턴스를 삭제합니다
            }
        }
    }

    void SpawnMonster(MonsterData data)
    {
        string prefabName = data.name;
        GameObject prefab = Resources.Load<GameObject>("Prefabs/" + prefabName);

        if (prefab != null)
        {
            for (int i = 0; i < 3; i++)
            {
                GameObject monsterObject = Instantiate(prefab, transform);
                Monster monsterComponent = monsterObject.GetComponent<Monster>();

                if (monsterComponent != null)
                {
                    monsterComponent.Initialize(data); // 몬스터 데이터를 초기화합니다
                    monsterObject.transform.SetParent(transform);
                    monsterObject.SetActive(false); // 초기에 비활성화 상태로 설정합니다
                    monsterPool.Add(monsterObject);                           // 추가적으로 필요한 초기화 작업을 수행할 수 있습니다

                }
                else
                {
                    Debug.LogError("Monster 스크립트를 프리팹에서 찾을 수 없습니다.");
                    Destroy(monsterObject); // 오류 발생 시 인스턴스를 삭제합니다
                }
            }
        }
        else
        {
            Debug.LogError("프리팹을 찾을 수 없습니다: " + prefabName);
        }
    }

    void SpawnEliteMonster(MonsterData data)
    {
        string prefabName = data.name;
        GameObject prefab = Resources.Load<GameObject>("Prefabs/" + prefabName);

        if(prefab != null)
        {
            GameObject monsterObject = Instantiate(prefab, transform);
            Monster monsterComponent = monsterObject.GetComponent<Monster>();

            if (monsterComponent != null)
            {
                monsterComponent.Initialize(data); // 몬스터 데이터를 초기화합니다
                monsterObject.transform.SetParent(transform);
                monsterObject.SetActive(false); // 초기에 비활성화 상태로 설정합니다
                elitemonsterPool.Add(monsterObject);                           // 추가적으로 필요한 초기화 작업을 수행할 수 있습니다

            }
            else
            {
                Debug.LogError("Monster 스크립트를 프리팹에서 찾을 수 없습니다.");
                Destroy(monsterObject); // 오류 발생 시 인스턴스를 삭제합니다
            }
        }
    }


    

    public void InitMonster(int i)
    {
        monsterPool[i].SetActive(true);
        monsterPool[i].transform.position = new Vector3(2f, -1.5f, 0f); //이 하드 코딩된 위치값은 combat stage라는 지역을 만들어서 그곳에 넣어도 되긴 할듯 -> combat stage list들을 넣어서 그 자리가 비어 있다면 넣기
        inCombatList.Add(monsterPool[i]);
    }
    public void InitEliteMonster(int i)
    {
        elitemonsterPool[i].SetActive(true);
        elitemonsterPool[i].transform.position = new Vector3(2f, -1.5f, 0f);
        inCombatList.Add(elitemonsterPool[i]);

        
    }
    public void InitBossMonster(int i )
    {
        bossmonsterPool[i].SetActive(true);
        bossmonsterPool[i].transform.position = new Vector3(2f, -1.5f, 0f);
        inCombatList.Add(bossmonsterPool[i]);
    }


}
