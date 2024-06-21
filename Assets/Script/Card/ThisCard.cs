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

    public Text nameText;
    public Text costText;
    public Text typeText;
    public Text descriptionText;
    public Sprite cardSprite;
    public Image cardImage;
    public Image rare_type_Image;
    public Image rareImage;



    private void Awake()
    {
        
    }

    // Start is called before the first frame update
    void Start()
    {
        thisCard = new Card();
        //thisCard[1] = CardDatabase.cardList[5];
        thisCard = CardDatabase.cardList[thisId];
        id = thisCard.id;
        cardName = thisCard.cardName;
        cost = thisCard.cost;
        type = thisCard.type;

        cardDescription = thisCard.cardDescription;


        nameText.text = "" + cardName;
        costText.text = "" + cost;
        typeText.text = "" + type;
        descriptionText.text = "" + cardDescription;

        cardSprite = thisCard.cardImage;

        rare_type_Image.sprite = thisCard.rare_type_Image;
        rareImage.sprite = thisCard.rare_Image;

        cardImage.sprite = cardSprite;
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

        

    }
}
