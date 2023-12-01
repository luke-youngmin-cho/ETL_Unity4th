using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Events;

namespace RPG.UI
{
    public class NumberConfirmWindowUI : UIMonoBehaviour
    {
        public int numInput => int.Parse(_numInput.text);
        private int _max;
        [SerializeField] private TMP_Text _message;
        [SerializeField] private TMP_InputField _numInput;
        [SerializeField] private Button _confirm;
        [SerializeField] private Button _cancel;

        public void Show(string message, int max, UnityAction onConfirm, UnityAction onCancel = null)
        {
            base.Show();
            _message.text = message;
            _max = max;

            _confirm.onClick.RemoveAllListeners();
            _confirm.onClick.AddListener(Hide);
            if (onConfirm != null)
                _confirm.onClick.AddListener(onConfirm);

            _cancel.onClick.RemoveAllListeners();
            _cancel.onClick.AddListener(Hide);
            if (onCancel != null)
                _cancel.onClick.AddListener(onCancel);
        }
    }
}