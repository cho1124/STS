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


    private List<GameObject> dots = new List<GameObject>(); // 생성된 점들
    
    private Vector2 startPoint; // 시작 지점

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
        if(thiscard.cost <= GameManager.instance.currentMana)
        {
            if (thiscard.type == "공격")
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
                        // 점 생성
                        GameObject dot = Instantiate(playerDeck.dotPrefab, arrowPrefab.transform);
                        dots.Add(dot);
                    }

                    // 점 위치 설정

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
        // UI 요소를 마우스 위치로 이동

        if(arrowPrefab != null)
        {
            
            

            arrowPrefab.transform.position = eventData.position;

            Vector2 direction = eventData.position - startPoint; //행복한 수학시간, 시작지점에서 현재 지점까지의 벡터값을 구해와서


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
                    AimedMonster = hit.collider.gameObject;
                    arrowPrefab_image.color = Color.red; // 화살표 색상을 빨갛게 변경
                    ChangeChildImageColors(arrowPrefab.transform, true);
                }
                
            }
            else if (arrowPrefab_image != null)
            {
                AimedMonster = null;
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


        if (cardDescription.Contains("피해를") && cardDescription.Contains("줍니다"))
        {
            if (cardDescription.Contains("현재 방어도만큼"))
            {
                effectType = CardEffectType.DealDamageBasedOnArmor;
            }
            else
            {
                effectType = CardEffectType.DealDamage;
            }
        }
        else if (cardDescription.Contains("취약을 부여합니다"))
        {
            effectType = CardEffectType.ApplyVulnerable;
        }
        else if (cardDescription.Contains("힘의 효과가 적용됩니다"))
        {
            effectType = CardEffectType.MultiplyStrengthEffect;
        }
        else if (cardDescription.Contains("카드를") && cardDescription.Contains("뽑습니다"))
        {
            effectType = CardEffectType.DrawCards;
        }
        else if (cardDescription.Contains("힘을") && cardDescription.Contains("얻습니다"))
        {
            effectType = CardEffectType.GainStrength;
        }
        else if (cardDescription.Contains("방어도를") && cardDescription.Contains("얻습니다"))
        {
            if (cardDescription.Contains("턴 종료시"))
            {
                effectType = CardEffectType.NoArmorExpiration;
            }
            else if (cardDescription.Contains("배로 증가합니다"))
            {
                effectType = CardEffectType.DoubleArmor;
            }
            else if (cardDescription.Contains("시 방어도가 사라지지 않습니다"))
            {
                effectType = CardEffectType.StartTurnNoArmorExpiration;
            }
            else
            {
                effectType = CardEffectType.GainArmor;
            }
        }
        else if (cardDescription.Contains("힘이") && cardDescription.Contains("증가합니다"))
        {
            if (cardDescription.Contains("매 턴 시작시"))
            {
                effectType = CardEffectType.IncreaseStrengthEveryTurn;
            }
            else if (cardDescription.Contains("배로 증가합니다"))
            {
                effectType = CardEffectType.MultiplyDamageEffect;
            }
            else
            {
                effectType = CardEffectType.IncreaseDefense;
            }
        }
        else if (cardDescription.Contains("치명타라면") && cardDescription.Contains("최대 체력이"))
        {
            effectType = CardEffectType.GainMaxHealthOnCritical;
        }
        else if (cardDescription.Contains("적 전체에게 피해를 줍니다"))
        {
            effectType = CardEffectType.DealDamageToAllEnemies;
        }
        else if (cardDescription.Contains("적의") && cardDescription.Contains("감소시킵니다"))
        {
            effectType = CardEffectType.ReduceOpponentStrength;
        }
        else if (cardDescription.Contains("턴 시작시 방어도가 사라지지 않습니다"))
        {
            effectType = CardEffectType.StartTurnNoArmorExpiration;
        }
        else if (cardDescription.Contains("소멸"))
        {
            // 소멸 처리 로직을 추가할 수 있습니다.
            // 예: effectType = CardEffectType.RemoveEffect;
            Debug.Log("소멸 효과 처리");
            effectType = CardEffectType.None; // 임시로 None으로 설정
        }
        else
        {
            effectType = CardEffectType.None;
            Debug.LogWarning("Unhandled card effect: " + cardDescription);
        }
    }


    // 설명에서 데미지 양을 추출하는 메서드 예시
    int ParseDamageFromDescription(string description)
    {
        int damage = 0;
        // 설명에서 데미지 양을 추출하는 로직을 구현
        // 예를 들어, "데미지: 5"와 같은 형식에서 숫자 부분을 추출하거나, 특정 패턴을 찾아서 값을 파싱할 수 있습니다.
        // 이 예시에서는 간단하게 5로 설정
        damage = 5;
        return damage;
    }

    // 상대에게 데미지를 가하는 메서드 예시
    void DealDamageToOpponent(int damageAmount)
    {
        // 상대에게 데미지를 주는 로직을 구현
        // 예를 들어, 상대의 체력을 감소시키는 등의 동작을 수행할 수 있습니다.
        Debug.Log("Deal " + damageAmount + " damage to opponent!");
    }

    // 설명에서 회복할 마나 양을 추출하는 메서드 예시
    int ParseManaRecoveryFromDescription(string description)
    {
        int mana = 0;
        // 설명에서 회복할 마나 양을 추출하는 로직을 구현
        // 예를 들어, "마나 회복: 2"와 같은 형식에서 숫자 부분을 추출하거나, 특정 패턴을 찾아서 값을 파싱할 수 있습니다.
        // 이 예시에서는 간단하게 2로 설정
        mana = 2;
        return mana;
    }

    // 마나를 회복하는 메서드 예시
    void RestoreMana(int manaAmount)
    {
        // 마나를 회복하는 로직을 구현
        // 예를 들어, 현재 플레이어의 마나를 증가시키는 등의 동작을 수행할 수 있습니다.
        Debug.Log("Restore " + manaAmount + " mana!");
    }

    // 설명에서 버프를 적용하는 메서드 예시
    void ApplyBuffToPlayer(string description)
    {
        // 버프를 적용하는 로직을 구현
        // 예를 들어, 플레이어에게 일시적인 강화를 부여하는 등의 동작을 수행할 수 있습니다.
        Debug.Log("Apply buff to player: " + description);
    }

    // 설명에서 카드를 뽑는 메서드 예시
    void DrawCards(string description)
    {
        // 카드를 뽑는 로직을 구현
        // 예를 들어, 플레이어의 덱에서 카드를 뽑는 등의 동작을 수행할 수 있습니다.
        Debug.Log("Draw cards: " + description);
    }

   void extinctionCards(string description)
    {
        Debug.Log("소멸됨: ");
    }





}






