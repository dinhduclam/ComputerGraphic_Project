using Unity.VisualScripting;
using UnityEngine;

public class Character : MonoBehaviour
{
    private CharacterController _cc;
    public float WalkSpeed = 2f;
    public float RunSpeed = 4f;
    public float MoveSpeed = 2f;
    public float JumpSpeed = 3f;
    private Vector3 _movementVelocity;
    private PlayerInput _playerInput;
    public float _verticalVelocity = 0f;
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

        if (_cc.isGrounded)
        {
            if (_playerInput.Run){
                MoveSpeed = RunSpeed;
                _animator.SetBool("Run", true);
            }
            else
            {
                _animator.SetBool("Run", false);
                MoveSpeed = WalkSpeed;
            }

            if (_playerInput.MouseButtonDown)
            {
                _animator.SetTrigger("Attack");
                _playerInput.MouseButtonDown = false;
            }
        }
            

        _animator.SetFloat("Speed", _movementVelocity.magnitude);

        if (_movementVelocity != Vector3.zero)
            transform.rotation = Quaternion.LookRotation(_movementVelocity);

        //_animator.SetBool("AirBorne", !_cc.isGrounded);
    }

    private void FixedUpdate()
    {
        CalculatePlayerMovement();
        
        if (_cc.isGrounded == false)
            _verticalVelocity += Gravity * Time.deltaTime;
        else _verticalVelocity = 0;


        if (_playerInput.SpaceKey)
        {
            if (_cc.isGrounded)
            {
                _verticalVelocity += JumpSpeed;
                _animator.SetBool("Jump", true);
            }
                
        }
        else _animator.SetBool("Jump", false);

        _movementVelocity.y = _verticalVelocity;

        _cc.Move(_movementVelocity * MoveSpeed * Time.deltaTime);
        _movementVelocity = Vector3.zero;

    }
}
