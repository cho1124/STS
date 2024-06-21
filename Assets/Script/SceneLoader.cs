using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class SceneLoader : MonoBehaviour
{
    public GameObject Map;
    public GameObject DownCanvas;
    public GameObject returnScene;
    // 버튼을 클릭할 때 호출할 함수
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
            Map.SetActive(false);
            DownCanvas.SetActive(true);
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
