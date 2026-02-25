using UnityEngine;

public class PlayerTauntAbility : PlayerAbility
{
    [SerializeField] private GameObject _sword;
    [SerializeField] private string _baseStateName = "MoveBlendTree";

    private Animator _animator;

    private static readonly int Taunt1Hash = Animator.StringToHash("Taunt1");
    private static readonly int Taunt2Hash = Animator.StringToHash("Taunt2");
    private static readonly int Taunt3Hash = Animator.StringToHash("Taunt3");

    private void Start()
    {
        _animator = GetComponent<Animator>();
    }

    private void Update()
    {
        bool isTaunting = _animator.GetCurrentAnimatorStateInfo(0).IsTag("Taunt");
        _sword.SetActive(!isTaunting);

        if (!_owner.PhotonView.IsMine) return;

        if (isTaunting && IsCancelInput())
        {
            _animator.CrossFadeInFixedTime(_baseStateName, 0.1f);
            return;
        }

        if (Input.GetKeyDown(KeyCode.Alpha1)) _animator.SetTrigger(Taunt1Hash);
        if (Input.GetKeyDown(KeyCode.Alpha2)) _animator.SetTrigger(Taunt2Hash);
        if (Input.GetKeyDown(KeyCode.Alpha3)) _animator.SetTrigger(Taunt3Hash);
    }

    private float _prevH;
    private float _prevV;

    private bool IsCancelInput()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        bool newMovement = (h != 0f && _prevH == 0f) || (v != 0f && _prevV == 0f);


        return newMovement
            || Input.GetKeyDown(KeyCode.Space)
            || Input.GetMouseButtonDown(0);
    }
}