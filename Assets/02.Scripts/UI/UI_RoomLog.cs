using Photon.Realtime;
using TMPro;
using UnityEngine;

public class UI_RoomLog : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _logText;

    private void Start()
    {
        _logText.text = "방에 입장했습니다";
        
        PhotonRoomManager.Instance.OnPlayerEntered += OnPlayerEnteredLog;
        PhotonRoomManager.Instance.OnPlayerLeft += OnPlayerLeftLog;
        PhotonRoomManager.Instance.OnPlayerDead += OnPlayerDeadLog;
    }
    private void OnPlayerEnteredLog(Player newPlayer)
    {
        _logText.text += "\n" + $"{newPlayer.NickName}님이 입장하였습니다.";
    }

    private void OnPlayerLeftLog(Player otherPlayer)
    {
        _logText.text += "\n" + $"{otherPlayer.NickName}님이 퇴장하였습니다.";
    }
    
    private void OnPlayerDeadLog(string attackerNickName, string victimNickname)
    {
        _logText.text += "\n" + $"{attackerNickName}님이 {victimNickname}님을 처치했습니다.";
    }
}
