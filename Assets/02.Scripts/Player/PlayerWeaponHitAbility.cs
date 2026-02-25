using Photon.Pun;
using UnityEngine;

public class PlayerWeaponHitAbility : PlayerAbility
{
    [SerializeField] private GameObject _hitEffectPrefab;

    private void OnTriggerEnter(Collider other)
    {
        if (!_owner.PhotonView.IsMine) return;
        if (other.transform == _owner.transform) return;

        if (other.TryGetComponent<IDamageable>(out var damageable))
        {
            Vector3 hitPoint = other.ClosestPoint(transform.position);
            SpawnHitEffect(hitPoint);
            _owner.PhotonView.RPC(nameof(PlayerController.RPC_SpawnHitEffect), RpcTarget.Others, hitPoint);

            PlayerController otherPlayer = other.GetComponent<PlayerController>();
            otherPlayer.PhotonView.RPC(nameof(damageable.TakeDamage), RpcTarget.All, _owner.Stat.Damage);
        }
    }

    public void SpawnHitEffect(Vector3 position)
    {
        if (_hitEffectPrefab == null) return;

        GameObject effect = Instantiate(_hitEffectPrefab, position, Quaternion.identity);
        Destroy(effect, 2f);
    }
}
