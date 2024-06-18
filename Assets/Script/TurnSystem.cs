using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class TurnSystem : MonoBehaviour
{

    public bool isMyTurn;
    public int myTurn;
    
    public Text turnText;

    public int maxMana;
    public int currentMana;
    public Text manatext;



    // Start is called before the first frame update
    void Start()
    {
        isMyTurn = true;
        myTurn = 1;
        currentMana = maxMana;
        
    }

    // Update is called once per frame
    void Update()
    {
        if(isMyTurn == true)
        {
            turnText.text = "턴 종료";
        }
        else
        {
            turnText.text = "적의 턴";
        }
        manatext.text = currentMana + "/" + maxMana;

    }

    public void EndMyTurn()
    {
        isMyTurn = false;

    }

    public void EndOponentTurn()
    {
        isMyTurn = true;
        myTurn += 1;
        currentMana = maxMana;
    }

}
