using UnityEngine;

[System.Serializable]
public class PlayerStatus
{
    public string statusName;
    public float duration; // 상태가 유지될 시간
    // 필요한 경우 추가적인 상태 관련 정보들을 정의할 수 있습니다.

    public PlayerStatus(string name, float duration)
    {
        statusName = name;
        this.duration = duration;
    }
}