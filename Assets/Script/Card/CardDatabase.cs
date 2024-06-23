using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public enum CardEffectType
{
    None, // 특정 효과가 없는 경우
    DealDamage, // 피해를 줌
    ApplyVulnerable, // 취약 상태 부여
    MultiplyStrengthEffect, // 힘의 효과 증폭
    DrawCards, // 카드를 뽑음
    GainStrength, // 힘을 얻음
    DealDamageBasedOnArmor, // 현재 방어도만큼 피해를 줌
    NoArmorExpiration, // 방어도 만료 없음
    DoubleArmor, // 방어도를 두 배로 증가
    StartTurnNoArmorExpiration, // 턴 시작 시 방어도 만료 없음
    GainArmor, // 방어도를 얻음
    IncreaseStrengthEveryTurn, // 매 턴마다 힘이 증가
    MultiplyDamageEffect, // 피해 효과를 배가함
    IncreaseDefense, // 민첩을 증가시킴->구현 x
    GainMaxHealthOnCritical, // 치명타 시 최대 체력을 얻음
    DealDamageToAllEnemies, // 모든 적에게 피해를 줌
    ReduceOpponentStrength, // 적의 힘을 감소시킴
    MutipleAttack,
    ReduceStrength,
    EveryTurnStart,
    EveryTurnEnd,
    GainEnergy,
    ReduceHealth,
    DoubleStrength,
    Deletable
}



public class CardDatabase : MonoBehaviour
{
    // Start is called before the first frame update
    public static List<Card> cardList = new List<Card>();

    private TextAsset CardDB;
    //public Sprite asd;
    //public List<Sprite> spriteSheets;

    public List<string> spriteSheetNames; // 여러 개의 스프라이트 시트 이름 리스트

    private List<Sprite> allSprites = new List<Sprite>(); // 모든 스프라이트를 담을 리스트

    public List<string> AllCardFrameNames;

    private List<Sprite> allFrames = new List<Sprite>();


    private void Awake()
    {

        CardDB = Resources.Load<TextAsset>("CardDB");
        string[] line = CardDB.text.Substring(0, CardDB.text.Length - 1).Split('\n');

        LoadAllSprites(spriteSheetNames, allSprites);
        LoadAllSprites(AllCardFrameNames, allFrames);

        for (int i = 0; i < line.Length; i++)
        {
            string[] row = line[i].Split('\t');

            int id = int.Parse(row[0]);
            string cardName = row[1];
            int cost = int.Parse(row[2]);
            string type = row[3];
            int rare = int.Parse(row[4]);
            string cardDescription = row[5];
            List<CardEffectType> effectTypes = ParseEffectTypes(cardDescription);
            List<int> effectValues = ParseEffectValues(cardDescription);

            // 효과 타입과 값을 파싱


            int frame = rare;
            if (type == "스킬")
            {
                frame += 3;
            }
            else if (type == "파워")
            {
                frame += 6;
            }

            Sprite cardSprite = FindSpriteByName("Card" + (id + 1), allSprites);
            Sprite FrameSprite = FindSpriteByName("Frame" + frame, allFrames);
            Sprite TitleSprite = FindSpriteByName("title" + rare, allFrames);

            cardList.Add(new Card(id, cardName, cost, type, rare, cardDescription, cardSprite, FrameSprite, TitleSprite, effectTypes, effectValues));
        }
    }


    private void LoadAllSprites(List<string> spriteSheetNames, List<Sprite> allSprites)
    {
        foreach (string spriteSheetName in spriteSheetNames)
        {
            Sprite[] sprites = Resources.LoadAll<Sprite>(spriteSheetName);
            allSprites.AddRange(sprites);
        }
    }



    private Sprite FindSpriteByName(string name, List<Sprite> allSprites)
    {
        return allSprites.Find(s => s.name == name);
    }

