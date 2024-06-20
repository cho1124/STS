using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    public Player player;
    public List<GameObject> monsterPrefabs;
    private Monster currentMonster;

    private void Start()
    {
        SpawnMonster();
    }

    void SpawnMonster()
    {
        int randomIndex = Random.Range(0, monsterPrefabs.Count);
        GameObject monsterObject = Instantiate(monsterPrefabs[randomIndex], transform.position, Quaternion.identity);
        currentMonster = monsterObject.GetComponent<Monster>();
        if (currentMonster == null)
        {
            Debug.LogError("Monster component not found on the spawned prefab.");
        }
    }

    public void PlayerAttack()
    {
        if (currentMonster != null)
        {
            player.SetDefense(0); // 방어 초기화
            currentMonster.TakeDamage(player.attackPower);
        }

        if (currentMonster != null)
        {
            currentMonster.Attack(player);
        }

        EndTurn();
    }

    void EndTurn()
    {
        player.EndTurn();
        if (currentMonster != null)
        {
            currentMonster.EndTurn();
        }
    }

    public void ApplyWeaknessToMonster()
    {
        if (currentMonster != null)
        {
            WeaknessEffect weakness = ScriptableObject.CreateInstance<WeaknessEffect>();
            weakness.effectName = "Weakness";
            weakness.duration = 3; // 3턴 지속
            weakness.attackReduction = 5;

            currentMonster.ApplyStatusEffect(weakness);
        }
    }

}
