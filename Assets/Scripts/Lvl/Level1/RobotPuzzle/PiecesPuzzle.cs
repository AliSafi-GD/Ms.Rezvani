using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public class PiecesPuzzle : MonoBehaviour, IDragHandler,IPointerDownHandler,IPointerUpHandler
{
    Vector2 mainPosition;
    public void OnDrag(PointerEventData eventData)
    {
        transform.position = eventData.position;
        //print(Vector3.Distance(transform.position, transform.parent.position));
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        mainPosition = transform.position;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (Vector3.Distance(transform.position, transform.parent.position) < 10f)
        {
            transform.localPosition = Vector3.zero;
            FindObjectOfType<PuzzleManager>().NumberTrue++;
            GetComponent<Image>().raycastTarget = false;
            DB.instance.SetCoinAndScore(25, 75);
        }
        else
        {
            transform.position = mainPosition;
            DB.instance.SetCoinAndScore(-25,-15);
        }
            
    }
}
