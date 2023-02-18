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

    public int maxHeal;
    public int currentHealth;
    private EnemyDamageCaster damageCaster;

    public enum EnemyState{
        Normal, Attacking,Dead,BeingHit
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
        
    }

    private void CalculateEnemyMovement(){
        if(Vector3.Distance(_TargetPlayer.position,transform.position)>= _NavMeshAgent.stoppingDistance){
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
            
        }

    }

    private void SwitchState(EnemyState newState){

        switch(newState){
            case EnemyState.Attacking:
                Quaternion newRotation=Quaternion.LookRotation(_TargetPlayer.position-transform.position);
                transform.rotation=newRotation;
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
        }

        CurrentState = newState;
    }

    public void FinishAttack(){
        
        SwitchState(EnemyState.Normal);
    }
    public void ApplyDamage(int damage){
        
        currentHealth-=damage;
        SwitchState(EnemyState.BeingHit);
        CheckHealth();
        Debug.Log(currentHealth);


    }

    public void EnableDamageCaster(){
        damageCaster.EnableDamageCaster();

    }

    public void DisableDamageCaster(){
        damageCaster.DisableDamageCaster();

    }

    private void CheckHealth(){
        if(currentHealth<=0){
            SwitchState(EnemyState.Dead);
        }

    }
  

    public void DropItem(){
        if(itemToDrop!=null){
            Instantiate(itemToDrop,transform.position,Quaternion.identity);
        }

    }
    public void BeingHitAnimationEnds(){
        SwitchState(EnemyState.Normal);
    }
}
