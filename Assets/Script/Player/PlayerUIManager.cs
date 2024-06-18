using UnityEngine;
using UnityEngine.UI; // Text UI�� ����ϱ� ���� �߰�

public class PlayerUIManager : MonoBehaviour
{
    public Text healthText; // ü���� ǥ���� Text UI ���
    public Text goldText;
    public Text floorText;
    public Text ManaText;
    public Text DeckText;
    
    void Start()
    {
        // Unity Inspector���� ������ Text UI ��� ��������
        healthText = GameObject.Find("HealthText").GetComponent<Text>(); // HealthText�� Text UI ����� �̸��Դϴ�.
    }

    // ü���� ������Ʈ�ϴ� �Լ�
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
