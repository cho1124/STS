using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Draggerble : MonoBehaviour,  IBeginDragHandler, IDragHandler, IEndDragHandler ,IPointerDownHandler, IPointerUpHandler
{

    private Vector3 oldPosition;


    public void OnBeginDrag(PointerEventData eventData)
    {

        //oldPosition = transform.position;

        Debug.Log(oldPosition);
    } 

    public void OnDrag(PointerEventData eventData)
    {
        //transform.position = eventData.position;
        Debug.Log(transform.position);
        



    }
    public void OnEndDrag(PointerEventData eventData)
    {
        //transform.position = oldPosition;
        Debug.Log(oldPosition);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        
        Debug.Log("clicked!");
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        Debug.Log("clicked up!");
    }




    


}
