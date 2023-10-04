using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float hpMax
    {
        get
        {
            return _hpMax;
        }
    }

    public float hp
    {
        get
        {
            return _hp;
        }
        set
        {
            if (_hp == value)
                return;

            _hp = value;
            onHpChanged?.Invoke(value);
            Debug.Log($"HP has changed to {value}");
        }
    }
    private float _hp;
    public event Action<float> onHpChanged;
    [SerializeField] private float _hpMax = 100.0f;

    private void Awake()
    {
        _hp = _hpMax;
    }
}
