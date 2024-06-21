using UnityEngine;
using System.Collections.Generic;


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

    public List<GameObject> inCombatList;


    void Start()
    {
        // MonsterLoader 컴포넌트를 가져옵니다
        monsterLoader = GetComponent<MonsterLoader>();
        monsterPool = new List<GameObject>();
        inCombatList = new List<GameObject>();
        // 몬스터 데이터를 불러옵니다
        Monsters monstersList = monsterLoader.LoadMonsterData();
        if (monstersList != null)
        {
            // 각 몬스터 데이터를 사용하여 몬스터를 스폰합니다
            foreach (MonsterData data in monstersList.MonsterList)
            {
                SpawnMonster(data);
            }
        }
        else
        {
            Debug.Log("error!");
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

    public void InitMonster(int i)
    {
        monsterPool[i].SetActive(true);
        monsterPool[i].transform.position = new Vector3(2f, -2f, 0f);
        inCombatList.Add(monsterPool[i]);
    }


}
