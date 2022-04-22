using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

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
            var player = players.First(pi => pi.ID == id);
            player.OGS_State = state;
            Debug.Log(player.OGS_State);
        }

        public static void SetIGS(GameObject player, PlayerIGS state)
        {
            var candidate = players.First(pi => pi.Instance == player);
            candidate.IGS_State = state;
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
    private MultipleTargetCamera _camera;


    public bool InProgress { get; private set; }
    void Start()
    {
        DontDestroyOnLoad(gameObject);
        InProgress = false;
        _newConnected = false;
        _playerManager = GameObject.Find("PlayerManager").GetComponent<PlayerInputManager>();
        _levelManager = GetComponent<LevelManager>();
        _camera = GameObject.FindWithTag("MainCamera").GetComponent<MultipleTargetCamera>();
    }

    void Update()
    {
        if (InProgress == false && Players.Count > 0 && Players.IsAllReady)
        {
            StartCoroutine(_levelManager.LoadRandomLevel());
            InProgress = true;
            //_camera.zoomEnabled = true;
        }

        if (_newConnected)
        {
            Players.UpdatePlayers();
            _camera.players = new List<Transform>(Players.players.Select(pi => pi.Instance.GetComponent<Transform>())); 
            _newConnected = false;
        }
    }

    public void OnJoin()
    {
        _newConnected = true;
    }
}
