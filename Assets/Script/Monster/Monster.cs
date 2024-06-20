using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class Monster : Character
{
    public MonsterData data;

    private void Start()
    {
        InitializeMonster();
    }

    private void InitializeMonster()
    {
        if (data != null)
        {
            characterName = data.monsterName;
            maxHealth = data.maxHealth;
            currentHealth = maxHealth;
            attackPower = data.attackPower;
            defense = 0;
        }
        else
        {
            Debug.LogError("Monster data not assigned.");
        }
    }

    public override void Attack(Character target)
    {
        int damage = Mathf.Max(0, attackPower - target.defense);
        target.TakeDamage(damage);
        Debug.Log(characterName + " attacks " + target.characterName + " for " + damage + " damage.");
    }

    public override void TakeDamage(int damage)
    {
        currentHealth = Mathf.Max(0, currentHealth - damage);
        if (currentHealth == 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Debug.Log(characterName + " has been defeated.");
        Destroy(gameObject);
    }
}

