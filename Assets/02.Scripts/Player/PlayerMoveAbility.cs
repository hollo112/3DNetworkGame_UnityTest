using UnityEngine;

public class PlayerMoveAbility : PlayerAbility
{
    [SerializeField] private float _staminaDrainRate = 20f;
    [SerializeField] private float _jumpStaminaCost = 20f;

    private const float GRAVITY = 9.8f;
    private static readonly int MoveHash = Animator.StringToHash("Move");
    private float _yVeocity = 0f;

    private CharacterController _characterController;
    private Animator _animator;
    private PlayerStaminaAbility _stamina;
    private PlayerHealthAbility _health;

    private void Start()
    {
        _characterController = GetComponent<CharacterController>();
        _animator = GetComponent<Animator>();
        _stamina = _owner.GetAbility<PlayerStaminaAbility>();
        _health = _owner.GetAbility<PlayerHealthAbility>();
    }

    private void Update()
    {
        if (!_owner.PhotonView.IsMine) return;
        if (_health.IsDead) return;

        Vector3 direction = GetMoveDirection();
        bool isMoving = direction.sqrMagnitude > 0.01f;
        bool isSprinting = Input.GetKey(KeyCode.LeftShift) && isMoving && _stamina.HasStamina(0.1f);

        if (isSprinting)
            _stamina.UseStamina(_staminaDrainRate * Time.deltaTime);

        UpdateRotation(direction, isMoving);
        UpdateGravity();
        Move(direction, isSprinting);

        _animator.SetFloat(MoveHash, direction.magnitude);
    }

    private Vector3 GetMoveDirection()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        Vector3 direction = new Vector3(h, 0, v);
        direction = Camera.main.transform.TransformDirection(direction);
        direction.y = 0f;
        direction.Normalize();

        return direction;
    }

    private void UpdateRotation(Vector3 direction, bool isMoving)
    {
        if (!isMoving) return;

        Quaternion targetRotation = Quaternion.LookRotation(direction, Vector3.up);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, _owner.Stat.RotationSpeed * Time.deltaTime);
    }

    private void UpdateGravity()
    {
        _yVeocity -= GRAVITY * Time.deltaTime;

        if (Input.GetKey(KeyCode.Space) && _characterController.isGrounded && _stamina.HasStamina(_jumpStaminaCost))
        {
            _yVeocity = _owner.Stat.JumpPower;
            _stamina.UseStamina(_jumpStaminaCost);
        }
    }

    private void Move(Vector3 direction, bool isSprinting)
    {
        float speed = isSprinting ? _owner.Stat.RunSpeed : _owner.Stat.MoveSpeed;
        Vector3 velocity = direction * speed;
        velocity.y = _yVeocity;

        _characterController.Move(velocity * Time.deltaTime);
    }
}