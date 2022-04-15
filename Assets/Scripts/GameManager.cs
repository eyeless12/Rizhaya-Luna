using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private bool[] _ready = new bool[0];
    private PlayerInputManager _playerManager;
    private List<GameObject> _players;
    private List<GameObject> _spawnpoints;
    private List<GameObject> _gunsOnSceneLoad;
    private List<GameObject> Spawnpoints
    {
        get => _spawnpoints;
        set => _spawnpoints = new List<GameObject>(value);
    }
    void Start()
    {
        DontDestroyOnLoad(gameObject);
        _playerManager = GameObject.Find("PlayerManager").GetComponent<PlayerInputManager>();
        Spawnpoints = GameObject.FindGameObjectsWithTag("Spawnpoint").ToList();
        _gunsOnSceneLoad = GameObject.FindGameObjectsWithTag("Weapon").ToList();
    }

    void Update()
    {
        if (_playerManager.playerCount > 0 && _ready.All(x => x is true))
            Load("level_01");
    }

    public void SetReady(int id, bool state)
    {
        _ready[id] = state;
    }

    public void OnJoin()
    {
        _ready = new bool[_playerManager.playerCount];
        _players = GameObject.FindGameObjectsWithTag("Player").ToList();
    }

    public void Load(string level)
    {
        SceneManager.LoadScene(level);
        _players = GameObject.FindGameObjectsWithTag("Player").ToList();
        //Debug.Log(_players[0]);
        Spawnpoints = GameObject.FindGameObjectsWithTag("Spawnpoint").ToList();
        Debug.Log(Spawnpoints[0]);
        //Debug.Log(Spawnpoints);
        
        foreach (var player in _players)
        {
            SpawnPlayer(player);
            //Debug.Log(player);
        }
            
    }

    public void SpawnPlayer(GameObject player)
    {
        var point = Spawnpoints[Random.Range(0, Spawnpoints.Count)];
        var position = point.transform.position;
        player.GetComponent<Transform>().position = position;
        Spawnpoints.Remove(point);
    }
    
}
