using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ThisCard : MonoBehaviour
{
    //public List<Card> thisCard = new List<Card>();
    public Card thisCard;
    public int thisId;

    public int id;
    public string cardName;
    public int cost;
    public string type;
    public int rare;
    public string cardDescription;
    public List<CardEffectType> effectTypes;
    public List<int> effectValues;

    public Text nameText;
    public Text costText;
    public Text typeText;
    public Text descriptionText;
    public Sprite cardSprite;
    public Image cardImage;
    public Image rare_type_Image;
    public Image rareImage;



    // Start is called before the first frame update
    void Start()
    {
        thisCard = CardDatabase.cardList[thisId];
        id = thisCard.id;
        cardName = thisCard.cardName;
        cost = thisCard.cost;
        type = thisCard.type;

        cardDescription = thisCard.cardDescription;
        effectTypes = thisCard.effectTypes;
        effectValues = thisCard.effectValues;

        nameText.text = "" + cardName;
        costText.text = "" + cost;
        typeText.text = "" + type;
        descriptionText.text = "" + cardDescription;

        cardSprite = thisCard.cardImage;

        rare_type_Image.sprite = thisCard.rare_type_Image;
        rareImage.sprite = thisCard.rare_Image;

        cardImage.sprite = cardSprite;
    }

    

    private void DealDamage(int damage)
    {
        Monster target = FindObjectOfType<Monster>();
        if (target != null)
        {
            target.TakeDamage(damage);
        }
    }

    private void DrawCards(int drawCount)
    {
        for (int i = 0; i < drawCount; i++)
        {
            // 드로우 로직 구현 (예: 덱에서 카드 한 장씩 드로우)
        }
    }
}
