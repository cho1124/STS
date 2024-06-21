using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Net;
using DG.Tweening;


public enum CardEffectType
{
    None,
    DealDamage,
    ApplyVulnerable,
    DealDamageBasedOnArmor,
    MultiplyStrengthEffect,
    DrawCards,
    ApplyBuffToPlayer,
    GainArmor,
    GainStrength,
    GainMana,
    ReduceOpponentStrength,
    DoubleArmor,
    NoArmorExpiration,
    IncreaseDefense,
    StartTurnNoArmorExpiration,
    IncreaseStrengthEveryTurn,
    GainMaxHealthOnCritical,
    DealDamageToAllEnemies,
    MultiplyDamageEffect,
    ApplyCriticalDamageEffect,
    GainEnergy,
    LoseHealth,
    EnemyMovement,
    PhysicalAttack,
    RemoveShieldEffect
}


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
    public CardEffectType effectType;
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
        if(thiscard.cost <= GameManager.instance.currentMana)
        {
            if (thiscard.type == "����")
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


    }

    public void OnDrag(PointerEventData eventData)
    {
        // UI ��Ҹ� ���콺 ��ġ�� �̵�

        if(arrowPrefab != null)
        {
            
            

            arrowPrefab.transform.position = eventData.position;

            Vector2 direction = eventData.position - startPoint; //�ູ�� ���нð�, ������������ ���� ���������� ���Ͱ��� ���ؿͼ�


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
        }
        else
        {

            if(transform.position.y > 400)
            {
                playerDeck.DiscardCard(gameObject);
            }
            else
            {
                //transform.position = originalPosition;
                transform.Translate(originalPosition);
            }

            
        }

        
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
    public void ProcessCardEffect()
    {
        switch (effectType)
        {
            case CardEffectType.DealDamage:
                //DealDamage();
                break;
            case CardEffectType.ApplyVulnerable:
                //ApplyVulnerable();
                break;
            case CardEffectType.DealDamageBasedOnArmor:
                //DealDamageBasedOnArmor();
                break;
            case CardEffectType.MultiplyStrengthEffect:
                //MultiplyStrengthEffect();
                break;
            case CardEffectType.DrawCards:
                //DrawCards();
                break;
            case CardEffectType.ApplyBuffToPlayer:
                //ApplyBuffToPlayer();
                break;
            case CardEffectType.GainArmor:
                //GainArmor();
                break;
            case CardEffectType.GainStrength:
                //GainStrength();
                break;
            case CardEffectType.GainMana:
                //GainMana();
                break;
            case CardEffectType.ReduceOpponentStrength:
                //ReduceOpponentStrength();
                break;
            case CardEffectType.DoubleArmor:
                //DoubleArmor();
                break;
            case CardEffectType.NoArmorExpiration:
                //NoArmorExpiration();
                break;
            case CardEffectType.IncreaseDefense:
                //IncreaseDefense();
                break;
            case CardEffectType.StartTurnNoArmorExpiration:
                //StartTurnNoArmorExpiration();
                break;
            case CardEffectType.IncreaseStrengthEveryTurn:
                //IncreaseStrengthEveryTurn();
                break;
            case CardEffectType.GainMaxHealthOnCritical:
                //GainMaxHealthOnCritical();
                break;
            case CardEffectType.DealDamageToAllEnemies:
                //DealDamageToAllEnemies();
                break;
            case CardEffectType.MultiplyDamageEffect:
                //MultiplyDamageEffect();
                break;
            case CardEffectType.ApplyCriticalDamageEffect:
                //ApplyCriticalDamageEffect();
                break;
            case CardEffectType.GainEnergy:
                //GainEnergy();
                break;
            case CardEffectType.LoseHealth:
                //LoseHealth();
                break;
            case CardEffectType.EnemyMovement:
                //EnemyMovement();
                break;
            case CardEffectType.PhysicalAttack:
                //PhysicalAttack();
                break;
            case CardEffectType.RemoveShieldEffect:
                //RemoveShieldEffect();
                break;
            default:
                Debug.LogWarning("Unhandled card effect: " + cardDescription);
                break;
        }
    }
    private void DetermineEffectType()
    {
        cardDescription = thiscard.cardDescription;


        if (cardDescription.Contains("���ظ�") && cardDescription.Contains("�ݴϴ�"))
        {
            if (cardDescription.Contains("���� ����ŭ"))
            {
                effectType = CardEffectType.DealDamageBasedOnArmor;
            }
            else
            {
                effectType = CardEffectType.DealDamage;
            }
        }
        else if (cardDescription.Contains("����� �ο��մϴ�"))
        {
            effectType = CardEffectType.ApplyVulnerable;
        }
        else if (cardDescription.Contains("���� ȿ���� ����˴ϴ�"))
        {
            effectType = CardEffectType.MultiplyStrengthEffect;
        }
        else if (cardDescription.Contains("ī�带") && cardDescription.Contains("�̽��ϴ�"))
        {
            effectType = CardEffectType.DrawCards;
        }
        else if (cardDescription.Contains("����") && cardDescription.Contains("����ϴ�"))
        {
            effectType = CardEffectType.GainStrength;
        }
        else if (cardDescription.Contains("����") && cardDescription.Contains("����ϴ�"))
        {
            if (cardDescription.Contains("�� �����"))
            {
                effectType = CardEffectType.NoArmorExpiration;
            }
            else if (cardDescription.Contains("��� �����մϴ�"))
            {
                effectType = CardEffectType.DoubleArmor;
            }
            else if (cardDescription.Contains("�� ���� ������� �ʽ��ϴ�"))
            {
                effectType = CardEffectType.StartTurnNoArmorExpiration;
            }
            else
            {
                effectType = CardEffectType.GainArmor;
            }
        }
        else if (cardDescription.Contains("����") && cardDescription.Contains("�����մϴ�"))
        {
            if (cardDescription.Contains("�� �� ���۽�"))
            {
                effectType = CardEffectType.IncreaseStrengthEveryTurn;
            }
            else if (cardDescription.Contains("��� �����մϴ�"))
            {
                effectType = CardEffectType.MultiplyDamageEffect;
            }
            else
            {
                effectType = CardEffectType.IncreaseDefense;
            }
        }
        else if (cardDescription.Contains("ġ��Ÿ���") && cardDescription.Contains("�ִ� ü����"))
        {
            effectType = CardEffectType.GainMaxHealthOnCritical;
        }
        else if (cardDescription.Contains("�� ��ü���� ���ظ� �ݴϴ�"))
        {
            effectType = CardEffectType.DealDamageToAllEnemies;
        }
        else if (cardDescription.Contains("����") && cardDescription.Contains("���ҽ�ŵ�ϴ�"))
        {
            effectType = CardEffectType.ReduceOpponentStrength;
        }
        else if (cardDescription.Contains("�� ���۽� ���� ������� �ʽ��ϴ�"))
        {
            effectType = CardEffectType.StartTurnNoArmorExpiration;
        }
        else if (cardDescription.Contains("�Ҹ�"))
        {
            // �Ҹ� ó�� ������ �߰��� �� �ֽ��ϴ�.
            // ��: effectType = CardEffectType.RemoveEffect;
            Debug.Log("�Ҹ� ȿ�� ó��");
            effectType = CardEffectType.None; // �ӽ÷� None���� ����
        }
        else
        {
            effectType = CardEffectType.None;
            Debug.LogWarning("Unhandled card effect: " + cardDescription);
        }
    }


    // ������ ������ ���� �����ϴ� �޼��� ����
    int ParseDamageFromDescription(string description)
    {
        int damage = 0;
        // ������ ������ ���� �����ϴ� ������ ����
        // ���� ���, "������: 5"�� ���� ���Ŀ��� ���� �κ��� �����ϰų�, Ư�� ������ ã�Ƽ� ���� �Ľ��� �� �ֽ��ϴ�.
        // �� ���ÿ����� �����ϰ� 5�� ����
        damage = 5;
        return damage;
    }

    // ��뿡�� �������� ���ϴ� �޼��� ����
    void DealDamageToOpponent(int damageAmount)
    {
        // ��뿡�� �������� �ִ� ������ ����
        // ���� ���, ����� ü���� ���ҽ�Ű�� ���� ������ ������ �� �ֽ��ϴ�.
        Debug.Log("Deal " + damageAmount + " damage to opponent!");
    }

    // ������ ȸ���� ���� ���� �����ϴ� �޼��� ����
    int ParseManaRecoveryFromDescription(string description)
    {
        int mana = 0;
        // ������ ȸ���� ���� ���� �����ϴ� ������ ����
        // ���� ���, "���� ȸ��: 2"�� ���� ���Ŀ��� ���� �κ��� �����ϰų�, Ư�� ������ ã�Ƽ� ���� �Ľ��� �� �ֽ��ϴ�.
        // �� ���ÿ����� �����ϰ� 2�� ����
        mana = 2;
        return mana;
    }

    // ������ ȸ���ϴ� �޼��� ����
    void RestoreMana(int manaAmount)
    {
        // ������ ȸ���ϴ� ������ ����
        // ���� ���, ���� �÷��̾��� ������ ������Ű�� ���� ������ ������ �� �ֽ��ϴ�.
        Debug.Log("Restore " + manaAmount + " mana!");
    }

    // ������ ������ �����ϴ� �޼��� ����
    void ApplyBuffToPlayer(string description)
    {
        // ������ �����ϴ� ������ ����
        // ���� ���, �÷��̾�� �Ͻ����� ��ȭ�� �ο��ϴ� ���� ������ ������ �� �ֽ��ϴ�.
        Debug.Log("Apply buff to player: " + description);
    }

    // ������ ī�带 �̴� �޼��� ����
    void DrawCards(string description)
    {
        // ī�带 �̴� ������ ����
        // ���� ���, �÷��̾��� ������ ī�带 �̴� ���� ������ ������ �� �ֽ��ϴ�.
        Debug.Log("Draw cards: " + description);
    }

   void extinctionCards(string description)
    {
        Debug.Log("�Ҹ��: ");
    }





}






