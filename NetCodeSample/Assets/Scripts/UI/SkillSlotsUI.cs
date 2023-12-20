using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillSlotsUI : MonoBehaviour
{
    [SerializeField] SkillSlot _slot;
    [SerializeField] SkillData _dataForTesting;
    [SerializeField] Button _useSkillForTesting;


    private void Awake()
    {
        _slot.data = _dataForTesting;
        _useSkillForTesting.onClick.AddListener(() =>
        {
            StartCoroutine(C_CoolDownSkillForTesting(_dataForTesting.coolDownTime));
        });
    }

    IEnumerator C_CoolDownSkillForTesting(float coolDownTime)
    {
        float timeMark = Time.time;
        while (true)
        {
            float elapsedTime = Time.time - timeMark;
            _slot.OnSkillCoolDownChanged(elapsedTime);

            if (elapsedTime >= coolDownTime)
                break;

            yield return null;
        }
    }
}
