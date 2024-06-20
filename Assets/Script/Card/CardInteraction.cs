using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Net;


public class CardInteraction : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
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


    private List<GameObject> dots = new List<GameObject>(); // ������ ����
    private Vector2 startPoint; // ���� ����

    private int dotCount = 20;

    private LineRenderer lineRenderer;

    void Start()
    {
        playerDeck = FindObjectOfType<PlayerDeck>();

        originalScale = transform.localScale;
        //
        rectTransform = GetComponent<RectTransform>();
        originalSiblingIndex = rectTransform.GetSiblingIndex();

        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = dotCount + 1;

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

    

    

    public void OnBeginDrag(PointerEventData eventData)
    {

        arrowPrefab = Instantiate(playerDeck.arrowPrefab);
        arrowPrefab.transform.SetParent(playerDeck.CombatCanvas.transform);
        arrowPrefab.transform.position = eventData.position;
        arrowPrefab_image = arrowPrefab.GetComponent<Image>();
        arrowPrefab_originalColor = arrowPrefab.GetComponent<Image>().color;

        startPoint = eventData.position;

        for (int i = 0; i < dotCount; i++)
        {
            if (i >= dots.Count)
            {
                // �� ����
                GameObject dot = Instantiate(playerDeck.dotPrefab, arrowPrefab.transform);
                dots.Add(dot);
            }
        
            // �� ��ġ ����
            
        }
        
        
    }

    public void OnDrag(PointerEventData eventData)
    {
        // UI ��Ҹ� ���콺 ��ġ�� �̵�
        arrowPrefab.transform.position = eventData.position;
        //Debug.Log($"Start position : {startPoint}, Current position : {eventData.position}");
        Vector2 direction =  eventData.position - startPoint; //�ູ�� ���нð�, ������������ ���� ���������� ���Ͱ��� ���ؿͼ�
        //Debug.Log($"Direction vector: {direction}");
        
        arrowPrefab.transform.up = direction.normalized;


        //Debug.Log(direction);
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
                ChangeChildImageColors(arrowPrefab.transform, true);
            }
            else
            {
                arrowPrefab_image.color = arrowPrefab_originalColor; // ���� �������� ����
                
            }
        }
        else if (arrowPrefab_image != null)
        {
            arrowPrefab_image.color = arrowPrefab_originalColor; // ���� �������� ����
            ChangeChildImageColors(arrowPrefab.transform, false);
        }

        
        

        for (int i = 0; i < dotCount; i++)
        {
            float t = (float)i / (dotCount + 0.5f);
            Vector2 dotPosition = BezierCurve(startPoint, new Vector2(startPoint.x, eventData.position.y), eventData.position, t);
            
            dots[i].transform.position = dotPosition;
            dots[i].transform.up = eventData.position - dotPosition;
            dots[i].SetActive(true);
        }



    }
    public void OnEndDrag(PointerEventData eventData)
    {
        Destroy(arrowPrefab);
        foreach (GameObject dot in dots)
        {
            Destroy(dot);
        }
        dots.Clear();
    }

    Vector2 BezierCurve(Vector2 startPoint, Vector2 controlPoint, Vector2 endPoint, float t)
    {
        float u = 1 - t;
        float tt = t * t;
        float uu = u * u;

        Vector2 point = uu * startPoint + 2 * u * t * controlPoint + tt * endPoint;

        return point;
    }

    void ChangeChildImageColors(Transform parent, bool isDected)
    {
        // �θ� ������Ʈ�� ��� �ڽ��� ��ȸ�ϸ鼭 Image ������Ʈ�� ������ ���� ����
        foreach (Transform child in parent)
        {
            Image image = child.GetComponent<Image>();
            if (image != null)
            {
                if(isDected)
                {
                    image.color = Color.red; // �̹����� ���� ����
                }
                else
                {
                    //Debug.Log("isDected is false");
                    image.color = arrowPrefab_originalColor;
                }

                
            }

            
        }

    }
}






