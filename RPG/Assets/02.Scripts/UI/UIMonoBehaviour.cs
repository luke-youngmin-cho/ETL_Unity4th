using RPG.EventSystems;
using RPG.GameSystems;
using System;
using UnityEngine;
using UnityEngine.EventSystems;
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
        [SerializeField] private bool _hideWhenPointerDownOutside;

        public virtual void InputAction()
        {
            if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1))
            {
                if (CustomInputModule.main.TryGetHovered<GraphicRaycaster, CanvasRenderer>
                    (out CanvasRenderer hovered))
                {
                    // 감지된 렌더러의 최상위에 UI 단위가 있으면서 
                    // 해당 감지된 UI 가 현재 InputAction 실행중인 UI 와 다르다면 -> 다른UI 선택된거임
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

            if (_hideWhenPointerDownOutside)
                CreateOutsidePanel();
        }

        private void Update()
        {
            if (inputActionEnabled)
                InputAction();
        }

        private void CreateOutsidePanel()
        {
            GameObject outsidePanel = new GameObject("Outside");
            outsidePanel.transform.SetParent(transform);
            outsidePanel.transform.SetAsFirstSibling();
            Image image = outsidePanel.AddComponent<Image>();
            image.color = new Color(0f, 0f, 0f, 0.5f);

            RectTransform rectTransform = outsidePanel.GetComponent<RectTransform>();
            rectTransform.anchorMin = Vector2.zero;
            rectTransform.anchorMax = Vector2.one;
            rectTransform.pivot = new Vector2(0.5f, 0.5f);
            rectTransform.localScale = Vector3.one;

            EventTrigger trigger = outsidePanel.AddComponent<EventTrigger>();
            EventTrigger.Entry entry = new EventTrigger.Entry();
            entry.eventID = EventTriggerType.PointerDown;
            entry.callback.AddListener(eventData => Hide());
            trigger.triggers.Add(entry);
        }
    }
}