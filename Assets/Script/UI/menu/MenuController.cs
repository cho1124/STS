using UnityEngine;
using UnityEngine.SceneManagement; // 씬 관리를 위한 네임스페이스 추가
using UnityEngine.UI; // UI 요소를 다루기 위한 네임스페이스 추가
using System.Collections.Generic;

public class MenuController : MonoBehaviour
{
    // UI 패널들에 대한 참조
    public GameObject startPanel; // 시작 화면 패널
    public Button startButton;
    public Button exitButton;
    
    public GameObject characterSelectPanel; // 캐릭터 선택 패널
    public GameObject confirmPanel; // 캐릭터 확인 패널
    public GameObject IronCladImage;
    private Stack<GameObject> uiStack = new Stack<GameObject>(); // UI 스택

    void Start()
    {
        PushToStack(startPanel); // 시작 화면을 스택에 넣음
    }

    // 시작 버튼 클릭 시 호출되는 메서드
    public void OnStartButtonClick()
    {
        PushToStack(characterSelectPanel); // 캐릭터 선택 패널을 스택에 넣음
        
    }

    // 각 캐릭터 버튼 클릭 시 호출되는 메서드
    public void OnCharacterButtonClick(string characterName)
    {
        // 캐릭터 선택 관련 로직을 수행하고, 캐릭터 확인 패널을 스택에 넣음
        // 여기서는 간단히 캐릭터 이름을 출력하는 예시를 들었습니다.
        //Debug.Log("Selected Character: " + characterName);
        //PushToStack(confirmPanel);
        
        IronCladImage.SetActive(true);
    }

    // 취소 버튼 클릭 시 호출되는 메서드
    public void OnCancelButtonClick()
    {
        IronCladImage.SetActive(false);
        PopFromStack(); // 스택에서 팝하여 이전 UI로 돌아감
    }

    // 스택에 UI 패널을 추가하는 메서드
    private void PushToStack(GameObject uiPanel)
    {
        

        uiStack.Push(uiPanel);
        uiPanel.SetActive(true); // 스택에 추가된 패널 활성화
    }

    // 스택에서 UI 패널을 제거하는 메서드
    private void PopFromStack()
    {
        if (uiStack.Count == 0)
        {
            Debug.LogWarning("UI 스택이 비어 있습니다.");
            return;
        }

        GameObject currentPanel = uiStack.Pop();
        currentPanel.SetActive(false); // 현재 활성화된 패널 비활성화

        if (uiStack.Count > 0)
        {
            GameObject previousPanel = uiStack.Peek();
            previousPanel.SetActive(true); // 이전 패널을 활성화하여 이전 상태로 돌아감
        }
    }

    // 실제 게임 시작 버튼 클릭 시 호출되는 메서드
    public void OnConfirmButtonClick()
    {
        // 선택된 캐릭터 정보를 저장하고 실제 게임 씬으로 전환하는 로직을 추가할 수 있습니다.
        SceneManager.LoadScene("Fight");
    }
    




    // 게임 종료 버튼 클릭 시 호출되는 메서드
    public void OnQuitButtonClick()
    {
        // 게임을 종료합니다.
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}
