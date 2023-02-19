using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    private List<SpawnPoint> spawnPointList;
    private bool hasSpawned;
    public Collider collider;
    private List<Enemy> spawnedEnemy;
    private void Awake() {
        var spawnPointArray=transform.parent.GetComponentsInChildren<SpawnPoint>();
        spawnPointList=new List<SpawnPoint>(spawnPointArray);
        spawnedEnemy=new List<Enemy>();
    }

    public void SpawnCharacters(){
        if(hasSpawned){
            return;
        }
        hasSpawned=true;
        foreach(SpawnPoint point in spawnPointList){
            if(point.enemyToSpawn!=null){
                GameObject spawnedGameObject=Instantiate(point.enemyToSpawn,point.transform.position,Quaternion.identity);
                spawnedEnemy.Add(spawnedGameObject.GetComponent<Enemy>());
            }
        }
    }

    public bool AllEnemyDead()
    {
        foreach (Enemy e in spawnedEnemy)
        {
            if (e.CurrentState != Enemy.EnemyState.Dead)
            {
                return false;
            }
        }

        return true;
    }

    private void OnTriggerEnter(Collider other) {
        if(other.tag=="Player"){
            SpawnCharacters();
        }
    }

    private void OnDrawGizmos() {
        Gizmos.color=Color.red;
        Gizmos.DrawWireCube(transform.position,collider.bounds.size);
    }
}
