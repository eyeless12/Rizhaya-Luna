using UnityEngine;
using UnityEngine.SceneManagement;

public class Explosion : MonoBehaviour
{
    private void Awake()
    {
        //SceneManager.sceneUnloaded += arg0 => Destroy(gameObject);
    }

    public void Cancel()
    {
        Destroy(gameObject);
    }
}
