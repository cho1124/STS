using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public enum CardEffectType
{
    None, // Ư�� ȿ���� ���� ���
    DealDamage, // ���ظ� ��
    ApplyVulnerable, // ��� ���� �ο�
    MultiplyStrengthEffect, // ���� ȿ�� ����
    DrawCards, // ī�带 ����
    GainStrength, // ���� ����
    DealDamageBasedOnArmor, // ���� ����ŭ ���ظ� ��
    NoArmorExpiration, // �� ���� ����
    DoubleArmor, // ���� �� ��� ����
    StartTurnNoArmorExpiration, // �� ���� �� �� ���� ����
    GainArmor, // ���� ����
    IncreaseStrengthEveryTurn, // �� �ϸ��� ���� ����
    MultiplyDamageEffect, // ���� ȿ���� �谡��
    IncreaseDefense, // ��ø�� ������Ŵ->���� x
    GainMaxHealthOnCritical, // ġ��Ÿ �� �ִ� ü���� ����
    DealDamageToAllEnemies, // ��� ������ ���ظ� ��
    ReduceOpponentStrength, // ���� ���� ���ҽ�Ŵ
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

    public List<string> spriteSheetNames; // ���� ���� ��������Ʈ ��Ʈ �̸� ����Ʈ

    private List<Sprite> allSprites = new List<Sprite>(); // ��� ��������Ʈ�� ���� ����Ʈ

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

            // ȿ�� Ÿ�԰� ���� �Ľ�


            int frame = rare;
            if (type == "��ų")
            {
                frame += 3;
            }
            else if (type == "�Ŀ�")
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

        // �� ȿ�� Ÿ�Կ� ���� ���� �����ϰ� ����Ʈ�� �߰�
        if (cardDescription.Contains("���ظ�"))
        {
            if (cardDescription.Contains("��"))
            {
                effectTypes.Add(CardEffectType.MutipleAttack);
            }
            effectTypes.Add(CardEffectType.DealDamage);
        }
        if (cardDescription.Contains("�����"))
        {
            effectTypes.Add(CardEffectType.ApplyVulnerable);
        }
        if (cardDescription.Contains("���� ȿ����"))
        {
            effectTypes.Add(CardEffectType.MultiplyStrengthEffect);
        }
        if (cardDescription.Contains("ī�带") && cardDescription.Contains("�̽��ϴ�"))
        {
            effectTypes.Add(CardEffectType.DrawCards);
        }
        if (cardDescription.Contains("���� "))
        {
            if(cardDescription.Contains("�� ���۽�"))
            {
                effectTypes.Add(CardEffectType.EveryTurnStart);
            }

            if (cardDescription.Contains("����ϴ�"))
            {
                effectTypes.Add(CardEffectType.GainStrength);
            }
            else if (cardDescription.Contains("�ҽ��ϴ�"))
            {
                effectTypes.Add(CardEffectType.ReduceStrength);
            }
        }
        if (cardDescription.Contains("���� ����ŭ ���ظ� �ݴϴ�"))
        {
            effectTypes.Add(CardEffectType.DealDamageBasedOnArmor);
        }
        if (cardDescription.Contains("���� �� ���"))
        {
            effectTypes.Add(CardEffectType.DoubleArmor);
        }
        
        if (cardDescription.Contains("���� "))
        {
            if (cardDescription.Contains("�� �����"))
            {
                effectTypes.Add(CardEffectType.EveryTurnEnd);

            }
            effectTypes.Add(CardEffectType.GainArmor);
        }
        

        if (cardDescription.Contains("���� ������� �ʽ��ϴ�"))
        {
            effectTypes.Add(CardEffectType.NoArmorExpiration);
        }


        if (cardDescription.Contains("���� ȿ����"))
        {
            effectTypes.Add(CardEffectType.MultiplyDamageEffect);
        }
        
        if (cardDescription.Contains("�� ��ü����"))
        {
            effectTypes.Add(CardEffectType.DealDamageToAllEnemies);
        }
        if (cardDescription.Contains("������"))
        {
            effectTypes.Add(CardEffectType.GainEnergy);
        }
        if (cardDescription.Contains("ü����"))
        {
            effectTypes.Add(CardEffectType.ReduceHealth);
        }
        if (cardDescription.Contains("���� 2��� ����"))
        {
            effectTypes.Add(CardEffectType.DoubleArmor);
        }
        if (cardDescription.Contains("���� 2��� ����"))
        {
            effectTypes.Add(CardEffectType.DoubleStrength);
        }
        if (cardDescription.Contains("���� ����"))
        {
            effectTypes.Add(CardEffectType.ReduceOpponentStrength);
        }
        if (cardDescription.Contains("�Ҹ�"))
        {
            effectTypes.Add(CardEffectType.Deletable);
        }

        return effectTypes;
    }

    // ī�� ������ ȿ�� ���� �Ľ��Ͽ� ��ȯ
    public List<int> ParseEffectValues(string cardDescription)
    {
        List<int> effectValues = new List<int>();

        // �� ȿ�� �� ����
        if (cardDescription.Contains("���ظ� "))
        {
            effectValues.Add(ParseEffectValue(cardDescription, "���ظ� "));
        }
        if (cardDescription.Contains("����� "))
        {
            effectValues.Add(ParseEffectValue(cardDescription, "����� "));
        }
        if (cardDescription.Contains("���� ȿ���� "))
        {
            effectValues.Add(ParseEffectValue(cardDescription, "���� ȿ���� "));
            
        }
        if (cardDescription.Contains("ī�带") && cardDescription.Contains("�̽��ϴ�"))
        {
            effectValues.Add(ParseEffectValue(cardDescription, "ī�带 "));
           
        }
        if (cardDescription.Contains("���� "))
        {
            effectValues.Add(ParseEffectValue(cardDescription, "���� "));
            
        }
        if (cardDescription.Contains("���� ����ŭ ���ظ� �ݴϴ�"))
        {
            effectValues.Add(0);
        }
        if (cardDescription.Contains("���� �� ���"))
        {
            effectValues.Add(0);
        }
        if (cardDescription.Contains("���� ������� �ʽ��ϴ�"))
        {
            effectValues.Add(0);
        }
        if (cardDescription.Contains("���� "))
        {
            effectValues.Add(ParseEffectValue(cardDescription, "���� "));
            
        }
        if (cardDescription.Contains("�� ���۽� "))
        {
            effectValues.Add(0);
            if(cardDescription.Contains("���� "))
            {
                effectValues.Add(ParseEffectValue(cardDescription, "���� "));
            }

            
        }
        
       
        if (cardDescription.Contains("ġ��Ÿ��� �ִ� ü����"))
        {
            effectValues.Add(ParseEffectValue(cardDescription, "�ִ� ü���� "));
            
        }
        if (cardDescription.Contains("�� ��ü����"))
        {
            effectValues.Add(0);
            
        }
        if (cardDescription.Contains("�������� "))
        {
            effectValues.Add(ParseEffectValue(cardDescription, "�������� "));
        }
        if (cardDescription.Contains("ü���� ") && cardDescription.Contains("�ҽ��ϴ�"))
        {
            effectValues.Add(-(ParseEffectValue(cardDescription, "ü���� ")));
        }
        if (cardDescription.Contains("���� 2��� ����"))
        {
            effectValues.Add(0);
           
        }
        if (cardDescription.Contains("���� 2��� ����"))
        {
            effectValues.Add(0);
        }
        
        if (cardDescription.Contains("�Ҹ�"))
        {
            effectValues.Add(0);
        }

        return effectValues;
    }

    // ī�� ������ ȿ�� ���� �����Ͽ� ��ȯ
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