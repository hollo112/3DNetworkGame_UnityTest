using System;
using System.Collections.Generic;
using System.IO;
using Photon.Pun;
using UnityEngine;

// 플레이어 대표로서 외부와의 소통 또는 어빌리티들을 관리하는 역할
public class PlayerController : MonoBehaviour, IPunObservable, IDamageable
{
    public PhotonView PhotonView;
    public PlayerStat Stat;
    [SerializeField] private GameObject _hitEffectPrefab;

    private void Awake()
    {
        PhotonView = GetComponent<PhotonView>();
    }

    [PunRPC]
    public void TakeDamage(float damage, int attackerActorNumber)
    {
        GetAbility<PlayerHealthAbility>().TakeDamage(damage, attackerActorNumber);
    }

    [PunRPC]
    public void RPC_SpawnHitEffect(Vector3 position)
    {
        if (_hitEffectPrefab == null) return;

        GameObject effect = Instantiate(_hitEffectPrefab, position, Quaternion.identity);
        Destroy(effect, 2f);
    }

    private float _lastHealth;
    private float _lastStamina;

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            bool healthChanged  = !Mathf.Approximately(Stat.Health,  _lastHealth);
            bool staminaChanged = !Mathf.Approximately(Stat.Stamina, _lastStamina);

            // 비트마스크로 변경 여부 표현 (항상 고정 3개 전송)
            byte mask = (byte)((healthChanged ? 1 : 0) | (staminaChanged ? 2 : 0));
            stream.SendNext(mask);
            stream.SendNext(Stat.Health);
            stream.SendNext(Stat.Stamina);

            if (healthChanged)  _lastHealth  = Stat.Health;
            if (staminaChanged) _lastStamina = Stat.Stamina;
        }
        else if (stream.IsReading)
        {
            byte mask     = (byte)stream.ReceiveNext();
            float health  = (float)stream.ReceiveNext();
            float stamina = (float)stream.ReceiveNext();

            if ((mask & 1) != 0) Stat.Health  = health;
            if ((mask & 2) != 0) Stat.Stamina = stamina;
        }
    }
    
    private Dictionary<Type, PlayerAbility> _abilitiesCache = new();
    
    public T GetAbility<T>() where T : PlayerAbility
    {
        var type = typeof(T);

        if (_abilitiesCache.TryGetValue(type, out PlayerAbility ability))
        {
            return ability as T;
        }

        // 게으른 초기화/로딩 -> 처음에 곧바로 초기화/로딩을 하는게 아니라
        //                    필요할때만 하는.. 뒤로 미루는 기법
        ability = GetComponent<T>();

        if (ability != null)
        {
            _abilitiesCache[ability.GetType()] = ability;

            return ability as T;
        }
        
        throw new Exception($"어빌리티 {type.Name}을 {gameObject.name}에서 찾을 수 없습니다.");
    }
}