using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDamageCaster : MonoBehaviour
{
    private Collider _enemyDamageCasterCollider;
    public int damage =50;
    public string targetTag;
    private List<Collider> _damagedTarget;
    private void Awake() {
        _enemyDamageCasterCollider=GetComponent<Collider>();
        _enemyDamageCasterCollider.enabled=false;
        _damagedTarget=new List<Collider>();
    
    }

    private void OnTriggerEnter(Collider other) {
        if(other.tag==targetTag&&!_damagedTarget.Contains(other)){
            Character targetCC=other.GetComponent<Character>();
            if(targetCC!=null){
                targetCC.ApplyDamage(damage);
            }
            _damagedTarget.Add(other);

        }

    }

    public void EnableDamageCaster(){
        _damagedTarget.Clear();
        _enemyDamageCasterCollider.enabled=true;

    }

    public void DisableDamageCaster(){
        _damagedTarget.Clear();
        _enemyDamageCasterCollider.enabled=false;

    }
}
