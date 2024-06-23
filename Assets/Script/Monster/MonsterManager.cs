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
        // MonsterLoader ������Ʈ�� �����ɴϴ�
        monsterLoader = GetComponent<MonsterLoader>();
        monsterPool = new List<GameObject>();
        elitemonsterPool = new List<GameObject>();
        bossmonsterPool = new List<GameObject>();
        inCombatList = new List<GameObject>();
        // ���� �����͸� �ҷ��ɴϴ�
        Monsters monstersList = monsterLoader.LoadMonsterData();

        int countToExclude = 4;
        int validCount = monstersList.MonsterList.Count - countToExclude;
        List<MonsterData> validMonstersList = monstersList.MonsterList.Take(validCount).ToList();
        List<MonsterData> eliteMonsterList  = monstersList.MonsterList.GetRange(validCount, countToExclude - 1);
        List<MonsterData> bossMonsterList = monstersList.MonsterList.GetRange(monstersList.MonsterList.Count - 1, 1); //���� ���� ���Ͱ� �Ѱ��� �����ѹ� ���
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
                monsterComponent.Initialize(data); // ���� �����͸� �ʱ�ȭ�մϴ�
                monsterObject.transform.SetParent(transform);
                monsterObject.SetActive(false); // �ʱ⿡ ��Ȱ��ȭ ���·� �����մϴ�
                bossmonsterPool.Add(monsterObject);                           // �߰������� �ʿ��� �ʱ�ȭ �۾��� ������ �� �ֽ��ϴ�

            }
            else
            {
                Debug.LogError("Monster ��ũ��Ʈ�� �����տ��� ã�� �� �����ϴ�.");
                Destroy(monsterObject); // ���� �߻� �� �ν��Ͻ��� �����մϴ�
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
                    monsterComponent.Initialize(data); // ���� �����͸� �ʱ�ȭ�մϴ�
                    monsterObject.transform.SetParent(transform);
                    monsterObject.SetActive(false); // �ʱ⿡ ��Ȱ��ȭ ���·� �����մϴ�
                    monsterPool.Add(monsterObject);                           // �߰������� �ʿ��� �ʱ�ȭ �۾��� ������ �� �ֽ��ϴ�

                }
                else
                {
                    Debug.LogError("Monster ��ũ��Ʈ�� �����տ��� ã�� �� �����ϴ�.");
                    Destroy(monsterObject); // ���� �߻� �� �ν��Ͻ��� �����մϴ�
                }
            }
        }
        else
        {
            Debug.LogError("�������� ã�� �� �����ϴ�: " + prefabName);
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
                monsterComponent.Initialize(data); // ���� �����͸� �ʱ�ȭ�մϴ�
                monsterObject.transform.SetParent(transform);
                monsterObject.SetActive(false); // �ʱ⿡ ��Ȱ��ȭ ���·� �����մϴ�
                elitemonsterPool.Add(monsterObject);                           // �߰������� �ʿ��� �ʱ�ȭ �۾��� ������ �� �ֽ��ϴ�

            }
            else
            {
                Debug.LogError("Monster ��ũ��Ʈ�� �����տ��� ã�� �� �����ϴ�.");
                Destroy(monsterObject); // ���� �߻� �� �ν��Ͻ��� �����մϴ�
            }
        }
    }


    

    public void InitMonster(int i)
    {
        monsterPool[i].SetActive(true);
        monsterPool[i].transform.position = new Vector3(2f, -1.5f, 0f); //�� �ϵ� �ڵ��� ��ġ���� combat stage��� ������ ���� �װ��� �־ �Ǳ� �ҵ� -> combat stage list���� �־ �� �ڸ��� ��� �ִٸ� �ֱ�
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
