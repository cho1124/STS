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

        // �ʿ��� �ʱ�ȭ �۾��� ���⿡ �߰��� �� �ֽ��ϴ�.
        //Debug.Log($"���� ����: {monsterName}, ü��: {minHealth}-{maxHealth}, ���ݷ�: {attack}");

        


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
        // ���� ��� ó��
        //setActive false�ϸ鼭 Ǯ�� ���Բ� ����
    }
}
