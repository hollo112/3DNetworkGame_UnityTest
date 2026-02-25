using Photon.Pun;
using UnityEngine;

public class PlayerWeaponHitAbility : PlayerAbility
{
    private void OnTriggerEnter(Collider other)
    {
        if (!_owner.PhotonView.IsMine) return;
        if (other.transform == _owner.transform) return;

        if (other.TryGetComponent<IDamageable>(out var damageable))
        {
            Debug.Log(damageable);
            
            //damageable.TakeDamage(_owner.Stat.Damage);
            
            // 상대방의 TakeDamage를 RPC로 호출한다
            PlayerController otherPlayer = other.GetComponent<PlayerController>();
            otherPlayer.PhotonView.RPC(nameof(damageable.TakeDamage), RpcTarget.All, _owner.Stat.Damage);
        }
    }
}
