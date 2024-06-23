using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class Character : MonoBehaviour
{
    public List<StatusEffect> activeStatusEffects = new List<StatusEffect>();
    public string characterName;
    public int maxHealth;
    public int currentHealth;
    public int attackPower;
    public int defense;
    public float damageTakenMultiplier = 1f;
    public bool isBaricate = false;


    public void ApplyStatusEffect(StatusEffect effect)
    {
        // Check if the same type of status effect is already active
        foreach (StatusEffect activeEffect in activeStatusEffects)
        {
            if (activeEffect.GetType() == effect.GetType())
            {
                // Update the existing effect instead of adding a new one
                activeEffect.duration = effect.duration;
                activeEffect.ApplyEffect(this);
                return;
            }
        }

        // Instantiate a new instance of the effect and apply it
        StatusEffect newEffect = ScriptableObject.CreateInstance(effect.GetType()) as StatusEffect;
        newEffect.duration = effect.duration;
        newEffect.ApplyEffect(this);
        activeStatusEffects.Add(newEffect);
    }

    public void RemoveStatusEffect(StatusEffect effect)
    {
        effect.RemoveEffect(this);
        activeStatusEffects.Remove(effect);
    }

    public void EndTurn()
    {
        
        

        //Debug.Log($"¤·¤É¾ÈµÉ±î : {activeStatusEffects.Count}");
        for (int i = activeStatusEffects.Count - 1; i >= 0; i--)
        {
            activeStatusEffects[i].duration--;
            if (activeStatusEffects[i].duration <= 0)
            {
                RemoveStatusEffect(activeStatusEffects[i]);
            }
        }
    }
    

    public abstract void Attack(Character target);
    public abstract void TakeDamage(int damage);
}
