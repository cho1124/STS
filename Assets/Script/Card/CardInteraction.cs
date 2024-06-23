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


    private List<GameObject> dots = new List<GameObject>(); // 생성된 점들
    
    private Vector2 startPoint; // 시작 지점

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

    public void OnDrag(PointerEventData eventData)
    {
        // UI 요소를 마우스 위치로 이동

        
        if(arrowPrefab != null)
        {
            
            

            arrowPrefab.transform.position = eventData.position;

            Vector2 direction = eventData.position - startPoint; //행복한 수학시간, 시작지점에서 현재 지점까지의 벡터값을 구해와서


            arrowPrefab.transform.up = direction;


            //Debug.Log(direction);
            // 마우스 위치에서 Raycast를 수행하여 2D 적 오브젝트 찾기
            Vector2 worldPoint = Camera.main.ScreenToWorldPoint(eventData.position);
            RaycastHit2D hit = Physics2D.Raycast(worldPoint, Vector2.zero);

            if (hit.collider != null)
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
            if(AimedMonster != null)
            {
                Debug.Log(AimedMonster.name);

                if (GameManager.instance.currentMana < thiscard.cost)
                {
                    Debug.Log("카드를 사용할 수 없습니다.");
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
                    Debug.Log("카드를 사용할 수 없습니다.");
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
                        //소멸칸으로
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
                    // 아무런 효과도 없음
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

    // 각 효과에 대한 구체적인 메서드를 정의합니다.
    private void DealDamage(int value, Monster monster)
    {
        monster.TakeDamage(value);

        // 피해를 주는 로직 구현
    }

    private void ApplyVulnerable(int value, Monster monster)
    {
        VulnerabilityEffect vulnerabilityEffect = ScriptableObject.CreateInstance<VulnerabilityEffect>();
        vulnerabilityEffect.effectName = "Vulnerability";
        vulnerabilityEffect.duration = value; // 지속 시간으로 사용
        //vulnerabilityEffect.defenseReduction = value; // 방어력 감소량으로 사용

        monster.ApplyStatusEffect(vulnerabilityEffect);
    }
    private void MultiplyStrengthEffect(int value)
    {
        // 힘의 효과를 증폭시키는 로직 구현
    }

    private void DrawCards(int value)
    {
        // 카드를 뽑는 로직 구현
    }

    private void GainStrength(int value)
    {
        // 힘을 얻는 로직 구현
    }

    private void DealDamageBasedOnArmor(int value, Monster monster)
    {
        // 현재 방어도만큼 피해를 주는 로직 구현
    }

    private void NoArmorExpiration(int value)
    {
        // 방어도 만료를 없애는 로직 구현
    }

    private void DoubleArmor(int value)
    {
        // 방어도를 두 배로 증가시키는 로직 구현
    }

    private void StartTurnNoArmorExpiration(int value)
    {
        // 턴 시작 시 방어도 만료를 없애는 로직 구현
    }

    private void GainArmor(int value)
    {
        // 방어도를 얻는 로직 구현
    }

    private void IncreaseStrengthEveryTurn(int value)
    {
        // 매 턴마다 힘이 증가하는 로직 구현
    }

    private void MultiplyDamageEffect(int value)
    {
        // 피해 효과를 배가하는 로직 구현
    }

    private void IncreaseDefense(int value)
    {
        // 민첩을 증가시키는 로직 구현
    }

    private void GainMaxHealthOnCritical(int value)
    {
        // 치명타 시 최대 체력을 얻는 로직 구현
    }

    private void DealDamageToAllEnemies(int value)
    {
        // 모든 적에게 피해를 주는 로직 구현
    }

    private void ReduceOpponentStrength(int value, Monster monster)
    {
        // 적의 힘을 감소시키는 로직 구현
    }

    private void MultipleAttack(int value, Monster monster)
    {
        // 다중 공격 로직 구현
    }

    private void ReduceStrength(int value, Monster monster)
    {
        // 힘 감소 로직 구현
    }

    private void EveryTurnStart(int value)
    {
        // 매 턴 시작 시의 로직 구현
    }

    private void EveryTurnEnd(int value)
    {
        // 매 턴 끝날 시의 로직 구현
    }

    private void GainEnergy(int value)
    {
        // 에너지를 얻는 로직 구현
    }

    private void ReduceHealth(int value, Monster monster)
    {
        // 체력 감소 로직 구현
    }

    private void DoubleStrength(int value)
    {
        // 힘을 두 배로 증가시키는 로직 구현
    }

    private void Deletable()
    {
        // 카드 삭제 가능 로직 구현
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

    




}






