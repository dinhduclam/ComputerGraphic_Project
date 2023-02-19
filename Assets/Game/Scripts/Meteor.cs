using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Meteor : MonoBehaviour
{
    public float Speed=2f;
    public int damage=10;
    private Rigidbody _rb;
    private void Awake() {
        _rb=GetComponent<Rigidbody>();
    }

    private void FixedUpdate() {
        _rb.MovePosition(transform.position+transform.forward*Speed*Time.deltaTime);

    }

    private void OnTriggerEnter(Collider other) {
        Character cc=other.gameObject.GetComponent<Character>();
        if(cc!=null){
            cc.ApplyDamage(damage);
            Destroy(gameObject);
        }

        
    }

  
}
