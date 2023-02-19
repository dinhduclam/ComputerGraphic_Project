using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private CharacterController _CharacterController;
    private Animator _Animator;
    public float moveSpeed;
    private  UnityEngine.AI.NavMeshAgent _NavMeshAgent;
    private Transform _TargetPlayer;

    public bool isInVincible;
    public float invincibleDuration=2f;

    public int maxHeal;
    public int currentHealth;
    private EnemyDamageCaster damageCaster;

    private float currentSpawnTime;

    public float spawnDuration=2f;

    public enum EnemyState{
        Normal, Attacking,Dead,BeingHit,Spawn
    };

    public EnemyState CurrentState;

    public GameObject itemToDrop;

    private void Awake() {
        currentHealth=maxHeal;
        _CharacterController=GetComponent<CharacterController>();
        _Animator=GetComponent<Animator>();
        _NavMeshAgent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        damageCaster=GetComponentInChildren<EnemyDamageCaster>();
        _TargetPlayer=GameObject.FindWithTag("Player").transform;
        SwitchState(EnemyState.Spawn);
        
    }

    private void CalculateEnemyMovement(){
        if(Vector3.Distance(_TargetPlayer.position,transform.position)>= _NavMeshAgent.stoppingDistance){
            Debug.Log(_TargetPlayer.position);
            _NavMeshAgent.SetDestination(_TargetPlayer.position);
            _Animator.SetFloat("Speed",0.2f);

        }else{
            _NavMeshAgent.SetDestination(transform.position);
            _Animator.SetFloat("Speed",0);
            SwitchState(EnemyState.Attacking);
        }
    }
    private void FixedUpdate() {
        switch(CurrentState){
            case EnemyState.Normal:
                CalculateEnemyMovement();
                break;
            case EnemyState.Dead:
                return;
            case EnemyState.BeingHit:
                break;
            case EnemyState.Spawn:
                isInVincible=false;
                currentSpawnTime-=Time.deltaTime;
                if(currentSpawnTime<=0){
                    SwitchState(EnemyState.Normal);
                }
                break;
            
        }

    }

    private void SwitchState(EnemyState newState){
        switch(newState){
            case EnemyState.Attacking:
                //Quaternion newRotation=Quaternion.LookRotation(_TargetPlayer.position-transform.position);
                //transform.rotation=newRotation;
                _Animator.SetTrigger("Attack");
                break;
            case EnemyState.Dead:

                _Animator.SetTrigger("Dead");
                DropItem();
                Destroy(this.gameObject,3f);
                break;
            case EnemyState.BeingHit:
                if(CurrentState!=EnemyState.Attacking){
                    _Animator.SetTrigger("BeingHit");
                }
                break;
            case EnemyState.Spawn:
                isInVincible=true;
                currentSpawnTime=spawnDuration;
                break;
        }

        CurrentState = newState;
    }

    public void FinishAttack(){
        if (CurrentState != EnemyState.Dead)
            SwitchState(EnemyState.Normal);
    }

    public void ApplyDamage(int damage){
        if (CurrentState == EnemyState.Dead || isInVincible)
            return;

        currentHealth-=damage;
        if (currentHealth <= 0)
        {
            SwitchState(EnemyState.Dead);
            return;
        }

        SwitchState(EnemyState.BeingHit);
        Debug.Log(currentHealth);
    }

    IEnumerator DelayCancelInvinCible(){
        yield return new WaitForSeconds(invincibleDuration);
        isInVincible=false;
    }

    public void EnableDamageCaster(){
        damageCaster.EnableDamageCaster();
    }

    public void DisableDamageCaster(){
        damageCaster.DisableDamageCaster();
    }

    public void DropItem(){
        if(itemToDrop!=null){
            Instantiate(itemToDrop,transform.position,Quaternion.identity);
        }
    }
    public void BeingHitAnimationEnds(){
        if (CurrentState != EnemyState.Dead)
            SwitchState(EnemyState.Normal);
    }

    public void RotateToTarget(){
        if(CurrentState!=EnemyState.Dead){
            transform.LookAt(_TargetPlayer,Vector3.up);
        }
    }
}
