using Unity.VisualScripting;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    private CharacterController _cc;
    private Animator _animator;
    private PlayerInput _playerInput;
    private PlayerDamageCaster _playerDamageCaster;

    public float WalkSpeed = 3f;
    public float RunSpeed = 6f;
    public float MoveSpeed = 2f;
    public float WalkingJump = 2f;
    public float RuningJump = 6f;
    public float _verticalVelocity = 0f;
    public float Gravity = -9.8f;
    public bool isInVincible;
    public float invincibleDuration=2f;

    private Vector3 _movementVelocity;

    //State Machine
    public enum PlayerState
    {
        Normal, Jump, Attacking, Dead, BeingHit
    }
    public PlayerState CurrentState;

    //Health
    public int MaxHealth = 100;
    public int Health = 100;

    private void Awake()
    {
        _cc = GetComponent<CharacterController>();
        _animator = GetComponent<Animator>();
        _playerInput = GetComponent<PlayerInput>();
        _playerDamageCaster = GetComponentInChildren<PlayerDamageCaster>();
    }

    private void FixedUpdate()
    {
        switch (CurrentState)
        {
            case PlayerState.Normal:
                _movementVelocity = Vector3.zero;
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

            case PlayerState.Attacking:
                _movementVelocity = Vector3.zero;
                break;
            case PlayerState.BeingHit:
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
                _verticalVelocity += MoveSpeed == WalkSpeed? WalkingJump : RuningJump;
                _animator.SetTrigger("Jump");
                break;

            case PlayerState.Attacking:
                _animator.SetTrigger("Attack");
                //attackStartTime = Time.time;
                break;

            case PlayerState.Dead:
                _cc.enabled = false;
                _animator.SetTrigger("Dead");
                break;

            case PlayerState.BeingHit:
                _animator.SetTrigger("BeingHit");
                isInVincible=true;
                StartCoroutine(DelayCancelInvinCible());
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

    public void ApplyDamage(int damage)
    {
        if(isInVincible){
            return;
        }
        Health -= damage;
        SwitchState(PlayerState.BeingHit);
    }
    IEnumerator DelayCancelInvinCible(){
        yield return new WaitForSeconds(invincibleDuration);
        isInVincible=false;
    }


    

    public void EnableDamageCaster()
    {
        _playerDamageCaster.EnableDamageCaster();
    }

    public void DisableDamageCaster()
    {
        _playerDamageCaster.DisableDamageCaster();
    }
    public void PickUpItem(PickUp item){
        switch(item.Type){
            case PickUp.PickUpType.Heal:
                AddHealth(item.value);
                break;
        }
    }

    public void AddHealth(int health){
        Health+=health;
        if(Health>MaxHealth){
            Health=MaxHealth;
        }
    }
    public void BeingHitAnimationEnds(){
        SwitchState(PlayerState.Normal);
    }
}