    public List<CardEffectType> ParseEffectTypes(string cardDescription)
    {
        List<CardEffectType> effectTypes = new List<CardEffectType>();

        // 각 효과 타입에 따라 값을 추출하고 리스트에 추가
        if (cardDescription.Contains("피해를"))
        {
            if (cardDescription.Contains("번"))
            {
                effectTypes.Add(CardEffectType.MutipleAttack);
            }
            effectTypes.Add(CardEffectType.DealDamage);
        }
        if (cardDescription.Contains("취약을"))
        {
            effectTypes.Add(CardEffectType.ApplyVulnerable);
        }
        if (cardDescription.Contains("힘의 효과가"))
        {
            effectTypes.Add(CardEffectType.MultiplyStrengthEffect);
        }
        if (cardDescription.Contains("카드를") && cardDescription.Contains("뽑습니다"))
        {
            effectTypes.Add(CardEffectType.DrawCards);
        }
        if (cardDescription.Contains("힘을 "))
        {
            if(cardDescription.Contains("턴 시작시"))
            {
                effectTypes.Add(CardEffectType.EveryTurnStart);
            }

            if (cardDescription.Contains("얻습니다"))
            {
                effectTypes.Add(CardEffectType.GainStrength);
            }
            else if (cardDescription.Contains("잃습니다"))
            {
                effectTypes.Add(CardEffectType.ReduceStrength);
            }
        }
        if (cardDescription.Contains("현재 방어도만큼 피해를 줍니다"))
        {
            effectTypes.Add(CardEffectType.DealDamageBasedOnArmor);
        }
        if (cardDescription.Contains("방어도를 두 배로"))
        {
            effectTypes.Add(CardEffectType.DoubleArmor);
        }
        
        if (cardDescription.Contains("방어도를 "))
        {
            if (cardDescription.Contains("턴 종료시"))
            {
                effectTypes.Add(CardEffectType.EveryTurnEnd);

            }
            effectTypes.Add(CardEffectType.GainArmor);
        }
        

        if (cardDescription.Contains("방어도가 사라지지 않습니다"))
        {
            effectTypes.Add(CardEffectType.NoArmorExpiration);
        }


        if (cardDescription.Contains("피해 효과를"))
        {
            effectTypes.Add(CardEffectType.MultiplyDamageEffect);
        }
        
        if (cardDescription.Contains("적 전체에게"))
        {
            effectTypes.Add(CardEffectType.DealDamageToAllEnemies);
        }
        if (cardDescription.Contains("에너지"))
        {
            effectTypes.Add(CardEffectType.GainEnergy);
        }
        if (cardDescription.Contains("체력을"))
        {
            effectTypes.Add(CardEffectType.ReduceHealth);
        }
        if (cardDescription.Contains("방어도가 2배로 증가"))
        {
            effectTypes.Add(CardEffectType.DoubleArmor);
        }
        if (cardDescription.Contains("힘이 2배로 증가"))
        {
            effectTypes.Add(CardEffectType.DoubleStrength);
        }
        if (cardDescription.Contains("적의 힘을"))
        {
            effectTypes.Add(CardEffectType.ReduceOpponentStrength);
        }
        if (cardDescription.Contains("소멸"))
        {
            effectTypes.Add(CardEffectType.Deletable);
        }

        return effectTypes;
    }

    // 카드 설명에서 효과 값을 파싱하여 반환
    public List<int> ParseEffectValues(string cardDescription)
    {
        List<int> effectValues = new List<int>();

        // 각 효과 값 추출
        if (cardDescription.Contains("피해를 "))
        {
            effectValues.Add(ParseEffectValue(cardDescription, "피해를 "));
        }
        if (cardDescription.Contains("취약을 "))
        {
            effectValues.Add(ParseEffectValue(cardDescription, "취약을 "));
        }
        if (cardDescription.Contains("힘의 효과가 "))
        {
            effectValues.Add(ParseEffectValue(cardDescription, "힘의 효과가 "));
            
        }
        if (cardDescription.Contains("카드를") && cardDescription.Contains("뽑습니다"))
        {
            effectValues.Add(ParseEffectValue(cardDescription, "카드를 "));
           
        }
        if (cardDescription.Contains("힘을 "))
        {
            effectValues.Add(ParseEffectValue(cardDescription, "힘을 "));
            
        }
        if (cardDescription.Contains("현재 방어도만큼 피해를 줍니다"))
        {
            effectValues.Add(0);
        }
        if (cardDescription.Contains("방어도를 두 배로"))
        {
            effectValues.Add(0);
        }
        if (cardDescription.Contains("방어도가 사라지지 않습니다"))
        {
            effectValues.Add(0);
        }
        if (cardDescription.Contains("방어도를 "))
        {
            effectValues.Add(ParseEffectValue(cardDescription, "방어도를 "));
            
        }
        if (cardDescription.Contains("턴 시작시 "))
        {
            effectValues.Add(0);
            if(cardDescription.Contains("힘을 "))
            {
                effectValues.Add(ParseEffectValue(cardDescription, "힘을 "));
            }

            
        }
        
       
        if (cardDescription.Contains("치명타라면 최대 체력이"))
        {
            effectValues.Add(ParseEffectValue(cardDescription, "최대 체력이 "));
            
        }
        if (cardDescription.Contains("적 전체에게"))
        {
            effectValues.Add(0);
            
        }
        if (cardDescription.Contains("에너지를 "))
        {
            effectValues.Add(ParseEffectValue(cardDescription, "에너지를 "));
        }
        if (cardDescription.Contains("체력을 ") && cardDescription.Contains("잃습니다"))
        {
            effectValues.Add(-(ParseEffectValue(cardDescription, "체력을 ")));
        }
        if (cardDescription.Contains("방어도가 2배로 증가"))
        {
            effectValues.Add(0);
           
        }
        if (cardDescription.Contains("힘이 2배로 증가"))
        {
            effectValues.Add(0);
        }
        
        if (cardDescription.Contains("소멸"))
        {
            effectValues.Add(0);
        }

        return effectValues;
    }

    // 카드 설명에서 효과 값을 추출하여 반환
    private int ParseEffectValue(string cardDescription, string keyword)
    {
        int value = 0;
        int startIndex = cardDescription.IndexOf(keyword);
        if (startIndex != -1)
        {
            startIndex += keyword.Length;
            int endIndex = cardDescription.IndexOf(" ", startIndex);
            if (endIndex == -1)
            {
                endIndex = cardDescription.Length;
            }
            string valueStr = cardDescription.Substring(startIndex, endIndex - startIndex).Trim();
            if (int.TryParse(valueStr, out value))
            {
                return value;
            }
        }
        return value;
    }


}