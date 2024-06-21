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
        // MonsterLoader ������Ʈ�� �����ɴϴ�
        monsterLoader = GetComponent<MonsterLoader>();
        monsterPool = new List<GameObject>();
        inCombatList = new List<GameObject>();
        // ���� �����͸� �ҷ��ɴϴ�
        Monsters monstersList = monsterLoader.LoadMonsterData();
        if (monstersList != null)
        {
            // �� ���� �����͸� ����Ͽ� ���͸� �����մϴ�
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

    public void InitMonster(int i)
    {
        monsterPool[i].SetActive(true);
        monsterPool[i].transform.position = new Vector3(2f, -2f, 0f);
        inCombatList.Add(monsterPool[i]);
    }


}
