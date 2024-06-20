using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterManager : MonoBehaviour
{
    // ���� �����͸� ���� ����Ʈ
    public List<MonsterData> monsterDatas;

    public List<SlimeData> slimeDatas;

    public List<GremlinData> gremlinDatas;

    public List<ReslaverData> reslaverDatas;

    public FungiData fumgiData;

    public LagaBulinData lagaBulinData;

    public CentryData centryData;

    public EliteGremlinData eliteGremlinData;

    public SlimeKingData slimeKingData;


    // �� ������ ������Ʈ Ǯ
    private void Start()
    {
        monsterDatas = new List<MonsterData>
        {
            centryData,
            lagaBulinData,
            eliteGremlinData,
            slimeKingData
        };
    }



}
