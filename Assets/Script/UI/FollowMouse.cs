using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;




public class FollowMouse : MonoBehaviour
{
    
    private Image image;
    private Color originalColor;

    void Start()
    {
        
        image = GetComponent<Image>();

        
    }

   

    public void OnEndDrag(PointerEventData eventData)
    {
        //Destroy(gameObject);
    }
}
