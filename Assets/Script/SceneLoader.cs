using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class SceneLoader : MonoBehaviour
{
    public GameObject Map;
    public GameObject DownCanvas;
    // 버튼을 클릭할 때 호출할 함수
    public void LoadSceneAdditively()
    {
        Map.SetActive(true);
        DownCanvas.SetActive(false);
    }
}
