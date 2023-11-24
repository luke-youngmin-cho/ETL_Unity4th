using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;
using UnityEditor;

public class RandomSortedText : MonoBehaviour
{
    private List<string> _list = new List<string>()
    {
        "김경현",
        "현명수",
        "김영익",
        "김영호",
        "김용욱",
        "김한결",
        "김현수",
        "노현",
        "방석헌",
        "백상민",
        "이건우",
        "이규광",
        "이태민",
        "하의향",
        "홍준현",
    };
    private TMP_Text _orderedListText;

    private void Awake()
    {
        _orderedListText = GetComponent<TMP_Text>();
        Suffle();   
    }

    private void Suffle()
    {
        foreach (var item in _list.OrderBy(x => GUID.Generate()))
        {
            _orderedListText.text += item + "\n";
        }
    }

}
