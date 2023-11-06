using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TestDraggableItem : MonoBehaviour, IDragHandler, IEndDragHandler
{
    private Vector2 _origin;

    private void Awake()
    {
        _origin = transform.localPosition;
    }


    public void OnDrag(PointerEventData eventData)
    {
        Vector3 point = Camera.main.ScreenToWorldPoint(eventData.position);
        transform.position = new Vector3(point.x, point.y, transform.position.z);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        transform.localPosition = _origin;
    }
}
