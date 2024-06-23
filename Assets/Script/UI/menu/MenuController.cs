using UnityEngine;
using UnityEngine.SceneManagement; // �� ������ ���� ���ӽ����̽� �߰�
using UnityEngine.UI; // UI ��Ҹ� �ٷ�� ���� ���ӽ����̽� �߰�
using System.Collections.Generic;

public class MenuController : MonoBehaviour
{
    // UI �гε鿡 ���� ����
    public GameObject startPanel; // ���� ȭ�� �г�
    public Button startButton;
    public Button exitButton;
    
    public GameObject characterSelectPanel; // ĳ���� ���� �г�
    public GameObject confirmPanel; // ĳ���� Ȯ�� �г�
    public GameObject IronCladImage;
    private Stack<GameObject> uiStack = new Stack<GameObject>(); // UI ����

    void Start()
    {
        PushToStack(startPanel); // ���� ȭ���� ���ÿ� ����
    }

    // ���� ��ư Ŭ�� �� ȣ��Ǵ� �޼���
    public void OnStartButtonClick()
    {
        PushToStack(characterSelectPanel); // ĳ���� ���� �г��� ���ÿ� ����
        
    }

    // �� ĳ���� ��ư Ŭ�� �� ȣ��Ǵ� �޼���
    public void OnCharacterButtonClick(string characterName)
    {
        // ĳ���� ���� ���� ������ �����ϰ�, ĳ���� Ȯ�� �г��� ���ÿ� ����
        // ���⼭�� ������ ĳ���� �̸��� ����ϴ� ���ø� ������ϴ�.
        //Debug.Log("Selected Character: " + characterName);
        //PushToStack(confirmPanel);
        
        IronCladImage.SetActive(true);
    }

    // ��� ��ư Ŭ�� �� ȣ��Ǵ� �޼���
    public void OnCancelButtonClick()
    {
        IronCladImage.SetActive(false);
        PopFromStack(); // ���ÿ��� ���Ͽ� ���� UI�� ���ư�
    }

    // ���ÿ� UI �г��� �߰��ϴ� �޼���
    private void PushToStack(GameObject uiPanel)
    {
        

        uiStack.Push(uiPanel);
        uiPanel.SetActive(true); // ���ÿ� �߰��� �г� Ȱ��ȭ
    }

    // ���ÿ��� UI �г��� �����ϴ� �޼���
    private void PopFromStack()
    {
        if (uiStack.Count == 0)
        {
            Debug.LogWarning("UI ������ ��� �ֽ��ϴ�.");
            return;
        }

        GameObject currentPanel = uiStack.Pop();
        currentPanel.SetActive(false); // ���� Ȱ��ȭ�� �г� ��Ȱ��ȭ

        if (uiStack.Count > 0)
        {
            GameObject previousPanel = uiStack.Peek();
            previousPanel.SetActive(true); // ���� �г��� Ȱ��ȭ�Ͽ� ���� ���·� ���ư�
        }
    }

    // ���� ���� ���� ��ư Ŭ�� �� ȣ��Ǵ� �޼���
    public void OnConfirmButtonClick()
    {
        // ���õ� ĳ���� ������ �����ϰ� ���� ���� ������ ��ȯ�ϴ� ������ �߰��� �� �ֽ��ϴ�.
        SceneManager.LoadScene("Fight");
    }
    




    // ���� ���� ��ư Ŭ�� �� ȣ��Ǵ� �޼���
    public void OnQuitButtonClick()
    {
        // ������ �����մϴ�.
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}
