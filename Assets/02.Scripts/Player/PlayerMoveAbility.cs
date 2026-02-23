using UnityEngine;

public class PlayerMoveAbility : PlayerAbility
{
    
    private const float GRAVITY = 9.8f;
    private float _yVeocity = 0f;

    private CharacterController _characterController;
    private Animator _animator;

    private void Start()
    {
        _characterController = GetComponent<CharacterController>();
        _animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (!_owner.PhotonView.IsMine) return;

        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        
        Vector3 direction = new Vector3(h, 0, v);
        direction.Normalize();
        
        direction = Camera.main.transform.TransformDirection(direction);
        direction.y = 0f;
        direction.Normalize();

        _animator.SetFloat("Move", direction.magnitude);

        if (direction.sqrMagnitude > 0.01f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction, Vector3.up);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, _owner.Stat.RotationSpeed * Time.deltaTime);
        }

        _yVeocity -= GRAVITY * Time.deltaTime;

        if (Input.GetKey(KeyCode.Space) && _characterController.isGrounded)
        {
            _yVeocity = _owner.Stat.JumpPower;
        }

        Vector3 velocity = direction * _owner.Stat.MoveSpeed;
        velocity.y = _yVeocity;

        _characterController.Move(velocity * Time.deltaTime);
    }
}