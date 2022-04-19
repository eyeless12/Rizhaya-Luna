using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public enum PlayerOGS
    {
        Unready,
        Ready,
        AFK
    }

    public enum PlayerIGS
    {
        Dead, 
        Alive
    }
    
    public struct PlayerInfo
    {
        public readonly GameObject Instance;
        public readonly int ID;
        public PlayerOGS OGS_State;
        public PlayerIGS IGS_State;

        public PlayerInfo(GameObject instance, int id, PlayerOGS ogs, PlayerIGS igs)
        {
            Instance = instance;
            ID = id;
            OGS_State = ogs;
            IGS_State = igs;
        }
    }

    public static class Players
    {
        public static readonly List<PlayerInfo> players = new();
        public static void AddPlayer(GameObject instance)
        {
            if (instance == null) return;
            players.Add(new PlayerInfo(instance, players.Count, PlayerOGS.Unready, PlayerIGS.Alive));
        }
        
        public static void SetReady(int id, PlayerOGS state)
        {
            var player = Players.players.First(p => p.ID == id);
            player.OGS_State = state;
        }

        public static bool IsAllReady => players.All(pi => pi.OGS_State == PlayerOGS.Ready);
        public static int Count => players.Count;
    }
    
    // private bool[] _ready = new bool[0];
    private PlayerInputManager _playerManager;
    private List<GameObject> _players;
    private List<GameObject> _spawnpoints;
    private List<GameObject> _gunsOnSceneLoad;
    private bool _inProgress;
    private List<GameObject> Spawnpoints
    {
        get => _spawnpoints;
        set => _spawnpoints = new List<GameObject>(value);
    }
    void Start()
    {
        DontDestroyOnLoad(gameObject);
        _inProgress = false;
        _playerManager = GameObject.Find("PlayerManager").GetComponent<PlayerInputManager>();
        Spawnpoints = GameObject.FindGameObjectsWithTag("Spawnpoint").ToList();
        _gunsOnSceneLoad = GameObject.FindGameObjectsWithTag("Weapon").ToList();
    }

    void Update()
    {
        if (Players.Count > 0 && _inProgress == false && Players.IsAllReady)
        {
            Load("level_01");
            _inProgress = true;
        }
    }

    public void OnJoin()
    {
        var players = GameObject.FindGameObjectsWithTag("Player").ToList();
        foreach (var player in players)
            if (!Players.players.Select(pi => pi.Instance).Contains(player))
                Players.AddPlayer(player);
    }

    public void Load(string level)
    {
        SceneManager.LoadScene(level);
        _players = GameObject.FindGameObjectsWithTag("Player").ToList();
        Spawnpoints = GameObject.FindGameObjectsWithTag("Spawnpoint").ToList();
        Debug.Log(Spawnpoints[0]);

        foreach (var player in _players)
        {
            SpawnPlayer(player);
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
