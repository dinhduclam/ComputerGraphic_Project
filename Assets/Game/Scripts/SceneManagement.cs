using UnityEngine.SceneManagement;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class SceneManagement : MonoBehaviour
{
    public IEnumerable<Spawner> spawners;
    private void Awake()
    {
        var spawnerGameObjects = GameObject.FindGameObjectsWithTag("Spawner");
        spawners = spawnerGameObjects.Select(x => x.GetComponent<Spawner>());
    }

    [SerializeField] int sceneToLoad=-1;
    private void OnTriggerEnter(Collider other) {
        if(other.tag=="Player"){
            if (AllEnemyDead())
                SceneManager.LoadScene(sceneToLoad);
        }
    }

    private bool AllEnemyDead()
    {
        foreach (var s in spawners)
        {
            if (!s.AllEnemyDead()) return false;
        }
        return true;
    }
}
