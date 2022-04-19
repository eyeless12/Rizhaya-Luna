using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private List<SceneAsset> levels;
    [SerializeField] private GameManager gameManager;

    private void Update()
    {
        if (GameManager.Players.AliveCount == 1 && gameManager.InProgress)
        {
            ChangeLevel();
        }
    }

    private void ChangeLevel()
    {
        var level = levels[Random.Range(0, levels.Count)];
        StartCoroutine(ChangeThreshold());
        gameManager.Load(level.name);
    }

    private IEnumerator ChangeThreshold()
    {
        yield return new WaitForSeconds(3f);
    }
}
