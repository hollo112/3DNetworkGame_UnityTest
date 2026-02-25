using System;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class PhotonRoomManager : MonoBehaviourPunCallbacks
{
    public static PhotonRoomManager Instance{get; private set;}

    private Room _room;
    public Room Room => _room;
    public event Action OnDataChanged;
    public event Action<Player> OnPlayerEntered;
    public event Action<Player> OnPlayerLeft;
    
    public event Action<string, string> OnPlayerDead;

    private void Awake()
    {
        Instance = this;
    }
    
    // 방 입장에 성공하면 자동으로 호출되는 콜백 함수
    public override void OnJoinedRoom()
    {
        // Debug.Log("룸 입장 완료!");
        //
        // Debug.Log($"룸: {PhotonNetwork.CurrentRoom.Name}");
        // Debug.Log($"플레이어 인원: {PhotonNetwork.CurrentRoom.PlayerCount}");
        //
        // // 룸에 입장한 플레이어 정보
        // Dictionary<int, Player> roomPlayers = PhotonNetwork.CurrentRoom.Players;
        // foreach (KeyValuePair<int, Player> player in roomPlayers)
        // {
        //     Debug.Log($"{player.Value.NickName} : {player.Value.ActorNumber}");
        // }
        
        _room = PhotonNetwork.CurrentRoom;
        OnDataChanged?.Invoke();
        
        // 리소스 폴더에서 "Player" 이름을 가진 프리팹을 생성(인스턴스화)하고, 서버에 등록도 한다
        // -> 리소스 폴더 대신 다른 방법 찾아보기
        PhotonNetwork.Instantiate("Player", SpawnManager.Instance.GetRandomSpawnPosition(), Quaternion.identity);
    }

    // 새로운 플레이어가 방에 입장하면 자동으로 호출되는 함수
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        OnDataChanged?.Invoke();
        OnPlayerEntered?.Invoke(newPlayer);
    }

    // 플레이어가 방에서 퇴장하면 자동으로 호출되는 함수
    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        OnDataChanged?.Invoke();
        OnPlayerLeft?.Invoke(otherPlayer);
    }

    public void OnPlayerDeath(int attackerActorNumber)
    {
        string attackerNickname = _room.Players[attackerActorNumber].NickName;
        string victimNickname = PhotonNetwork.LocalPlayer.NickName;
        
        OnPlayerDead?.Invoke(attackerNickname, victimNickname);
    }
}
