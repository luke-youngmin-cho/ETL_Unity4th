using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CharacterController = Platformer.Controllers.CharacterController;

namespace Platformer.Test
{
    public class TestGUI : MonoBehaviour
    {
// if 전처리기문 : if 내용이 참일 경우 해당 내용 컴파일 함. 아니면 안함.
#if UNITY_EDITOR
        [SerializeField] private CharacterController _controller;

        private void Awake()
        {
            // Find 는 테스트할때말고 지양해야함.. 하이어라키 전체 탐색하므로 성능 안좋음
            if ((GameObject.Find("Player")?.TryGetComponent(out _controller) ?? false) == false)
            {
                Debug.LogWarning($"[TestGUI] : Failed to get Player component.");
            }
        }

        private void OnGUI()
        {
            GUI.Box(new Rect(10.0f, 10.0f, 180.0f, 140.0f), "Test");

            if (GUI.Button(new Rect(20.0f, 40.0f, 140.0f, 80.0f), "Hurt"))
            {
                _controller?.DepleteHp(null, 10.0f);
            }
        }
#endif
    }
}