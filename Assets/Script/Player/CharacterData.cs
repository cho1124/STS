using UnityEngine;
using System.Collections.Generic;

[System.Flags]
public enum PlayerState
{
    None = 0,
    �г� = 1 << 0,
    ��� = 1 << 1,
    ��ȭ = 1 << 2,
    �ٸ�����Ʈ = 1 << 3,
    // �ʿ��� �ٸ� ���µ��� �߰��� �� �ֽ��ϴ�.
}

[CreateAssetMenu(fileName = "NewCharacter", menuName = "Character/Character Data")]
public class CharacterData : ScriptableObject
{
    public string characterName;
    public int maxHealth;
    public int startingGold;
    public int startMana;
    public int playerPower;
    public int playerSpeed;

    public List<PlayerStatus> playerStatuses = new List<PlayerStatus>();
    // �߰����� ĳ���� �Ӽ����� �����մϴ�.

    private int currentHealth;
    private int currentGold;

    public int CurrentHealth
    {
        get { return currentHealth; }
        set { currentHealth = value; }
    }

    public int CurrentGold
    {
        get { return currentGold; }
        set { currentGold = value; }
    }


}
