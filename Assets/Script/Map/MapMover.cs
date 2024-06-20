using UnityEngine;

public class MapMover : MonoBehaviour
{
    public float scrollSpeed = 10f;  // ��ũ�� �ӵ�

    void Update()
    {
        // ���콺 �� �Է� ����
        float scroll = Input.GetAxis("Mouse ScrollWheel");

        // �� ������Ʈ�� ���Ʒ��� �̵�
        if (scroll != 0)
        {
            Vector3 newPosition = transform.position + new Vector3(0, -scroll * scrollSpeed, 0);
            transform.position = newPosition;
        }
    }
}
