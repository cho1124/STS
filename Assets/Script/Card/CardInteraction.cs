using UnityEngine;
using UnityEngine.EventSystems;

public class CardInteraction : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    
    private Vector3 originalScale;
    private Vector3 originalPosition;
    private Quaternion originalRotation;

    private RectTransform rectTransform;
    private int originalSiblingIndex;



    void Start()
    {
        originalScale = transform.localScale;

        rectTransform = GetComponent<RectTransform>();
        originalSiblingIndex = rectTransform.GetSiblingIndex();

    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        // 마우스가 카드 위에 올라갔을 때 호출될 함수
        // 카드를 확대하기 위한 코드
        transform.localScale = originalScale * 1.2f; // 예시로 1.2배 확대

        originalPosition = transform.localPosition;
        originalRotation = transform.localRotation;
        



        // 위치도 약간 올리기
        Vector3 newPosition = transform.localPosition + new Vector3(0f, 150f, -10f); // 예시로 y축 방향으로 10만큼 올림
        transform.localPosition = newPosition;

        transform.localRotation = Quaternion.identity;
        rectTransform.SetAsLastSibling();


    }

    public void OnPointerExit(PointerEventData eventData)
    {
        // 마우스가 카드를 떠날 때 호출될 함수
        // 확대된 카드를 원래 크기로 되돌리기 위한 코드
        transform.localScale = originalScale;
        transform.localPosition = originalPosition; // 원래 위치로 되돌리기
        transform.localRotation = originalRotation;
        rectTransform.SetSiblingIndex(originalSiblingIndex);


    }
}
