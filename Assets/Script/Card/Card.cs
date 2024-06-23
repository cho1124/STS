using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class Card
{
    public int id;
    public string cardName;
    public int cost;
    public int rare;
    public string type;
    public string cardDescription;
    public string targetType; // enemy select, none, enemy all

    public Sprite cardImage;
    public Sprite rare_type_Image;
    public Sprite rare_Image;
    public List<CardEffectType> effectTypes; // 여러 효과 타입 (damage, draw, gainArmor 등)
    public List<int> effectValues;   // 각 효과 값

    public Card() { }

    public Card(int id, string cardName, int cost, string type, int rare, string cardDescription, Sprite cardImage, Sprite rare_type_Image, Sprite rare_Image, List<CardEffectType> effectTypes, List<int> effectValues)
    {
        this.id = id;
        this.cardName = cardName;
        this.cost = cost;
        this.type = type;
        this.rare = rare;
        this.cardDescription = cardDescription;
        this.cardImage = cardImage;
        this.rare_type_Image = rare_type_Image;
        this.rare_Image = rare_Image;
        this.effectTypes = effectTypes;
        this.effectValues = effectValues;
    }
}
