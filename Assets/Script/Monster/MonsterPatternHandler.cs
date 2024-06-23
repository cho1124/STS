using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterPatternHandler : MonoBehaviour
{
    public Monster monster;

    public void ApplyPatterns()
    {
        // ������ ������ �����մϴ�.
        // ���� ���, ���Ͱ� �����ϰų� ����ϴ� ������ ���⿡ �����մϴ�.

        // ���� ����: �� �ϸ��� 3 �������� �÷��̾�� �ݴϴ�.
        AttackPlayer(3);
    }

    void AttackPlayer(int damage)
    {
        // �÷��̾�� �������� ������ ������ �����մϴ�.
        Debug.Log($"Monster attacks player for {damage} damage");
        // �÷��̾��� TakeDamage �޼��带 ȣ���Ͽ� �������� �����մϴ�.
    }
}
