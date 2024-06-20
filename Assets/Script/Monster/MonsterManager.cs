using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterManager : MonoBehaviour
{
    // 몬스터 데이터를 담을 리스트
    public List<MonsterData> monsterDatas;

    public List<SlimeData> slimeDatas;

    public List<GremlinData> gremlinDatas;

    public List<ReslaverData> reslaverDatas;

    public FungiData fumgiData;

    public LagaBulinData lagaBulinData;

    public CentryData centryData;

    public EliteGremlinData eliteGremlinData;

    public SlimeKingData slimeKingData;


    // 각 몬스터의 오브젝트 풀
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
