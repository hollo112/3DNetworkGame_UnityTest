using UnityEngine;

public class PlayerIconAbility : PlayerAbility
{
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private Sprite _mySprite;
    [SerializeField] private Sprite _otherSprite;

    private void Start()
    {
        _spriteRenderer.sprite = _owner.PhotonView.IsMine ? _mySprite : _otherSprite;
    }
}