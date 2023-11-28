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


        // 특정종류의 모듈을 통해서 casting 된 GameObject 를 반환하는 함수
        public bool TryGetHovered<T>(out List<GameObject> hovered, int mouseID = kMouseLeftId)
            where T : BaseRaycaster
        {
            if (m_PointerData.TryGetValue(mouseID, out PointerEventData data))
            {
                BaseRaycaster module = data.pointerCurrentRaycast.module; // 현재 데이터는 어떤 모듈로 캐스팅을 했는지
                // 내가 찾는 모듈이 맞으면
                if (module != null &&
                    module is T)
                {
                    hovered = data.hovered; // 현재 마우스 위치에 올라와있는 모든 GameObject 반환
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
                BaseRaycaster module = data.pointerCurrentRaycast.module; // 현재 데이터는 어떤 모듈로 캐스팅을 했는지
                // 내가 찾는 모듈이 맞으면
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