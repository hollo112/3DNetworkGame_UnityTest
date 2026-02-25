using UnityEngine;

public class LockWorldRotation : MonoBehaviour
{
    private void LateUpdate()
    {
        transform.rotation = Quaternion.Euler(90f, Camera.main.transform.eulerAngles.y, 0f);
    }
}