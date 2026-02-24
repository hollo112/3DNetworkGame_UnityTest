using TMPro;
using UnityEngine;

public class PlayerNicknameAbility : PlayerAbility
{
    [SerializeField] private TextMeshProUGUI _nicknameTextUI;

    private void Start()
    {
        if (_owner.PhotonView.Owner != null)
        {
            _nicknameTextUI.text = _owner.PhotonView.Owner.NickName;
        }

        _nicknameTextUI.color = _owner.PhotonView.IsMine ? Color.green : Color.red;
    }

    private void Update()
    {
        transform.forward = Camera.main.transform.forward;
    }
}
