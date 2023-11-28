using System;

namespace RPG.UI
{
    public interface IUI
    {
        int sortingOrder { get; set; }
        bool inputActionEnabled { get; set; }

        void InputAction();
        void Show();
        void Hide();

        event Action onShow;
        event Action onHide;
    }
}