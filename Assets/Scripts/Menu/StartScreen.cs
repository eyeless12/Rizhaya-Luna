using UnityEngine;

public class StartScreen : MonoBehaviour
{
    private void Update()
    {
        CheckStart();
    }

    private void CheckStart()
    {
        if (GameManager.GameStarted)
            Destroy(gameObject);
    }
}
