using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private List<SceneAsset> levels;
    [SerializeField] private SceneAsset mainMenuLevel;
    [SerializeField] private GameManager gameManager;
    
    private bool LevelFinished => GameManager.Players.AliveCount == 1 && GameManager.Players.Count > 1;
    private float _timeToNextLevel = 3f;
    private bool _loaded;
    
    private List<GameObject> _spawnPoints; 
    private List<GameObject> SpawnPointsOnScene => GameObject.FindGameObjectsWithTag("Spawnpoint").ToList();
    
    private void Start()
    {
        _spawnPoints = GameObject.FindGameObjectsWithTag("Spawnpoint").ToList();
    }

    private void Update()
    {
        if (GameManager.Players.AliveCount == 1 && gameManager.InProgress 
                                                && LevelFinished && _loaded)
        {
            StartCoroutine(LoadRandomLevel());
            _loaded = false;
        }
    }

    public IEnumerator LoadRandomLevel()
    {
        var level = levels[Random.Range(0, levels.Count)];
        yield return new WaitForSeconds(_timeToNextLevel);
        LoadLevel(level.name);
    }

    private void LoadLevel(string levelName)
    {
        SceneManager.LoadScene(levelName);  
        _spawnPoints = SpawnPointsOnScene;

        foreach (var player in GameManager.Players.players)
        {
            player.IGS_State = GameManager.PlayerIGS.Alive;
            SpawnPlayer(player.Instance);
        }

        _loaded = true;
    } 
    
    public void SpawnPlayer(GameObject player)
    {
        var point = _spawnPoints[Random.Range(0, _spawnPoints.Count)];
        var position = point.transform.position;
        player.GetComponent<Transform>().position = position;
        _spawnPoints.Remove(point);
    }
}
