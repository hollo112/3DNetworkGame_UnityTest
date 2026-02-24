using UnityEngine;
using UnityEngine.UI;

public class PlayerStatusAbility : PlayerAbility
{
    [SerializeField] private Image _healthBar;
    [SerializeField] private Image _staminaBar;

    private float _maxHealth;
    private PlayerStaminaAbility _stamina;

    protected override void Awake()
    {
        base.Awake();
    }

    private void Start()
    {
        _maxHealth = _owner.Stat.Health;
        _stamina = _owner.GetAbility<PlayerStaminaAbility>();
    }

    private void Update()
    {
        if (_maxHealth > 0f)
            _healthBar.fillAmount = _owner.Stat.Health / _maxHealth;

        if (_stamina.Max > 0f)
            _staminaBar.fillAmount = _stamina.Current / _stamina.Max;
    }
}