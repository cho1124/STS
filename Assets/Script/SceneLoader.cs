using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class SceneLoader : MonoBehaviour
{
    public GameObject Map;
    public GameObject DownCanvas;
    // ��ư�� Ŭ���� �� ȣ���� �Լ�
    public void LoadSceneAdditively()
    {
        Map.SetActive(true);
        DownCanvas.SetActive(false);
    }
}
