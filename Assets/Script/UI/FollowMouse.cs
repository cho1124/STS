using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class FollowMouse : MonoBehaviour
{
    
    private Image image;
    private Color originalColor;

    void Start()
    {
        
        image = GetComponent<Image>();

        
    }

    public void OnDrag(PointerEventData eventData)
    {
        // UI ��Ҹ� ���콺 ��ġ�� �̵�
        transform.position = eventData.position;
        //Debug.Log(transform.position);

        // ���콺 ��ġ���� Raycast�� �����Ͽ� 2D �� ������Ʈ ã��
        Vector2 worldPoint = Camera.main.ScreenToWorldPoint(eventData.position);
        RaycastHit2D hit = Physics2D.Raycast(worldPoint, Vector2.zero);

        if (hit.collider != null && image != null)
        {
            // �� ������Ʈ�� ����
            if (hit.collider.CompareTag("Enemy"))
            {
                Debug.Log("Enemy Detected: " + hit.collider.gameObject.name);
                image.color = Color.red; // ȭ��ǥ ������ ������ ����
            }
            else
            {
                image.color = originalColor; // ���� �������� ����
            }
        }
        else if (image != null)
        {
            image.color = originalColor; // ���� �������� ����
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        //Destroy(gameObject);
    }
}
