using UnityEngine;
using System.Collections.Generic;

[System.Flags]
public enum PlayerState
{
    None = 0,
    분노 = 1 << 0,
    취약 = 1 << 1,
    약화 = 1 << 2,
    바리케이트 = 1 << 3,
    // 필요한 다른 상태들을 추가할 수 있습니다.
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
    // 추가적인 캐릭터 속성들을 정의합니다.

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
