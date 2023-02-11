using UnityEngine;

public class Character : MonoBehaviour
{
    private CharacterController _cc;
    public float MoveSpeed = 5f;
    private Vector3 _movementVelocity;
    private PlayerInput _playerInput;
    private float _verticalVelocity;
    public float Gravity = -9.8f;
    private Animator _animator;

    public int Coin;

    //Enemy
    public bool IsPlayer = true;
    private UnityEngine.AI.NavMeshAgent _navMeshAgent;
    private Transform TargetPlayer;


    //Player slides
    private float attackStartTime;
    public float AttackSlideDuration = 0.4f;
    public float AttackSlideSpeed = 0.06f;

    private Vector3 impactOnCharacter;

    public bool IsInvincible;
    public float invincibleDuration = 2f;

    private float attackAnimationDuration;
    public float SlideSpeed = 9f;

    //State Machine
    public enum CharacterState
    {
        Normal, Attacking, Dead, BeingHit, Slide, Spawn
    }
    public CharacterState CurrentState;

    public float SpawnDuration = 2f;
    private float currentSpawnTime;

    //Material animation
    private MaterialPropertyBlock _materialPropertyBlock;
    private SkinnedMeshRenderer _skinnedMeshRenderer;

    public GameObject ItemToDrop;

    private void Awake()
    {
        _cc = GetComponent<CharacterController>();
        _animator = GetComponent<Animator>();

        _playerInput = GetComponent<PlayerInput>();
       
    }


    private void CalculatePlayerMovement()
    {
        _movementVelocity.Set(_playerInput.HorizontalInput, 0f, _playerInput.verticalInput);
        _movementVelocity.Normalize();
        _movementVelocity = Quaternion.Euler(0, -45f, 0) * _movementVelocity;

        _animator.SetFloat("Speed", _movementVelocity.magnitude);

        _movementVelocity *= MoveSpeed * Time.deltaTime;

        if (_movementVelocity != Vector3.zero)
            transform.rotation = Quaternion.LookRotation(_movementVelocity);

        _animator.SetBool("AirBorne", !_cc.isGrounded);
    }

    private void FixedUpdate()
    {
        CalculatePlayerMovement();
        
        if (_cc.isGrounded == false)
            _verticalVelocity = Gravity;
        else
            _verticalVelocity = Gravity * 0.3f;

        _movementVelocity += _verticalVelocity * Vector3.up * Time.deltaTime;

        _cc.Move(_movementVelocity);
        _movementVelocity = Vector3.zero;

    }
}
