using UnityEngine;

public enum AttackMode
{
    Sequential,
    Random
}

public class PlayerAttackAbility : MonoBehaviour
{
    [SerializeField] private AttackMode attackMode = AttackMode.Sequential;

    private static readonly string[] AttackNames = { "Attack1", "Attack2", "Attack3" };

    private PlayerAnimationAbility _animationAbility;
    private int _sequentialIndex;

    private void Awake()
    {
        _animationAbility = GetComponent<PlayerAnimationAbility>();
    }

    private void Update()
    {
        if (!Input.GetMouseButtonDown(0))
            return;

        if (_animationAbility.IsPlayingAttack())
            return;

        _animationAbility.PlayAttack(PickNextAttack());
    }

    private string PickNextAttack()
    {
        if (attackMode == AttackMode.Sequential)
        {
            string attack = AttackNames[_sequentialIndex];
            _sequentialIndex = (_sequentialIndex + 1) % AttackNames.Length;
            return attack;
        }

        return AttackNames[Random.Range(0, AttackNames.Length)];
    }
}
