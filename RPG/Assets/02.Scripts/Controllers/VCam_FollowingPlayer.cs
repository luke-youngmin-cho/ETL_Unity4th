using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class VCam_FollowingPlayer : MonoBehaviour
{
    private CinemachineVirtualCamera _vCam;
    private Transform _target;
    private Transform _targetRoot;

    [SerializeField] private float _rotateSpeedY;
    [SerializeField] private float _rotateSpeedX;
    [SerializeField] private float _angleXMin = -8.0f;
    [SerializeField] private float _angleXMax = 45.0f;
    [SerializeField] private float _fovMin = 20.0f;
    [SerializeField] private float _fovMax = 90.0f;
    [SerializeField] private float _scrollThreshold;
    [SerializeField] private float _scrollSpeed;

    private void Awake()
    {
        _vCam = GetComponent<CinemachineVirtualCamera>();
        _target = _vCam.Follow;
        _targetRoot = _target.root;
    }

    private void LateUpdate()
    {
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        _targetRoot.Rotate(Vector3.up, mouseX * _rotateSpeedY * Time.deltaTime, Space.World);
        _target.Rotate(Vector3.left, mouseY * _rotateSpeedX * Time.deltaTime, Space.Self);
        _target.localRotation = Quaternion.Euler(ClampAngle(_target.eulerAngles.x, _angleXMin, _angleXMax), 0.0f, 0.0f);

        if (Mathf.Abs(Input.mouseScrollDelta.y) >= _scrollThreshold)
        {
            _vCam.m_Lens.FieldOfView -= Input.mouseScrollDelta.y * _scrollSpeed * Time.deltaTime;
            _vCam.m_Lens.FieldOfView = Mathf.Clamp(_vCam.m_Lens.FieldOfView, _fovMin, _fovMax);
        }
    }

    private float ClampAngle(float angle, float min, float max)
    {
        angle = (angle + 360.0f) % 360.0f;

        if (angle > 180.0f)
        {
            angle -= 360.0f;
        }

        return Mathf.Clamp(angle, min, max);
    }
}
