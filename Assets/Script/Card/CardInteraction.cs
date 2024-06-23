using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Net;
using DG.Tweening;
using System;

public class CardInteraction : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
{

    public PlayerDeck playerDeck;
    public GameObject AimedMonster;


    private Vector3 originalScale;
    private Vector3 originalPosition;
    private Quaternion originalRotation;

    private RectTransform rectTransform;
    private int originalSiblingIndex;

    
    private GameObject arrowPrefab;

    private Image arrowPrefab_image;
    private Color arrowPrefab_originalColor;


    private List<GameObject> dots = new List<GameObject>(); // ������ ����
    
    private Vector2 startPoint; // ���� ����

    private int dotCount = 10;

    private LineRenderer lineRenderer;
    private ThisCard thiscard;
    //private PlayerDeck playerDeck;
    
    
    public string cardDescription;

    void Start()
    {
        playerDeck = FindObjectOfType<PlayerDeck>();
        
        thiscard = GetComponent<ThisCard>();
        
        originalScale = transform.localScale;
        //
        rectTransform = GetComponent<RectTransform>();
        originalSiblingIndex = rectTransform.GetSiblingIndex();

        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = dotCount + 1;
        ;
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

        if (thiscard.effectTypes.Contains(CardEffectType.DealDamage) || thiscard.effectTypes.Contains(CardEffectType.ReduceOpponentStrength))
        {
            //transform.position = eventData.position;


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
        else
        {
            transform.position = eventData.position;
        }



    }

    public void OnDrag(PointerEventData eventData)
    {
        // UI ��Ҹ� ���콺 ��ġ�� �̵�

        
        if(arrowPrefab != null)
        {
            
            

            arrowPrefab.transform.position = eventData.position;

            Vector2 direction = eventData.position - startPoint; //�ູ�� ���нð�, ������������ ���� ���������� ���Ͱ��� ���ؿͼ�


            arrowPrefab.transform.up = direction;


            //Debug.Log(direction);
            // ���콺 ��ġ���� Raycast�� �����Ͽ� 2D �� ������Ʈ ã��
            Vector2 worldPoint = Camera.main.ScreenToWorldPoint(eventData.position);
            RaycastHit2D hit = Physics2D.Raycast(worldPoint, Vector2.zero);

            if (hit.collider != null)
            {
                // �� ������Ʈ�� ����
                if (hit.collider.CompareTag("Enemy"))
                {
                    Debug.Log("Enemy Detected: " + hit.collider.gameObject.name);
                    AimedMonster = hit.collider.gameObject;
                    arrowPrefab_image.color = Color.red; // ȭ��ǥ ������ ������ ����
                    ChangeChildImageColors(arrowPrefab.transform, true);
                }
                
            }
            else if (arrowPrefab_image != null)
            {
                AimedMonster = null;
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
        else
        {
            transform.position = eventData.position;
        }



    }
    public void OnEndDrag(PointerEventData eventData)
    {

        if(arrowPrefab != null)
        {
            Destroy(arrowPrefab);
            foreach (GameObject dot in dots)
            {
                Destroy(dot);
            }
            dots.Clear();
            if(AimedMonster != null)
            {
                Debug.Log(AimedMonster.name);

                if (GameManager.instance.currentMana < thiscard.cost)
                {
                    Debug.Log("ī�带 ����� �� �����ϴ�.");
                    //transform.Translate(originalPosition);
                }
                else
                {
                    GameManager.instance.currentMana -= thiscard.cost;
                    playerDeck.DiscardCard(gameObject);
                    ApplyCardEffect(AimedMonster.GetComponent<Monster>());
                }


                
            }
            


        }
        else
        {

            if(transform.position.y > 400)
            {
                if (GameManager.instance.currentMana < thiscard.cost)
                {
                    Debug.Log("ī�带 ����� �� �����ϴ�.");
                    transform.Translate(originalPosition);
                    playerDeck.cardAllignment();
                }
                else
                {
                    GameManager.instance.currentMana -= thiscard.cost;
                    ApplyCardEffect(AimedMonster.GetComponent<Monster>());

                    foreach(CardEffectType cardEffectType in thiscard.effectTypes)
                    {

                    }
                    if(thiscard.effectTypes.Contains(CardEffectType.Deletable))
                    {
                        //�Ҹ�ĭ����
                    }

                    playerDeck.DiscardCard(gameObject);
                }

                

                
            }
            else
            {
                //transform.position = originalPosition;
                transform.Translate(originalPosition);
                playerDeck.cardAllignment();
            }

            
        }

        


        
    }

    private void ApplyCardEffectToMonster(ThisCard thisCard, Monster target)
    {
        //if (cardEffectHandler != null)
        //{
        //    cardEffectHandler.ApplyCardEffect(card, target);
        //}
        //else
        //{
        //    Debug.LogWarning("CardEffectHandler not found!");
        //}

        //if(thiscard.effectTypes)

        Debug.Log("applyed!");


    }

    private void ApplyCardEffect(Monster monster)
    {
        for (int i = 0; i < thiscard.effectTypes.Count; i++)
        {
            CardEffectType cardEffectType = thiscard.effectTypes[i];
            int effectValue = thiscard.effectValues[i];

            switch (cardEffectType)
            {
                case CardEffectType.None:
                    // �ƹ��� ȿ���� ����
                    break;
                case CardEffectType.DealDamage:
                    DealDamage(effectValue, monster);
                    break;
                case CardEffectType.ApplyVulnerable:
                    ApplyVulnerable(effectValue, monster);
                    break;
                case CardEffectType.MultiplyStrengthEffect:
                    MultiplyStrengthEffect(effectValue);
                    break;
                case CardEffectType.DrawCards:
                    DrawCards(effectValue);
                    break;
                case CardEffectType.GainStrength:
                    GainStrength(effectValue);
                    break;
                case CardEffectType.DealDamageBasedOnArmor:
                    DealDamageBasedOnArmor(effectValue, monster);
                    break;
                case CardEffectType.NoArmorExpiration:
                    NoArmorExpiration(effectValue);
                    break;
                case CardEffectType.DoubleArmor:
                    DoubleArmor(effectValue);
                    break;
                case CardEffectType.StartTurnNoArmorExpiration:
                    StartTurnNoArmorExpiration(effectValue);
                    break;
                case CardEffectType.GainArmor:
                    GainArmor(effectValue);
                    break;
                case CardEffectType.IncreaseStrengthEveryTurn:
                    IncreaseStrengthEveryTurn(effectValue);
                    break;
                case CardEffectType.MultiplyDamageEffect:
                    MultiplyDamageEffect(effectValue);
                    break;
                case CardEffectType.IncreaseDefense:
                    IncreaseDefense(effectValue);
                    break;
                case CardEffectType.GainMaxHealthOnCritical:
                    GainMaxHealthOnCritical(effectValue);
                    break;
                case CardEffectType.DealDamageToAllEnemies:
                    DealDamageToAllEnemies(effectValue);
                    break;
                case CardEffectType.ReduceOpponentStrength:
                    ReduceOpponentStrength(effectValue, monster);
                    break;
                case CardEffectType.MutipleAttack:
                    MultipleAttack(effectValue, monster);
                    break;
                case CardEffectType.ReduceStrength:
                    ReduceStrength(effectValue, monster);
                    break;
                case CardEffectType.EveryTurnStart:
                    EveryTurnStart(effectValue);
                    break;
                case CardEffectType.EveryTurnEnd:
                    EveryTurnEnd(effectValue);
                    break;
                case CardEffectType.GainEnergy:
                    GainEnergy(effectValue);
                    break;
                case CardEffectType.ReduceHealth:
                    ReduceHealth(effectValue, monster);
                    break;
                case CardEffectType.DoubleStrength:
                    DoubleStrength(effectValue);
                    break;
                case CardEffectType.Deletable:
                    Deletable();
                    break;
            }
        }
    }

    // �� ȿ���� ���� ��ü���� �޼��带 �����մϴ�.
    private void DealDamage(int value, Monster monster)
    {
        monster.TakeDamage(value);

        // ���ظ� �ִ� ���� ����
    }

    private void ApplyVulnerable(int value, Monster monster)
    {
        VulnerabilityEffect vulnerabilityEffect = ScriptableObject.CreateInstance<VulnerabilityEffect>();
        vulnerabilityEffect.effectName = "Vulnerability";
        vulnerabilityEffect.duration = value; // ���� �ð����� ���
        //vulnerabilityEffect.defenseReduction = value; // ���� ���ҷ����� ���

        monster.ApplyStatusEffect(vulnerabilityEffect);
    }
    private void MultiplyStrengthEffect(int value)
    {
        // ���� ȿ���� ������Ű�� ���� ����
    }

    private void DrawCards(int value)
    {
        // ī�带 �̴� ���� ����
    }

    private void GainStrength(int value)
    {
        // ���� ��� ���� ����
    }

    private void DealDamageBasedOnArmor(int value, Monster monster)
    {
        // ���� ����ŭ ���ظ� �ִ� ���� ����
    }

    private void NoArmorExpiration(int value)
    {
        // �� ���Ḧ ���ִ� ���� ����
    }

    private void DoubleArmor(int value)
    {
        // ���� �� ��� ������Ű�� ���� ����
    }

    private void StartTurnNoArmorExpiration(int value)
    {
        // �� ���� �� �� ���Ḧ ���ִ� ���� ����
    }

    private void GainArmor(int value)
    {
        // ���� ��� ���� ����
    }

    private void IncreaseStrengthEveryTurn(int value)
    {
        // �� �ϸ��� ���� �����ϴ� ���� ����
    }

    private void MultiplyDamageEffect(int value)
    {
        // ���� ȿ���� �谡�ϴ� ���� ����
    }

    private void IncreaseDefense(int value)
    {
        // ��ø�� ������Ű�� ���� ����
    }

    private void GainMaxHealthOnCritical(int value)
    {
        // ġ��Ÿ �� �ִ� ü���� ��� ���� ����
    }

    private void DealDamageToAllEnemies(int value)
    {
        // ��� ������ ���ظ� �ִ� ���� ����
    }

    private void ReduceOpponentStrength(int value, Monster monster)
    {
        // ���� ���� ���ҽ�Ű�� ���� ����
    }

    private void MultipleAttack(int value, Monster monster)
    {
        // ���� ���� ���� ����
    }

    private void ReduceStrength(int value, Monster monster)
    {
        // �� ���� ���� ����
    }

    private void EveryTurnStart(int value)
    {
        // �� �� ���� ���� ���� ����
    }

    private void EveryTurnEnd(int value)
    {
        // �� �� ���� ���� ���� ����
    }

    private void GainEnergy(int value)
    {
        // �������� ��� ���� ����
    }

    private void ReduceHealth(int value, Monster monster)
    {
        // ü�� ���� ���� ����
    }

    private void DoubleStrength(int value)
    {
        // ���� �� ��� ������Ű�� ���� ����
    }

    private void Deletable()
    {
        // ī�� ���� ���� ���� ����
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

    void GetDamage()
    {
        Monster monster = AimedMonster.GetComponent<Monster>();
        //monster.currentHealth -= thiscard.
    }

    




}






