using System.Collections.Generic;
using UnityEngine;

public abstract class Character : MonoBehaviour
{
    public string characterName;
    public int maxHealth;
    public int currentHealth;
    public int attackPower;
    public int defense;

    private List<StatusEffect> activeEffects = new List<StatusEffect>();

    public void ApplyStatusEffect(StatusEffect effect)
    {
        effect.ApplyEffect(this);
        activeEffects.Add(effect);
    }

    public void RemoveStatusEffect(StatusEffect effect)
    {
        effect.RemoveEffect(this);
        activeEffects.Remove(effect);
    }

    public void EndTurn()
    {
        for (int i = activeEffects.Count - 1; i >= 0; i--)
        {
            activeEffects[i].duration--;
            if (activeEffects[i].duration <= 0)
            {
                RemoveStatusEffect(activeEffects[i]);
            }
        }
    }

    public abstract void Attack(Character target);
    public abstract void TakeDamage(int damage);
}
