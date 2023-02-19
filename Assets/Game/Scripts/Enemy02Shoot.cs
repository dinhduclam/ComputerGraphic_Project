using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy02Shoot : MonoBehaviour
{
    public Transform shootingPoint;
    public GameObject meteor;
    private Enemy enemy;
    private void Awake() {
        enemy=GetComponent<Enemy>();
    }
    public void ShootTheMeteor(){
        Instantiate(meteor,shootingPoint.position,Quaternion.LookRotation(shootingPoint.forward));
    }

    private void Update() {
        enemy.RotateToTarget();
    }
}
