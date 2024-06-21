using UnityEngine;

public class Monster : MonoBehaviour
{
    public string monsterName;
    public int maxHealth;
    public int minHealth;
    public float attack;
    public int type;
    public int currentHealth;

    private void OnEnable()
    {
        currentHealth = Random.Range(minHealth, maxHealth + 1);
    }


    public void Initialize(MonsterData data)
    {
        monsterName = data.name;
        maxHealth = data.maxhealth;
        minHealth = data.minhealth;
        attack = data.attack;
        type = data.type;
        //currentHealth = Random.Range(minHealth, maxHealth + 1);

        // 필요한 초기화 작업을 여기에 추가할 수 있습니다.
        //Debug.Log($"몬스터 생성: {monsterName}, 체력: {minHealth}-{maxHealth}, 공격력: {attack}");




    }
    public int GetCurrentHealth()
    {
        return currentHealth;
    }

}
