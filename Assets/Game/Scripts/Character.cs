using Unity.VisualScripting;
using UnityEngine;

public class Character : MonoBehaviour
{
    private CharacterController _cc;
    private Animator _animator;
    private PlayerInput _playerInput;

    public float WalkSpeed = 2f;
    public float RunSpeed = 6f;
    public float MoveSpeed = 2f;
    public float JumpSpeed = 2f;
    public float _verticalVelocity = 0f;
    public float Gravity = -9.8f;

    private Vector3 _movementVelocity;

    //Player slides
    private float attackStartTime;
    public float AttackSlideDuration = 0.4f;
    public float AttackSlideSpeed = 0.06f;

    //State Machine
    public enum PlayerState
    {
        Normal, Attacking, Dead, BeingHit
    }
    public PlayerState CurrentState;

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

        MoveSpeed = WalkSpeed;

        if (_cc.isGrounded)
        {
            if (_playerInput.Run)
            {
                MoveSpeed = RunSpeed;
                _animator.SetBool("Run", true);
            }
            else
            {
                _animator.SetBool("Run", false);
            }
        }

        Debug.Log(_movementVelocity.magnitude);

        _animator.SetFloat("Speed", _movementVelocity.magnitude * MoveSpeed);

        if (_movementVelocity != Vector3.zero)
            transform.rotation = Quaternion.LookRotation(_movementVelocity);
    }

    private void FixedUpdate()
    {
        CalculatePlayerMovement();

        if (_cc.isGrounded && _playerInput.MouseButtonDown)
        {
            SwitchState(PlayerState.Attacking);
        }

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

    private void SwitchState(PlayerState newState)
    {
        //Exiting state
        switch (CurrentState)
        {
            case PlayerState.Normal:
                break;

            case PlayerState.Attacking:

                //if (_damageCaster != null)
                //    _damageCaster.DisableDamageCaster();

                break;

            case PlayerState.Dead:
                return;

            case PlayerState.BeingHit:
                break;
        }

        //Entering state
        switch (newState)
        {
            case PlayerState.Normal:
                break;

            case PlayerState.Attacking:
                _animator.SetTrigger("Attack");
                _playerInput.MouseButtonDown = false;
                break;

            case PlayerState.Dead:
                _cc.enabled = false;
                _animator.SetTrigger("Dead");
                break;

            case PlayerState.BeingHit:
                _animator.SetTrigger("BeingHit");
                break;

        }

        CurrentState = newState;

        Debug.Log("Switched to " + CurrentState);
    }
}