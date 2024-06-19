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
        // UI 요소를 마우스 위치로 이동
        transform.position = eventData.position;
        //Debug.Log(transform.position);

        // 마우스 위치에서 Raycast를 수행하여 2D 적 오브젝트 찾기
        Vector2 worldPoint = Camera.main.ScreenToWorldPoint(eventData.position);
        RaycastHit2D hit = Physics2D.Raycast(worldPoint, Vector2.zero);

        if (hit.collider != null && image != null)
        {
            // 적 오브젝트를 감지
            if (hit.collider.CompareTag("Enemy"))
            {
                Debug.Log("Enemy Detected: " + hit.collider.gameObject.name);
                image.color = Color.red; // 화살표 색상을 빨갛게 변경
            }
            else
            {
                image.color = originalColor; // 원래 색상으로 복원
            }
        }
        else if (image != null)
        {
            image.color = originalColor; // 원래 색상으로 복원
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        //Destroy(gameObject);
    }
}
