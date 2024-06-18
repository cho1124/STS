using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDataManager : MonoBehaviour
{
    public static PlayerDataManager instance;

    public CharacterData playerData;

    private int currentHealth;
    private int currentGold;
    


    private void Awake()
    {
        // �ٸ� ������ ������ ������Ʈ�� �̹� �ִٸ� ���� �������� �ʵ��� ó��
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // ���� ����� �� �ı����� �ʵ��� ����
        }
        else
        {
            Destroy(gameObject); // �̹� �ִ� ��� �� ������Ʈ�� �ı�
        }
    }

    private void Start()
    {
        currentHealth = playerData.maxHealth;
        currentGold = playerData.startingGold;
    }

    



    // �ʿ信 ���� �÷��̾� �����͸� �ʱ�ȭ�ϰų� �ε��ϴ� ��� �߰�
}
