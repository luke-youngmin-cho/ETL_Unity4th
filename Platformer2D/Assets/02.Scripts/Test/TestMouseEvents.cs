using UnityEngine;
using UnityEngine.EventSystems;

namespace Platformer.Test
{
    public class TestMouseEvents : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler, IPointerClickHandler
    {
        private void OnMouseDown()
        {
            Debug.Log($"{name} mouse down");
        }

        private void OnMouseUp()
        {
            Debug.Log($"{name} mouse up");
        }

        private void OnMouseDrag()
        {
            Debug.Log($"{name} mouse drag");
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            Debug.Log($"{name} mouse begin drag {eventData.position}");
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            Debug.Log($"{name} mouse end drag {eventData.position}");
        }

        public void OnDrag(PointerEventData eventData)
        {
            Debug.Log($"{name} mouse drag {eventData.position}");
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            Debug.Log($"{name} mouse click {eventData.position}");
        }
    }
}