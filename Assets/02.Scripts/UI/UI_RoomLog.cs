using Photon.Realtime;
using TMPro;
using UnityEngine;

public class UI_RoomLog : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _logText;

    private void Start()
    {
        _logText.text = "방에 입장했습니다";
        
        PhotonRoomManager.Instance.OnPlayerEntered += OnPlayerEntered;
        PhotonRoomManager.Instance.OnPlayerLeft += OnPlayerLeft;
    }
    private void OnPlayerEntered(Player newPlayer)
    {
        _logText.text += "\n" + $"{newPlayer.NickName}님이 입장하였습니다.";
    }

    private void OnPlayerLeft(Player otherPlayer)
    {
        _logText.text += "\n" + $"{otherPlayer.NickName}님이 퇴장하였습니다.";
    }
}
