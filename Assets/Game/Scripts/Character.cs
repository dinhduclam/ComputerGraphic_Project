using Unity.VisualScripting;
using UnityEngine;

public class Character : MonoBehaviour
{
    private CharacterController _cc;
    private Animator _animator;
    private PlayerInput _playerInput;

    public float WalkSpeed = 3f;
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
        Normal, Jump, Attacking, Dead, BeingHit
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
        _movementVelocity.Set(_playerInput.HorizontalInput, 0f, _playerInput.VerticalInput);
        _movementVelocity.Normalize();
        _movementVelocity = Quaternion.Euler(0, -45f, 0) * _movementVelocity;

        MoveSpeed = WalkSpeed;

        if (_cc.isGrounded && _playerInput.Run)
        {
            MoveSpeed = RunSpeed;
        }

        _animator.SetFloat("Speed", _movementVelocity.magnitude * MoveSpeed);

        if (_movementVelocity != Vector3.zero)
            transform.rotation = Quaternion.LookRotation(_movementVelocity);
    }

    private void FixedUpdate()
    {
        switch (CurrentState)
        {
            case PlayerState.Normal:
                _movementVelocity = Vector3.zero;
                CalculatePlayerMovement();
                
                if (_cc.isGrounded)
                {
                    if (_playerInput.MouseButtonDown)
                    {
                        SwitchState(PlayerState.Attacking);
                        return;
                    }

                    if (_playerInput.SpaceKeyDown)
                    {
                        SwitchState(PlayerState.Jump);
                        return;
                    }
                }
                break;

            case PlayerState.Jump:
                break;
        }

        _movementVelocity.y = _verticalVelocity;
        _cc.Move(_movementVelocity * MoveSpeed * Time.deltaTime);

        if (_cc.isGrounded == false)
            _verticalVelocity += Gravity * Time.deltaTime;
        else
            _verticalVelocity = 0;
    }

    private void SwitchState(PlayerState newState)
    {
        //Exiting state
        switch (CurrentState)
        {
            case PlayerState.Normal:
                break;

            case PlayerState.Jump:
                _playerInput.SpaceKeyDown = false;
                break;
                        
            case PlayerState.Attacking:
                _playerInput.MouseButtonDown = false;
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

            case PlayerState.Jump:
                _verticalVelocity += MoveSpeed > 5f? 2.5f : 4f;
                _animator.SetTrigger("Jump");
                break;

            case PlayerState.Attacking:
                _animator.SetTrigger("Attack");
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

    public void FinishAttack()
    {
        SwitchState(PlayerState.Normal);
    }

    public void FinishJump() 
    {
        SwitchState(PlayerState.Normal);
    }
}