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
        // ���콺�� ī�� ���� �ö��� �� ȣ��� �Լ�
        // ī�带 Ȯ���ϱ� ���� �ڵ�
        transform.localScale = originalScale * 1.2f; // ���÷� 1.2�� Ȯ��

        originalPosition = transform.localPosition;
        originalRotation = transform.localRotation;
        



        // ��ġ�� �ణ �ø���
        Vector3 newPosition = transform.localPosition + new Vector3(0f, 150f, -10f); // ���÷� y�� �������� 10��ŭ �ø�
        transform.localPosition = newPosition;

        transform.localRotation = Quaternion.identity;
        rectTransform.SetAsLastSibling();


    }

    public void OnPointerExit(PointerEventData eventData)
    {
        // ���콺�� ī�带 ���� �� ȣ��� �Լ�
        // Ȯ��� ī�带 ���� ũ��� �ǵ����� ���� �ڵ�
        transform.localScale = originalScale;
        transform.localPosition = originalPosition; // ���� ��ġ�� �ǵ�����
        transform.localRotation = originalRotation;
        rectTransform.SetSiblingIndex(originalSiblingIndex);


    }
}
