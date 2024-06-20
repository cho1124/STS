using UnityEngine;

public class MapMover : MonoBehaviour
{
    public float scrollSpeed = 10f;  // 스크롤 속도

    void Update()
    {
        // 마우스 휠 입력 감지
        float scroll = Input.GetAxis("Mouse ScrollWheel");

        // 맵 오브젝트를 위아래로 이동
        if (scroll != 0)
        {
            Vector3 newPosition = transform.position + new Vector3(0, -scroll * scrollSpeed, 0);
            transform.position = newPosition;
        }
    }
}
