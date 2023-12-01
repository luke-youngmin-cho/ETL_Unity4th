using RPG.Singleton;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.UI
{
    public class UIManager : SingletonBase<UIManager>
    {
        public Dictionary<Type, IUI> uis = new Dictionary<Type, IUI>(); // 등록된 모든 UI
        public LinkedList<IUI> showns = new LinkedList<IUI>(); // 현재 보여지고있는 팝업 UI 들

        public T Get<T>()
            where T : IUI
        {
            if (uis.TryGetValue(typeof(T), out IUI ui))
                return (T)ui;
            else
                throw new Exception($"[UIManager] : {typeof(T)} has not been registered but you tried to get it ..");
        }

        public void Register(IUI ui)
        {
            Type type = ui.GetType();
            if (uis.TryAdd(type, ui) == false)
                throw new Exception($"[UIManager] : {type} already registered but you tried to add it again..!");
            Debug.Log($"[UIManager] : Registered {type}.");
        }

        public void Push(IUI ui)
        {
            // 이미 젤 뒤에있으면 안함
            if (showns.Count > 0 &&
                showns.Last.Value == ui)
                return;

            // 가장뒤에있던 UI 보다 뒤로 보내기 
            int sortingOrder = 0;
            if (showns.Last?.Value != null)
            {
                sortingOrder = showns.Last.Value.sortingOrder + 1;
                showns.Last.Value.inputActionEnabled = false;
            }
            ui.sortingOrder = sortingOrder;
            ui.inputActionEnabled = true;
            showns.Remove(ui);
            showns.AddLast(ui);

            if (showns.Count == 1)
            {
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.Confined;
            }

            Debug.Log($"{ui} is top.");
        }

        public void Pop(IUI ui)
        {
            // 빼려는게 하필 마지막꺼면서 마지막꺼 앞에 하나이상 있으면 InputAction 활성화 바톤터치
            if (showns.Count > 1 &&
                showns.Last.Value == ui)
                showns.Last.Previous.Value.inputActionEnabled = true;

            showns.Remove(ui); // ui 뺌

            if (showns.Count == 0)
            {
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
            }

            Debug.Log($"{showns.Last.Value} is top.");
        }

        public void HideLast()
        {
            if (showns.Count <= 0)
                return;

            showns.Last.Value.Hide();
        }
    }
}