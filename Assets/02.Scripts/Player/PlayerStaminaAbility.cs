using UnityEngine;

public class PlayerStaminaAbility : PlayerAbility
{
    [SerializeField] private float _regenRate = 10f;
    [SerializeField] [Range(0f, 1f)] private float _exhaustionRecoveryRatio = 0.15f;

    private float _maxStamina;
    private bool _isExhausted;

    public float Current => _owner.Stat.Stamina;
    public float Max => _maxStamina;
    public bool IsExhausted => _isExhausted;

    private void Start()
    {
        _maxStamina = _owner.Stat.Stamina;
    }

    private void Update()
    {
        Regenerate();
    }

    public bool HasStamina(float amount)
    {
        if (_isExhausted) return false;

        return _owner.Stat.Stamina >= amount;
    }

    public void UseStamina(float amount)
    {
        _owner.Stat.Stamina = Mathf.Max(_owner.Stat.Stamina - amount, 0f);

        if (_owner.Stat.Stamina <= 0f)
            _isExhausted = true;
    }

    private void Regenerate()
    {
        if (_owner.Stat.Stamina >= _maxStamina) return;

        _owner.Stat.Stamina = Mathf.Min(_owner.Stat.Stamina + _regenRate * Time.deltaTime, _maxStamina);

        if (_isExhausted && _owner.Stat.Stamina >= _maxStamina * _exhaustionRecoveryRatio)
            _isExhausted = false;
    }
}