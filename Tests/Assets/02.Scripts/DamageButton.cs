using UnityEngine;
using UnityEngine.UI;

public class DamageButton : MonoBehaviour
{
    [SerializeField] private Player _player;
    private Button _button;

    private void Awake()
    {
        _button = GetComponent<Button>();
        _button.onClick.AddListener(DepletePlayerHP5);
        // 람다식 (익명함수) 
        // 컴파일러가 판단 할 수 있는 부분들을 생략하고 이름을 사용하지 않는 형태의 함수식
        _button.onClick.AddListener(() => _player.hp -= 5.0f);
    }

    private void DepletePlayerHP5()
    {
        _player.hp -= 5.0f;
    }
}
