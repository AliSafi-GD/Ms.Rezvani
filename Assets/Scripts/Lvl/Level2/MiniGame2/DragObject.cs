using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragObject : MonoBehaviour,IDragHandler,IPointerDownHandler,IPointerUpHandler
{
    public Vector2 firstPos;
    Vector3 mainPos;
    public bool isDone;
    public void OnDrag(PointerEventData eventData)
    {
        if(!isDone)
        transform.position = eventData.position;

        //print(Vector2.Distance(GetComponent<RectTransform>().anchoredPosition, firstPos));
        
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        transform.SetAsLastSibling();
        mainPos = transform.position;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (Vector2.Distance(GetComponent<RectTransform>().anchoredPosition, firstPos) < 15)
        {
            GetComponent<RectTransform>().anchoredPosition = firstPos;
            isDone = true;
            FindObjectOfType<MiniGame2>().AddScore(this);
        }
        else
            transform.position = mainPos;
    }
   
}
