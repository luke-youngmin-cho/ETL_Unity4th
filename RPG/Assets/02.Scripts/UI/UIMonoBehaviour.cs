using RPG.EventSystems;
using RPG.GameSystems;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.UI
{
    public abstract class UIMonoBehaviour : MonoBehaviour, IUI, IInitializable
    {
        public int sortingOrder 
        { 
            get => canvas.sortingOrder;
            set => canvas.sortingOrder = value;
        }
        public bool inputActionEnabled { get; set; }

        public event Action onShow;
        public event Action onHide;

        protected Canvas canvas;


        public virtual void InputAction()
        {
            if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1))
            {
                if (CustomInputModule.main.TryGetHovered<GraphicRaycaster, CanvasRenderer>
                    (out CanvasRenderer hovered))
                {
                    // ������ �������� �ֻ����� UI ������ �����鼭 
                    // �ش� ������ UI �� ���� InputAction �������� UI �� �ٸ��ٸ� -> �ٸ�UI ���õȰ���
                    if (hovered.transform.root.TryGetComponent(out UIMonoBehaviour ui) &&
                        ui != this)
                    {
                        UIManager.instance.Push(ui);
                        ui.InputAction();
                    }
                }
            }
        }

        public virtual void Show()
        {
            UIManager.instance.Push(this);
            gameObject.SetActive(true);
            onShow?.Invoke();
        }

        public virtual void Hide()
        {
            UIManager.instance.Pop(this);
            gameObject.SetActive(false);
            onHide?.Invoke();
        }

        public virtual void Init()
        {
            canvas = GetComponent<Canvas>();
            UIManager.instance.Register(this);
        }

        private void Update()
        {
            if (inputActionEnabled)
                InputAction();
        }
    }
}