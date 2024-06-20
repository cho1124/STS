using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Character
{
    private void Start()
    {
        currentHealth = maxHealth;
    }

    public override void Attack(Character target)
    {
        int damage = Mathf.Max(0, attackPower - target.defense);
        target.TakeDamage(damage);
    }

    public override void TakeDamage(int damage)
    {
        currentHealth = Mathf.Max(0, currentHealth - damage);
        GameManager.instance.UpdateAllUI();
        if (currentHealth == 0)
        {
            Die();
        }
    }

    public void SetDefense(int def)
    {

    }

    private void Die()
    {
        Debug.Log(characterName + " has been defeated.");
        // 추가적인 사망 처리 로직
    }
}



