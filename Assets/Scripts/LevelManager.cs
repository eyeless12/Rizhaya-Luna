using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public enum SpawnMode
{
    ForceSpawn,
    SafeSpawn,
    Default
}

public class LevelManager : MonoBehaviour
{
    [SerializeField] private List<SceneAsset> levels;
    [SerializeField] private SceneAsset mainMenuLevel;
    [SerializeField] private GameManager gameManager;
    
    private bool LevelFinished => GameManager.Players.AliveCount <= 1 && GameManager.Players.Count > 1;
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
        if (gameManager.InProgress && LevelFinished && _loaded)
        {
            StartCoroutine(LoadRandomLevel());
            _loaded = false;
        }

        if (_spawnPoints.Count == 0)
            _spawnPoints = SpawnPointsOnScene;
    }

    public IEnumerator LoadRandomLevel()
    {
        var level = levels[Random.Range(0, levels.Count)];
        yield return new WaitForSeconds(_timeToNextLevel);
        StartCoroutine(LoadLevel(level.name));
    }

    private IEnumerator LoadLevel(string levelName)
    {
        yield return new WaitUntil(ClearMisc);
        SceneManager.LoadScene(levelName);
        yield return new WaitForFixedUpdate();
        
        _spawnPoints = SpawnPointsOnScene;
        
        foreach (var player in GameManager.Players.players)
        {
            player.IGS_State = GameManager.PlayerIGS.Alive;
            SpawnPlayer(player.Instance, SpawnMode.Default);
        }

        _loaded = true;
    }

    public void SpawnPlayer(GameObject player, SpawnMode mode)
    {
        var point = _spawnPoints[Random.Range(0, _spawnPoints.Count)];
        var position = point.transform.position;
        player.GetComponent<Transform>().position = position;
        
        if (mode == SpawnMode.Default) 
            _spawnPoints.Remove(point);
    }

    private bool ClearMisc()
    {
        var misc = FindObjectsOfType<Item>();
        foreach (var item in misc)
        {
            Debug.Log(item.name);
            Destroy(item.gameObject);
        }

        foreach (var hands in GameManager.Players.players.Select(pi => pi.Instance.transform.Find("Hands")))
        {
            var item = hands.GetComponentInChildren<Item>();
            if (item) item.Delete();
        }

        return true;
    }
}
