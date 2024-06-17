using UnityEngine;

[CreateAssetMenu(fileName = "NewCharacter", menuName = "Character/Character Data")]
public class CharacterData : ScriptableObject
{
    public string characterName;
    public int maxHealth;
    public int startingGold;
    public Sprite characterSprite;
    // �߰����� ĳ���� �Ӽ����� �����մϴ�.
}
