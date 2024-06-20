using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;


public class TurnSystem : MonoBehaviour
{

    public bool isMyTurn;
    public int myTurn;
    
    public Text turnText;

    public int maxMana;
    public int currentMana;
    public Text manatext;

    public GameObject myturnImage;
    public GameObject enemyturnImage;
    public Text turncountText;



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
        if(Input.GetKeyDown(KeyCode.T))
        {
            if(isMyTurn)
            {
                EndMyTurn();
            }
            else
            {
                EndOponentTurn();
            }
        }

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
        StartCoroutine(MyTurnUICo());

    }

    public void EndOponentTurn()
    {
        isMyTurn = true;
        myTurn += 1;
        currentMana = maxMana;
        StartCoroutine(MyTurnUICo());
    }

    public IEnumerator MyTurnUICo()
    {
        if(isMyTurn)
        {
            myturnImage.SetActive(true);
            turncountText.text = $"{myTurn}턴";
            yield return new WaitForSeconds(1.5f);
            myturnImage.SetActive(false);
        }
        else
        {
            enemyturnImage.SetActive(true);
            yield return new WaitForSeconds(1.5f);
            enemyturnImage.SetActive(false);
        }


        


        // 로그 출력
        
    }

}
