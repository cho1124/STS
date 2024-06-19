using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class CardInteraction : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
{

    public PlayerDeck playerDeck;
    
    private Vector3 originalScale;
    private Vector3 originalPosition;
    private Quaternion originalRotation;

    private RectTransform rectTransform;
    private int originalSiblingIndex;

    private bool isDragging = false;
    private Vector3 offsetToMouse;
    private GameObject arrowPrefab;

    private Image arrowPrefab_image;
    private Color arrowPrefab_originalColor;
    


    void Start()
    {
        playerDeck = FindObjectOfType<PlayerDeck>();

        originalScale = transform.localScale;
        //
        rectTransform = GetComponent<RectTransform>();
        originalSiblingIndex = rectTransform.GetSiblingIndex();
        //playerDeck.Initializecard(transform, out originalScale, out originalSiblingIndex, out rectTransform);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        //// 마우스가 카드 위에 올라갔을 때 호출될 함수
        //// 카드를 확대하기 위한 코드
        
        originalPosition = transform.localPosition;
        originalRotation = transform.localRotation;

        playerDeck.EnlargeCard(gameObject, originalScale, originalPosition, new Vector3(0f, 180f, -10f), 1.2f, rectTransform);
    }


    public void OnPointerExit(PointerEventData eventData)
    {
        
        playerDeck.RestoreCard(gameObject, originalScale, originalPosition, originalRotation, originalSiblingIndex, rectTransform);
    }

    

    public void OnPointerDown(PointerEventData eventData)
    {
        // 드래그 시작 시 프리팹을 인스턴스화하거나 다른 작업을 수행합니다.
        
        Debug.Log("Drag started!");
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        // 드래그 종료 시 프리팹을 제거하거나 다른 작업을 수행합니다.
        //if (arrowPrefab != null)
        //{
        //    Destroy(arrowPrefab);
        //}
        Debug.Log("Drag ended!");
    }

    public void OnBeginDrag(PointerEventData eventData)
    {

        arrowPrefab = Instantiate(playerDeck.arrowPrefab);
        arrowPrefab.transform.SetParent(playerDeck.CombatCanvas.transform);
        arrowPrefab.transform.position = eventData.position;
        arrowPrefab_image = arrowPrefab.GetComponent<Image>();

        arrowPrefab_originalColor = arrowPrefab.GetComponent<Image>().color;

        


        //oldPosition = transform.position;

        //Debug.Log(oldPosition);
    }

    public void OnDrag(PointerEventData eventData)
    {
        // UI 요소를 마우스 위치로 이동
        arrowPrefab.transform.position = eventData.position;
        //Debug.Log(transform.position);

        // 마우스 위치에서 Raycast를 수행하여 2D 적 오브젝트 찾기
        Vector2 worldPoint = Camera.main.ScreenToWorldPoint(eventData.position);
        RaycastHit2D hit = Physics2D.Raycast(worldPoint, Vector2.zero);

        if (hit.collider != null && arrowPrefab_image != null)
        {
            // 적 오브젝트를 감지
            if (hit.collider.CompareTag("Enemy"))
            {
                Debug.Log("Enemy Detected: " + hit.collider.gameObject.name);
                arrowPrefab_image.color = Color.red; // 화살표 색상을 빨갛게 변경
            }
            else
            {
                arrowPrefab_image.color = arrowPrefab_originalColor; // 원래 색상으로 복원
            }
        }
        else if (arrowPrefab_image != null)
        {
            arrowPrefab_image.color = arrowPrefab_originalColor; // 원래 색상으로 복원
        }


    }
    public void OnEndDrag(PointerEventData eventData)
    {
        Destroy(arrowPrefab);
    }




}
