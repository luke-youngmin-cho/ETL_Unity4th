using UnityEngine;
using UnityEngine.UI;
using CharacterController = Platformer.Controllers.CharacterController;

namespace Platformer.Test
{
    public class Test_DepletePlayerHpButton : MonoBehaviour
    {
        private Button _button;
        [SerializeField] private float _depleteAmount = 10.0f;
        [SerializeField] private CharacterController _controller;

        private void Awake()
        {
            _button = GetComponent<Button>();
            _button.onClick.AddListener(() => _controller.DepleteHp(this, _depleteAmount));
        }
    }
}