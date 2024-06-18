using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class CardDatabase : MonoBehaviour
{
    // Start is called before the first frame update
    public static List<Card> cardList = new List<Card>();

    public TextAsset CardDB;
    //public Sprite asd;
    //public List<Sprite> spriteSheets;

    public List<string> spriteSheetNames; // 여러 개의 스프라이트 시트 이름 리스트

    private List<Sprite> allSprites = new List<Sprite>(); // 모든 스프라이트를 담을 리스트

    public List<string> AllCardFrameNames;

    private List<Sprite> allFrames = new List<Sprite>();


    private void Awake()
    {   

        string[] line = CardDB.text.Substring(0, CardDB.text.Length - 1).Split('\n');
        //Debug.Log(line.Length);


        //Sprite[] Sprites = Resources.LoadAll<Sprite>(spriteSheetName);

        LoadAllSprites(spriteSheetNames, allSprites);
        LoadAllSprites(AllCardFrameNames, allFrames);


        for (int i = 0; i < line.Length; i++)
        {
            string[] row = line[i].Split('\t');

            int id = int.Parse(row[0]);
            int cost = int.Parse(row[2]);
            int rare = int.Parse(row[4]);

            int frame = rare;
            if(row[3] == "스킬")
            {
                frame += 3;
            }
            else if(row[3] == "파워")
            {
                frame += 6;
            }

            Sprite cardSprite = FindSpriteByName("Card"+(id + 1), allSprites);
            Sprite FrameSprite = FindSpriteByName("Frame" + frame, allFrames);
            Sprite TitleSprite = FindSpriteByName("title" + rare, allFrames);
            //card1Sprite = Resources.Load<Sprite>("123")
            //Sprite cardSprite = FindSpriteByName(allSprites, "Card" + id);
            cardList.Add(new Card(id, row[1], cost, row[3], rare, row[5], cardSprite, FrameSprite, TitleSprite));

        }
        



        //CardDB.
        //cardList.Add(new Card(0, "None", 0, 1, "power", "None"));
        //cardList.Add(new Card(1, "1", 2, 1, "skill", "None"));
        //cardList.Add(new Card(2, "2", 3, 2, "attack", "None"));
        //cardList.Add(new Card(3, "3", 4, 3, "power", "None"));
        //cardList.Add(new Card(4, "4", 1, 3, "skill", "None"));
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

}
