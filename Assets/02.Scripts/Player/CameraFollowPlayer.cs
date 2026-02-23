using Photon.Pun;
using Unity.Cinemachine;
using UnityEngine;

public class CameraFollowPlayer : MonoBehaviour
{
    [SerializeField] private Transform _cameraRoot;
    private PhotonView _photonView;
    private void Start()
    {
        _photonView = GetComponent<PhotonView>();
        if (!_photonView.IsMine) return;
        
        Cursor.lockState = CursorLockMode.Locked;
        
        CinemachineCamera vcam = GameObject.Find("FollowCamera").GetComponent<CinemachineCamera>();
        vcam.Follow = _cameraRoot.transform;
    }
}
