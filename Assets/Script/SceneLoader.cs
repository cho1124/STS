using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public GameObject Map;

    // ��ư�� Ŭ���� �� ȣ���� �Լ�
    public void LoadSceneAdditively()
    {
        Map.SetActive(true);
    }
}
