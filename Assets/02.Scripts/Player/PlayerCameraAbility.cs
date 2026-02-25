using Unity.Cinemachine;
using UnityEngine;

public class PlayerCameraAbility : PlayerAbility
{
    [SerializeField] private Transform _cameraRoot;

    private void Start()
    {
        if (!_owner.PhotonView.IsMine) return;

        Cursor.lockState = CursorLockMode.Locked;

        CinemachineCamera vcam = GameObject.Find("FollowCamera").GetComponent<CinemachineCamera>();
        vcam.Follow = _cameraRoot;
        
        CopyPosition minimapCam = GameObject.Find("MinimapCamera").GetComponent<CopyPosition>();
        minimapCam.SetTarget(transform);
    }
}