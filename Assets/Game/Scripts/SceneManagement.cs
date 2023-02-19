using UnityEngine.SceneManagement;
using UnityEngine;

public class SceneManagement : MonoBehaviour
{
    [SerializeField] int sceneToLoad=-1;
    private void OnTriggerEnter(Collider other) {
        if(other.tag=="Player"){
            SceneManager.LoadScene(sceneToLoad);
        }
    }
}
