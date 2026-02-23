using UnityEngine;

public class PlayerMoveAbility : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 7f;
    [SerializeField] private float rotationSpeed = 10f;
    [SerializeField] private float jumpHeight = 2.5f;
    [SerializeField] private float gravity = 9.8f;
    [SerializeField] private KeyCode jumpKey = KeyCode.Space;
    [SerializeField] private Transform cameraTransform;

    private float _yVelocity = 0f;

    private CharacterController _characterController;

    public float NormalizedSpeed { get; private set; }

    private void Awake()
    {
        _characterController = GetComponent<CharacterController>();
        if (cameraTransform == null && Camera.main != null)
        {
            cameraTransform = Camera.main.transform;
        }
    }

    private void Update()
    {
        Vector3 moveDirection = ReadMoveInput();
        bool isGrounded = _characterController.isGrounded;

        NormalizedSpeed = Mathf.Clamp01(moveDirection.magnitude);

        RotateTowardMoveDirection(moveDirection);

        HandleGroundState(isGrounded);
        HandleJumpInput(isGrounded);
        ApplyGravity();

        Vector3 velocity = BuildVelocity(moveDirection);
        _characterController.Move(velocity * Time.deltaTime);
    }

    private Vector3 ReadMoveInput()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        return BuildCameraRelativeDirection(horizontal, vertical);
    }

    private Vector3 BuildCameraRelativeDirection(float horizontal, float vertical)
    {
        Transform cam = GetReferenceCamera();
        if (cam == null)
        {
            return new Vector3(horizontal, 0f, vertical).normalized;
        }

        Vector3 forward = cam.forward;
        Vector3 right = cam.right;
        forward.y = 0f;
        right.y = 0f;

        forward.Normalize();
        right.Normalize();

        Vector3 moveDirection = (forward * vertical) + (right * horizontal);
        return moveDirection.sqrMagnitude > 1f ? moveDirection.normalized : moveDirection;
    }

    private Transform GetReferenceCamera()
    {
        if (cameraTransform != null)
        {
            return cameraTransform;
        }

        Camera mainCam = Camera.main;
        return mainCam != null ? mainCam.transform : null;
    }

    private void HandleGroundState(bool isGrounded)
    {
        if (isGrounded && _yVelocity < 0f)
        {
            _yVelocity = -1f;
        }
    }

    private void HandleJumpInput(bool isGrounded)
    {
        if (!isGrounded || !Input.GetKeyDown(jumpKey))
        {
            return;
        }

        _yVelocity = Mathf.Sqrt(jumpHeight * 2f * gravity);
    }

    private void ApplyGravity()
    {
        _yVelocity -= gravity * Time.deltaTime;
    }

    private void RotateTowardMoveDirection(Vector3 moveDirection)
    {
        if (moveDirection.sqrMagnitude < 0.01f)
            return;

        Quaternion targetRotation = Quaternion.LookRotation(moveDirection, Vector3.up);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }

    private Vector3 BuildVelocity(Vector3 moveDirection)
    {
        Vector3 velocity = moveDirection * moveSpeed;
        velocity.y = _yVelocity;
        return velocity;
    }
}
