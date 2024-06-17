using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ThisCard : MonoBehaviour
{
    public List<Card> thisCard = new List<Card>();
    public int thisId;

    public int id;
    public string cardName;
    public int cost;
    public string type;
    public int rare;
    public string cardDescription;

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
        thisCard[0] = CardDatabase.cardList[thisId];
        //thisCard[1] = CardDatabase.cardList[5];
        id = thisCard[0].id;
        cardName = thisCard[0].cardName;
        cost = thisCard[0].cost;
        type = thisCard[0].type;

        cardDescription = thisCard[0].cardDescription;


        nameText.text = "" + cardName;
        costText.text = "" + cost;
        typeText.text = "" + type;
        descriptionText.text = "" + cardDescription;
    }

    // Update is called once per frame
    void Update()
    {
        //id = thisCard[0].id;
        //cardName = thisCard[0].cardName;
        //cost = thisCard[0].cost;
        //type = thisCard[0].type;
        //
        //
        //
        //cardDescription = thisCard[0].cardDescription;
        //
        //
        //nameText.text = "" + cardName;
        //costText.text = "" + cost;
        //typeText.text = "" + type;
        //descriptionText.text = "" + cardDescription;

        cardSprite = thisCard[0].cardImage;

        rare_type_Image.sprite = thisCard[0].rare_type_Image;
        rareImage.sprite = thisCard[0].rare_Image;

        cardImage.sprite = cardSprite;

    }
}
