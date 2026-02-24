using UnityEngine;

public class PlayerAttackAbility : PlayerAbility
{
    [SerializeField] private EAnimationSequenceType _animationSequenceType;
    [SerializeField] private float _staminaCost = 15f;

    private Animator _animator;
    private PlayerStaminaAbility _stamina;

    private int _prevAnimationNumber = 0;
    private float _attackTimer = 0f;

    private void Start()
    {
        _animator = GetComponent<Animator>();
        _stamina = _owner.GetAbility<PlayerStaminaAbility>();
    }

    private void Update()
    {
        if (!_owner.PhotonView.IsMine) return;

        _attackTimer += Time.deltaTime;

        if (Input.GetMouseButton(0) && _attackTimer >= _owner.Stat.AttackSpeed && _stamina.HasStamina(_staminaCost))
        {
            _attackTimer = 0f;
            _stamina.UseStamina(_staminaCost);

            int animationNumber = 0;
            switch (_animationSequenceType)
            {
                case EAnimationSequenceType.Sequence:
                {
                    animationNumber = 1 + (_prevAnimationNumber++) % 3;
                    break;
                }

                case EAnimationSequenceType.Random:
                {
                    animationNumber = Random.Range(1, 4);
                    break;
                }
            }

            _animator.SetTrigger($"Attack{animationNumber}");
        }
    }
}

public enum EAnimationSequenceType
{
    Sequence,
    Random,
}