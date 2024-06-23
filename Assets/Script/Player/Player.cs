using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Character
{
    //public CharacterData characterData;

    private void Start()
    {

        CharacterData data = GameManager.instance.playerData;

        maxHealth = data.maxHealth;
        characterName = data.characterName;
        
        currentHealth = maxHealth;
        isBaricate = false;
    }

    public override void Attack(Character target)
    {
        int damage = Mathf.Max(0, attackPower - target.defense);
        target.TakeDamage(damage);
    }

    public override void TakeDamage(int damage)
    {

        currentHealth -= (int)(damage * damageTakenMultiplier);
        GameManager.instance.UpdateAllUI();
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    
    private void Die()
    {
        Debug.Log(characterName + " has been defeated.");
        // 추가적인 사망 처리 로직
    }
}



