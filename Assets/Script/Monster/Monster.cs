using UnityEngine;

public class Monster : Character
{
    public string monsterName;
    //public int maxHealth;
    public int minHealth;
    public float attack;
    public int type;

    public delegate void MonsterDeathHandler(Monster monster);
    public event MonsterDeathHandler OnMonsterDeath;


    //public int currentHealth;

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

    public override void Attack(Character target)
    {
        Debug.Log($"{monsterName} attacks {target.name} for {attack} damage.");
        target.TakeDamage((int)attack);
    }

    public override void TakeDamage(int damage)
    {
        currentHealth -= (int)(damage  * damageTakenMultiplier);
        Debug.Log($"{monsterName} takes {damage * damageTakenMultiplier} damage. Current health: {currentHealth}");
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Debug.Log($"{monsterName} has died.");
        gameObject.SetActive(false);
        OnMonsterDeath?.Invoke(this);
        // 몬스터 사망 처리
        //setActive false하면서 풀로 들어가게끔 유도
    }
}
