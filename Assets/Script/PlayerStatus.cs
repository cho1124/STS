using UnityEngine;

[System.Serializable]
public class PlayerStatus
{
    public string statusName;
    public float duration; // ���°� ������ �ð�
    // �ʿ��� ��� �߰����� ���� ���� �������� ������ �� �ֽ��ϴ�.

    public PlayerStatus(string name, float duration)
    {
        statusName = name;
        this.duration = duration;
    }
}