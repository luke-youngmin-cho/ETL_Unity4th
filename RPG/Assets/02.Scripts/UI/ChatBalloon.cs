using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace RPG.UI
{
    public class ChatBalloon : MonoBehaviour
    {
        [SerializeField] TMP_Text _message;
        [SerializeField] float _duration = 3.0f;
        private float _timeMark;
        private bool _isCorouting;
        private Coroutine _coroutine;

        public void Show(string message)
        {
            if (_isCorouting)
                StopCoroutine(_coroutine);

            gameObject.SetActive(true);
            _timeMark = Time.time;
            _isCorouting = true;
            _coroutine = StartCoroutine(C_Show(message));
        }

        public void Hide()
        {
            if (_isCorouting)
                StopCoroutine(_coroutine);

            gameObject.SetActive(false);
            _isCorouting = false;
            _coroutine = null;
        }

        IEnumerator C_Show(string message)
        {
            _message.text = message;
            while (Time.time - _timeMark < _duration)
            {
                yield return null;
            }
            Hide();
        }
    }
}