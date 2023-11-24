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
        "�����",
        "�����",
        "�迵��",
        "�迵ȣ",
        "����",
        "���Ѱ�",
        "������",
        "����",
        "�漮��",
        "����",
        "�̰ǿ�",
        "�̱Ա�",
        "���¹�",
        "������",
        "ȫ����",
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
