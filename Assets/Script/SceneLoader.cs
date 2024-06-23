using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class SceneLoader : MonoBehaviour
{
    public GameObject Map;
    public GameObject DownCanvas;
    public GameObject returnScene;
    // ��ư�� Ŭ���� �� ȣ���� �Լ�
    public void LoadSceneAdditively(bool isTrue)
    {
        if(isTrue)
        {
            Map.SetActive(true);
            DownCanvas.SetActive(false);
            returnScene.SetActive(true);
        }
        else
        {
            if(GameManager.instance.isCombat == true)
            {
                DownCanvas.SetActive(true);
            }
            


            Map.SetActive(false);
            
            returnScene.SetActive(false);
        }

        
    }

    public void HideNonCombatUI()
    {
        LoadSceneAdditively(false);
    }

    public void ShowNonCombatUI()
    {
        LoadSceneAdditively(true);
    }




}
