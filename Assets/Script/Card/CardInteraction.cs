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


    private List<GameObject> dots = new List<GameObject>(); // 생성된 점들
    private Vector2 startPoint; // 시작 지점

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
                // 점 생성
                GameObject dot = Instantiate(playerDeck.dotPrefab, arrowPrefab.transform);
                dots.Add(dot);
            }
        
            // 점 위치 설정
            
        }
        
        
    }

    public void OnDrag(PointerEventData eventData)
    {
        // UI 요소를 마우스 위치로 이동
        arrowPrefab.transform.position = eventData.position;
        //Debug.Log($"Start position : {startPoint}, Current position : {eventData.position}");
        Vector2 direction =  eventData.position - startPoint; //행복한 수학시간, 시작지점에서 현재 지점까지의 벡터값을 구해와서
        //Debug.Log($"Direction vector: {direction}");
        
        arrowPrefab.transform.up = direction.normalized;


        //Debug.Log(direction);
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
                ChangeChildImageColors(arrowPrefab.transform, true);
            }
            else
            {
                arrowPrefab_image.color = arrowPrefab_originalColor; // 원래 색상으로 복원
                
            }
        }
        else if (arrowPrefab_image != null)
        {
            arrowPrefab_image.color = arrowPrefab_originalColor; // 원래 색상으로 복원
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
        // 부모 오브젝트의 모든 자식을 순회하면서 Image 컴포넌트가 있으면 색상 변경
        foreach (Transform child in parent)
        {
            Image image = child.GetComponent<Image>();
            if (image != null)
            {
                if(isDected)
                {
                    image.color = Color.red; // 이미지의 색상 변경
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






