using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewMonsterData", menuName = "Monster Data", order = 51)]
public class MonsterData : ScriptableObject
{
    public string monsterName;
    public int maxHealth;
    public int minHealth;
    public int attackPower;
    public GameObject monsterPrefab;
    
}

[CreateAssetMenu(fileName = "NewSlimeData", menuName = "Monster Data/Slime", order = 52)]
public class SlimeData : MonsterData
{
    public int splitHealthThreshold;
}

[CreateAssetMenu(fileName = "NewGremlinData", menuName = "Monster Data/Gremlin", order = 53)]
public class GremlinData : MonsterData
{
    public int defenseIncrease;
}

[CreateAssetMenu(fileName = "NewFungiData", menuName = "Monster Data/Fungi", order = 54)]
public class FungiData : MonsterData
{
    // �߰����� Fungi ���� �Ӽ� ���� ����
}

[CreateAssetMenu(fileName = "NewCultistData", menuName = "Monster Data/Cultist", order = 55)]
public class CultistData : MonsterData
{
    // �߰����� Cultist ���� �Ӽ� ���� ����
}

[CreateAssetMenu(fileName = "NewReslaverData", menuName = "Monster Data/Reslaver", order = 56)]
public class ReslaverData : MonsterData
{
    // �߰����� Cultist ���� �Ӽ� ���� ����
}

[CreateAssetMenu(fileName = "NewJawWormData", menuName = "Monster Data/JawWorm", order = 57)]
public class JawWormData : MonsterData
{
    // �߰����� Cultist ���� �Ӽ� ���� ����
}

[CreateAssetMenu(fileName = "NewLagaBulinData", menuName = "Monster Data/Elite/LagaBulin", order = 61)]
public class LagaBulinData : MonsterData
{
    // �߰����� LagaBulin ���� �Ӽ� ���� ����
}

[CreateAssetMenu(fileName = "NewEliteGremlinData", menuName = "Monster Data/Elite/EliteGremlin", order = 62)]
public class EliteGremlinData : MonsterData
{
    // �߰����� EliteGremlin ���� �Ӽ� ���� ����
}

[CreateAssetMenu(fileName = "NewCentryData", menuName = "Monster Data/Elite/Centry", order = 63)]
public class CentryData : MonsterData
{
    // �߰����� EliteGremlin ���� �Ӽ� ���� ����
}

[CreateAssetMenu(fileName = "NewSlimeKingData", menuName = "Monster Data/Boss/SlimeKing", order = 71)]
public class SlimeKingData : MonsterData
{
    public int splitHealthThreshold;
}