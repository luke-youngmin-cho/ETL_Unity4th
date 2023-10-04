using System;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    [SerializeField] private Slider _hpBar;
    [SerializeField] private Player _player;

    private Predicate<float> _match;

    private void Start()
    {
        _hpBar.minValue = 0.0f;
        _hpBar.maxValue = _player.hpMax;
        _hpBar.value = _player.hp;

        //_player.onHpChanged += Refresh;
        _player.onHpChanged += (value) => _hpBar.value = value;

        _match += (value) => value > 3;
    }

    //private void Refresh(float value)
    //{
    //    _hpBar.value = value;
    //}

    private bool IsBiggerThan3(float value)
    {
        return value > 3;
    }
}