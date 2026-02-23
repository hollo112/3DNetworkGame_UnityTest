using UnityEngine;

public class PlayerAnimationAbility : MonoBehaviour
{
    private static readonly int SpeedHash = Animator.StringToHash("Speed");
    private static readonly string[] AttackStateNames = { "Attack1", "Attack2", "Attack3" };

    [SerializeField] private float dampTime = 0.1f;
    [SerializeField] private float crossFadeDuration = 0.1f;

    private Animator _animator;
    private PlayerMoveAbility _moveAbility;

    private void Awake()
    {
        _animator = GetComponentInChildren<Animator>();
        _moveAbility = GetComponent<PlayerMoveAbility>();
    }

    private void Update()
    {
        _animator.SetFloat(SpeedHash, _moveAbility.NormalizedSpeed, dampTime, Time.deltaTime);
    }

    public bool IsPlayingAttack()
    {
        AnimatorStateInfo stateInfo = _animator.GetCurrentAnimatorStateInfo(0);
        for (int i = 0; i < AttackStateNames.Length; i++)
        {
            if (stateInfo.IsName(AttackStateNames[i]) && stateInfo.normalizedTime < 1f)
                return true;
        }
        return false;
    }

    public void PlayAttack(string attackName)
    {
        _animator.CrossFadeInFixedTime(attackName, crossFadeDuration);
    }
}