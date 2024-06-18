using UnityEngine;
using UnityEngine.UI; // Text UI를 사용하기 위해 추가

public class PlayerUIManager : MonoBehaviour
{
    public Text healthText; // 체력을 표시할 Text UI 요소
    public Text goldText;
    public Text floorText;
    public Text ManaText;
    public Text DeckText;
    
    void Start()
    {
        // Unity Inspector에서 연결한 Text UI 요소 가져오기
        healthText = GameObject.Find("HealthText").GetComponent<Text>(); // HealthText는 Text UI 요소의 이름입니다.
    }

    // 체력을 업데이트하는 함수
    public void UpdateHealthText(int currentHealth, int maxHealth)
    {
        healthText.text = $" {maxHealth} / {currentHealth}";
    }

    public void UpdateGoldText(int gold)
    {
        goldText.text = $"{gold}";
    }

    public void UpdateFloorText(int floor)
    {
        floorText.text = $"{floor}";
    }


}
