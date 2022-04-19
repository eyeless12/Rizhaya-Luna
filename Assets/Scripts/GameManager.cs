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
            var player = players.First(p => p.ID == id);
            player.OGS_State = state;
        }

        public static bool IsAllReady => players.All(pi => pi.OGS_State == PlayerOGS.Ready);
        public static int AliveCount => players.Count(pi => pi.IGS_State == PlayerIGS.Alive);
        public static GameObject[] Alive => players
            .Where(pi => pi.IGS_State == PlayerIGS.Alive)
            .Select(pi => pi.Instance)
            .ToArray();
        public static int Count => players.Count;
    }
    
    private PlayerInputManager _playerManager;
    private List<GameObject> _players;
    private List<GameObject> _spawnpoints;
    private List<GameObject> _gunsOnSceneLoad;
    
    public bool InProgress { get; private set; }
    private List<GameObject> Spawnpoints
    {
        get => _spawnpoints;
        set => _spawnpoints = new List<GameObject>(value);
    }
    void Start()
    {
        DontDestroyOnLoad(gameObject);
        InProgress = false;
        _playerManager = GameObject.Find("PlayerManager").GetComponent<PlayerInputManager>();
        Spawnpoints = GameObject.FindGameObjectsWithTag("Spawnpoint").ToList();
        _gunsOnSceneLoad = GameObject.FindGameObjectsWithTag("Weapon").ToList();
    }

    void Update()
    {
        if (InProgress == false && Players.Count > 0 && Players.IsAllReady)
        {
            Load("level_01");
            InProgress = true;
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
