using System;
using UnityEngine;

public class CopyPosition : MonoBehaviour
{
    [SerializeField] private bool x, y, z;
    [SerializeField] private bool copyYRotation;
    private Transform _target;
    private Camera _mainCamera;

    private void Start()
    {
        _mainCamera = Camera.main;
    }

    private void Update()
    {
        if (!_target) return;

        transform.position = new Vector3(
            (x ? _target.position.x : transform.position.x),
            (y ? _target.position.y : transform.position.y),
            (z ? _target.position.z : transform.position.z));

        if (copyYRotation)
            transform.rotation = Quaternion.Euler(90f, _mainCamera.transform.eulerAngles.y, 0f);
    }

    public void SetTarget(Transform target)
    {
        _target = target;
    }
}
