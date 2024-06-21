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

        // �ʿ��� �ʱ�ȭ �۾��� ���⿡ �߰��� �� �ֽ��ϴ�.
        //Debug.Log($"���� ����: {monsterName}, ü��: {minHealth}-{maxHealth}, ���ݷ�: {attack}");




    }
    public int GetCurrentHealth()
    {
        return currentHealth;
    }

}
