using UnityEngine;

public class PlayerMoveAbility : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 7f;
    [SerializeField] private float jumpHeight = 2.5f;
    [SerializeField] private float gravity = 9.8f;
    [SerializeField] private KeyCode jumpKey = KeyCode.Space;

    private float _yVelocity = 0f;
    
    private CharacterController _characterController;

    private void Awake()
    {
        _characterController = GetComponent<CharacterController>();
    }

    private void Update()
    {
        Vector3 moveDirection = ReadMoveInput();
        bool isGrounded = _characterController.isGrounded;

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
        return new Vector3(horizontal, 0f, vertical);
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

    private Vector3 BuildVelocity(Vector3 moveDirection)
    {
        Vector3 velocity = moveDirection * moveSpeed;
        velocity.y = _yVelocity;
        return velocity;
    }
}
