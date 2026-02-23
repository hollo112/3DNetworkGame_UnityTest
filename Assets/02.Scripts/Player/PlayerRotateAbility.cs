using UnityEngine;

public class PlayerRotateAbility : MonoBehaviour
{
    public Transform CameraRoot;
    public float RotationSpeed = 100f;
    
    private float _mx;
    private float _my;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void LateUpdate()
    {
        _mx += Input.GetAxis("Mouse X") * RotationSpeed * Time.deltaTime;
        _my += Input.GetAxis("Mouse Y") * RotationSpeed * Time.deltaTime;
        
        _my = Mathf.Clamp(_my, -90f, 90f);
        
        CameraRoot.rotation = Quaternion.Euler(-_my, _mx, 0f);
    }
}
