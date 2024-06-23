using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterPatternHandler : MonoBehaviour
{
    public Monster monster;

    public void ApplyPatterns()
    {
        // 몬스터의 패턴을 적용합니다.
        // 예를 들어, 몬스터가 공격하거나 방어하는 패턴을 여기에 구현합니다.

        // 예제 패턴: 매 턴마다 3 데미지를 플레이어에게 줍니다.
        AttackPlayer(3);
    }

    void AttackPlayer(int damage)
    {
        // 플레이어에게 데미지를 입히는 로직을 구현합니다.
        Debug.Log($"Monster attacks player for {damage} damage");
        // 플레이어의 TakeDamage 메서드를 호출하여 데미지를 적용합니다.
    }
}
