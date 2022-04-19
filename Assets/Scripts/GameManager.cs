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
    
    public class PlayerInfo
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

        private static void AddPlayer(GameObject instance)
        {
            if (instance == null) return;
            players.Add(new PlayerInfo(instance, players.Count, PlayerOGS.Unready, PlayerIGS.Alive));
        }
        
        public static void SetReady(int id, PlayerOGS state)
        {
            var player = players.First(p => p.ID == id);
            player.OGS_State = state;
            Debug.Log(player.OGS_State);
        }

        public static void UpdatePlayers()
        {
            var playersOnScene = GameObject.FindGameObjectsWithTag("Player").ToList();
            foreach (var player in playersOnScene)
                if (!players.Select(pi => pi.Instance).Contains(player))
                {
                    AddPlayer(player);
                    Debug.Log("ADDED");
                }
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
    private LevelManager _levelManager;
    private List<GameObject> _gunsOnSceneLoad;
    private bool _newConnected;

    public bool InProgress { get; private set; }
    void Start()
    {
        DontDestroyOnLoad(gameObject);
        InProgress = false;
        _newConnected = false;
        _playerManager = GameObject.Find("PlayerManager").GetComponent<PlayerInputManager>();
        _levelManager = GetComponent<LevelManager>();
    }

    void Update()
    {
        Debug.Log($"Is all Ready : {Players.IsAllReady}");
        Debug.Log($"Players count : {Players.Count}");
        if (Players.players.Count == 1)
        {
            var player = Players.players.First();
            Debug.Log($"Player-{player.ID} OGS: {player.OGS_State}");
        }
            
        if (InProgress == false && Players.Count > 0 && Players.IsAllReady)
        {
            Debug.Log("SWITCH");
            StartCoroutine(_levelManager.LoadRandomLevel());
            InProgress = true;
        }

        if (_newConnected)
        {
            Players.UpdatePlayers();
            _newConnected = false;
        }
    }

    public void OnJoin()
    {
        _newConnected = true;
    }
}
