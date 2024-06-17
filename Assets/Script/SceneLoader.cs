using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public GameObject Map;

    // 버튼을 클릭할 때 호출할 함수
    public void LoadSceneAdditively()
    {
        Map.SetActive(true);
    }
}
