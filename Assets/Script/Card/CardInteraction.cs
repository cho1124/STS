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
        //// ���콺�� ī�� ���� �ö��� �� ȣ��� �Լ�
        //// ī�带 Ȯ���ϱ� ���� �ڵ�
        
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
        // �巡�� ���� �� �������� �ν��Ͻ�ȭ�ϰų� �ٸ� �۾��� �����մϴ�.
        
        Debug.Log("Drag started!");
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        // �巡�� ���� �� �������� �����ϰų� �ٸ� �۾��� �����մϴ�.
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
        // UI ��Ҹ� ���콺 ��ġ�� �̵�
        arrowPrefab.transform.position = eventData.position;
        //Debug.Log(transform.position);

        // ���콺 ��ġ���� Raycast�� �����Ͽ� 2D �� ������Ʈ ã��
        Vector2 worldPoint = Camera.main.ScreenToWorldPoint(eventData.position);
        RaycastHit2D hit = Physics2D.Raycast(worldPoint, Vector2.zero);

        if (hit.collider != null && arrowPrefab_image != null)
        {
            // �� ������Ʈ�� ����
            if (hit.collider.CompareTag("Enemy"))
            {
                Debug.Log("Enemy Detected: " + hit.collider.gameObject.name);
                arrowPrefab_image.color = Color.red; // ȭ��ǥ ������ ������ ����
            }
            else
            {
                arrowPrefab_image.color = arrowPrefab_originalColor; // ���� �������� ����
            }
        }
        else if (arrowPrefab_image != null)
        {
            arrowPrefab_image.color = arrowPrefab_originalColor; // ���� �������� ����
        }


    }
    public void OnEndDrag(PointerEventData eventData)
    {
        Destroy(arrowPrefab);
    }




}
