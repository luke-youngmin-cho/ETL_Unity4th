using UnityEngine;
using UnityEngine.UI;
using TMPro;
using RPG.UI;
using RPG.EventSystems;

public class ChatEnterUI : UIMonoBehaviour
{
    public string message => _message.text;
    [SerializeField] TMP_InputField _message;

    public override void Hide()
    {
        base.Hide();
        _message.text = string.Empty;
    }
}
