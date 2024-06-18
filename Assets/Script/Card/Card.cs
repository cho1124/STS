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
    //public int rare;
    public string cardDescription;
    public string targetType; // enemy select, none, enemy all

    public Sprite cardImage;
    public Sprite rare_type_Image;
    public Sprite rare_Image;


    public Card()
    {

    }

    public Card(int id, string cardName, int cost, string type, int rare, string cardDescription, Sprite cardImage, Sprite rare_type_Image, Sprite rare_Image)
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
    }
}
