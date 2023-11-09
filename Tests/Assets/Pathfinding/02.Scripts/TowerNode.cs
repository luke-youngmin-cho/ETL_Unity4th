using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerNode : MonoBehaviour
{
    public bool isAvailable
    {
        get => _isAvailable;
        set
        {
            _isAvailable = value;
            _sprieRenderer.color = value ? _available : _notAvailable;
        }
    }
    [SerializeField] private bool _isAvailable;
    [SerializeField] private SpriteRenderer _sprieRenderer;
    [SerializeField] private Color _available;
    [SerializeField] private Color _notAvailable;

    private void OnMouseEnter()
    {
        _sprieRenderer.color = isAvailable ? _available : _notAvailable;
    }

    private void OnMouseExit()
    {
        _sprieRenderer.color = _available;
    }
}
