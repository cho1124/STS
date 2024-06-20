using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

public abstract class StatusEffect : ScriptableObject
{
    public string effectName;
    public int duration;

    public abstract void ApplyEffect(Character character);
    public abstract void RemoveEffect(Character character);
}

[CreateAssetMenu(fileName = "NewWeaknessEffect", menuName = "Status Effects/Weakness", order = 1)]
public class WeaknessEffect : StatusEffect
{
    public int attackReduction;

    public override void ApplyEffect(Character character)
    {
        character.attackPower -= attackReduction;
        Debug.Log(character.name + " is weakened. Attack power reduced by " + attackReduction);
    }

    public override void RemoveEffect(Character character)
    {
        character.attackPower += attackReduction;
        Debug.Log(character.name + " is no longer weakened. Attack power restored by " + attackReduction);
    }
}

[CreateAssetMenu(fileName = "NewVulnerabilityEffect", menuName = "Status Effects/Vulnerability", order = 2)]
public class VulnerabilityEffect : StatusEffect
{
    public int defenseReduction;

    public override void ApplyEffect(Character character)
    {
        character.defense -= defenseReduction;
        Debug.Log(character.name + " is vulnerable. Defense reduced by " + defenseReduction);
    }

    public override void RemoveEffect(Character character)
    {
        character.defense += defenseReduction;
        Debug.Log(character.name + " is no longer vulnerable. Defense restored by " + defenseReduction);
    }
}
