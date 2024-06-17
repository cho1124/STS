using UnityEngine;

[CreateAssetMenu(fileName = "NewCharacter", menuName = "Character/Character Data")]
public class CharacterData : ScriptableObject
{
    public string characterName;
    public int maxHealth;
    public int startingGold;
    public Sprite characterSprite;
    // 추가적인 캐릭터 속성들을 정의합니다.
}
