using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventoryController : MonoBehaviour
{
    [SerializeField] Image _preview;
    [SerializeField] GraphicRaycaster module;
    [SerializeField] EventSystem eventSystem;
    InventorySlot _selected;

    private void Update()
    {
        InputAction();
    }

    public void InputAction()
    {
        if (Input.GetMouseButtonDown(0))
        {
            PointerEventData pointerEventData = new PointerEventData(eventSystem);
            pointerEventData.position = Input.mousePosition;
            List<RaycastResult> results = new List<RaycastResult>();
            module.Raycast(pointerEventData, results);

            foreach (RaycastResult result in results)
            {
                if (result.gameObject.TryGetComponent(out InventorySlot slot))
                {
                    // 슬롯 선택한적 없으면 이 슬롯 선택
                    if (_selected == null)
                    {
                        Select(slot);
                    }
                    // 선택된 슬롯 있으면 스왑
                    else if (_selected != slot)
                    {
                        int tmpID = _selected.id;
                        _selected.id = slot.id;
                        slot.id = tmpID;
                        Deselect();
                    }

                    break;
                }
            }
        }
        //else if (Input.GetMouseButtonUp(0))
        //{
        //    Deselect(); 
        //}

        PreviewFollowsPointer();
    }

    private void Select(InventorySlot slot)
    {
        _selected = slot;
        _preview.sprite = ItemSprites.instance[slot.id];
        _preview.gameObject.SetActive(true);
    }

    private void Deselect()
    {
        _selected = null;
        _preview.sprite = null;
        _preview.gameObject.SetActive(false);
    }

    private void PreviewFollowsPointer()
    {
        if (_selected == null)
            return;

        _preview.transform.position = Input.mousePosition;
    }
}
