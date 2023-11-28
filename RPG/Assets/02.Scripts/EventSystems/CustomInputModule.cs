using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace RPG.EventSystems
{
    public class CustomInputModule : StandaloneInputModule
    {
        public static CustomInputModule main;

        protected override void Awake()
        {
            base.Awake();

            main = this;
        }


        // Ư�������� ����� ���ؼ� casting �� GameObject �� ��ȯ�ϴ� �Լ�
        public bool TryGetHovered<T>(out List<GameObject> hovered, int mouseID = kMouseLeftId)
            where T : BaseRaycaster
        {
            if (m_PointerData.TryGetValue(mouseID, out PointerEventData data))
            {
                BaseRaycaster module = data.pointerCurrentRaycast.module; // ���� �����ʹ� � ���� ĳ������ �ߴ���
                // ���� ã�� ����� ������
                if (module != null &&
                    module is T)
                {
                    hovered = data.hovered; // ���� ���콺 ��ġ�� �ö���ִ� ��� GameObject ��ȯ
                    return true;
                }
            }

            hovered = null;
            return false;
        }

        public bool TryGetHovered<T, K>(out K hovered, int mouseID = kMouseLeftId)
            where T : BaseRaycaster
        {
            if (m_PointerData.TryGetValue(mouseID, out PointerEventData data))
            {
                BaseRaycaster module = data.pointerCurrentRaycast.module; // ���� �����ʹ� � ���� ĳ������ �ߴ���
                // ���� ã�� ����� ������
                if (module != null &&
                    module is T)
                {
                    foreach (var item in data.hovered)
                    {
                        if (item.TryGetComponent<K>(out hovered))
                        {
                            return true;
                        }
                    }
                    
                }
            }

            hovered = default;
            return false;
        }
    }
}