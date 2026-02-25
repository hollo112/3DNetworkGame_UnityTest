using UnityEngine;

public class PlayerRotateAbility : PlayerAbility
{
    [SerializeField] private Transform _cameraRoot;

    private float _mx;
    private float _my;

    private void Update()
    {
        if (!_owner.PhotonView.IsMine) return;

        _mx += Input.GetAxis("Mouse X") * _owner.Stat.RotationSpeed * Time.deltaTime;
        _my += Input.GetAxis("Mouse Y") * _owner.Stat.RotationSpeed * Time.deltaTime;

        _my = Mathf.Clamp(_my, -90f, 90f);

        _cameraRoot.rotation = Quaternion.Euler(-_my, _mx, 0f);
    }
}