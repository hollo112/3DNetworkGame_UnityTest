using System.Collections;
using DG.Tweening;
using Photon.Pun;
using UnityEngine;

public class PlayerHealthAbility : PlayerAbility
{
    [SerializeField] private float _respawnDelay = 3f;
    [SerializeField] private float _CameraShakeDuration = 0.3f;
    [SerializeField] private float _CameraShakeStrength = 0.5f;
    [SerializeField] private float _PlayerShakeDuration = 0.3f;
    [SerializeField] private float _PlayerShakeStrength = 0.2f;

    private Animator _animator;
    private CharacterController _characterController;

    private static readonly int DieTrigger = Animator.StringToHash("Die");
    private static readonly int RespawnTrigger = Animator.StringToHash("Respawn");

    public bool IsDead { get; private set; }

    private void Start()
    {
        _animator = GetComponentInChildren<Animator>();
        _characterController = GetComponent<CharacterController>();
        _owner.Stat.Health = _owner.Stat.MaxHealth;
    }

    public void TakeDamage(float damage)
    {
        if (IsDead) return;

        _owner.Stat.Health -= damage;

        _animator.transform.DOComplete();
        _animator.transform.DOShakePosition(_PlayerShakeDuration, _PlayerShakeStrength, vibrato: 30);

        if (_owner.PhotonView.IsMine)
        {
            Camera.main.transform.DOShakePosition(_CameraShakeDuration, _CameraShakeStrength);
        }

        if (_owner.Stat.Health <= 0f)
        {
            _owner.Stat.Health = 0f;
            Die();
        }
    }

    private void Die()
    {
        if (_owner.PhotonView.IsMine)
        {
            PlayDieAnimation();
            
            _owner.PhotonView.RPC(nameof(PlayDieAnimation), RpcTarget.Others);
            StartCoroutine(RespawnCoroutine());
        }
    }

    private IEnumerator RespawnCoroutine()
    {
        yield return new WaitForSeconds(_respawnDelay);

        Vector3 spawnPos = SpawnManager.Instance.GetRandomSpawnPosition();

        _characterController.enabled = false;
        transform.position = spawnPos;
        _characterController.enabled = true;

        _owner.Stat.Health = _owner.Stat.MaxHealth;
        _owner.Stat.Stamina = _owner.Stat.MaxStamina;
        IsDead = false;

        _animator.SetTrigger(RespawnTrigger);
        _owner.PhotonView.RPC(nameof(PlayRespawnAnimation), RpcTarget.Others);
    }

    [PunRPC]
    private void PlayRespawnAnimation()
    {
        IsDead = false;
        _animator.SetTrigger(RespawnTrigger);
    }
    
    [PunRPC]
    private void PlayDieAnimation()
    {
        IsDead = true;
        _animator.SetTrigger(DieTrigger);
    }
}