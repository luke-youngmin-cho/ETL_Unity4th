using UnityEngine;
using UnityEngine.UI;
using TMPro;
using RPG.Controllers;
using RPG.UI;

public class PlayerStatusSlot : MonoBehaviour
{
    public ChatBalloon chatBallon;
    [SerializeField] Image _icon;
    [SerializeField] TMP_Text _nickName;
    [SerializeField] Slider _hp;

    public void SetUp(ulong clientID)
    {
        _nickName.text = $"Player{clientID}";
        var controller = PlayerController.GetSpawned(clientID);
        _hp.minValue = 0.0f;
        _hp.maxValue = controller.hpMax;
        _hp.value = controller.hp;
        controller.onHpChanged += value => _hp.value = value;
    }
}