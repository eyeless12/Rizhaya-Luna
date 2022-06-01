using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
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
        public int Score;
        public PlayerOGS OGS_State;
        public PlayerIGS IGS_State;

        public PlayerInfo(GameObject instance, int id, PlayerOGS ogs, PlayerIGS igs)
        {
            Instance = instance;
            ID = id;
            Score = 0;
            OGS_State = ogs;
            IGS_State = igs;
        }
    }

    public static class Players
    {
        public static readonly List<PlayerInfo> players = new();

        private static readonly Dictionary<PlayerOGS, Indicators> StateToIndicator = new()
        {
            [PlayerOGS.Unready] = Indicators.Unready,
            [PlayerOGS.Ready] = Indicators.Ready
        };

        private static void AddPlayer(GameObject instance)
        {
            if (instance == null) return;
            players.Add(new PlayerInfo(instance, players.Count, PlayerOGS.Unready, PlayerIGS.Alive));
        }
        
        public static void SetReady(int id, PlayerOGS state)
        {
            var player = players.First(pi => pi.ID == id);
            player.OGS_State = state;
            if (InProgress) return;
            
            _indicatorManager.Attach(StateToIndicator[state], player.Instance);
            _indicatorManager.Enable(player.Instance);
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
                    _indicatorManager.Attach(Indicators.Unready, player);
                    _indicatorManager.Enable(player);
                }
        }

        public static IEnumerator UpdateScores()
        {
            if (AliveCount != 1) yield break;
            yield return new WaitForSeconds(2f);
            if (AliveCount != 1) yield break;
            
            var player = GetPlayerByInstance(Alive.First());
            player.Score += 1;
            _indicatorManager.Attach(Indicators.Winner, player.Instance);
            _indicatorManager.Enable(player.Instance);
            
            Debug.Log($"Player {player.Instance} has {player.Score} score!");
        } 

        public static PlayerInfo GetPlayerByInstance(GameObject instance)
        {
            return players.First(p => p.Instance == instance);
        }

        public static bool IsAllReady => players.All(pi => pi.OGS_State == PlayerOGS.Ready);
        public static int AliveCount => players.Count(pi => pi.IGS_State == PlayerIGS.Alive);
        public static GameObject[] Alive => players
            .Where(pi => pi.IGS_State == PlayerIGS.Alive)
            .Select(pi => pi.Instance)
            .ToArray();
        
        public static GameObject[] Dead => players
            .Where(pi => pi.IGS_State == PlayerIGS.Dead)
            .Select(pi => pi.Instance)
            .ToArray();
        public static int Count => players.Count;
    }
    
    private PlayerInputManager _playerManager;
    private LevelManager _levelManager;
    private static IndicatorManager _indicatorManager;
    private GameObject _menuEventManager;
    private List<GameObject> _gunsOnSceneLoad;
    private bool _newConnected;
    private MultipleTargetCamera _multipleTargetCamera;
    [SerializeField] private GameObject popUpMenuAllReady;

    public static Shake CameraShake;
    public static bool InProgress { get; private set; }
    public static bool GameStarted;
    
    void Start()
    {
        DontDestroyOnLoad(gameObject);
        InProgress = false;
        _newConnected = false;
        
        _playerManager = GameObject.Find("PlayerManager").GetComponent<PlayerInputManager>();
        _indicatorManager = GetComponent<IndicatorManager>();
        _menuEventManager = GameObject.Find("EventSystem");
        _levelManager = GetComponent<LevelManager>();
        _multipleTargetCamera = GameObject.FindWithTag("CameraHolder").GetComponentInChildren<MultipleTargetCamera>();
        CameraShake = GameObject.FindWithTag("MainCamera").GetComponent<Shake>();
        
        DontDestroyOnLoad(_menuEventManager.gameObject);
    }

    private void Update()
    {
        if (InProgress == false)
        {
            StartCoroutine(ForceSpawnDeadPlayers());
        }
        if (InProgress == false && Players.Count > 0 && Players.IsAllReady)
        {
            _indicatorManager.DisableAll();
            StartCoroutine(_levelManager.LoadRandomLevel());
            InProgress = true;
            popUpMenuAllReady.SetActive(true);
        }

        if (_newConnected)
        {
            Players.UpdatePlayers();
            _multipleTargetCamera.players = new List<Transform>(Players.players.Select(pi => pi.Instance.GetComponent<Transform>())); 
            _newConnected = false;
        }
    }

    private IEnumerator ForceSpawnDeadPlayers()
    {
        if (Players.AliveCount == Players.Count)
            yield break;

        yield return new WaitForSeconds(2f);

        foreach (var candidate in Players.Dead)
        {
            Players.GetPlayerByInstance(candidate).IGS_State = PlayerIGS.Alive;
            _levelManager.SpawnPlayer(candidate, SpawnMode.ForceSpawn); 
            Debug.Log($"{candidate.name} force spawned");
        }
    }

    public void OnJoin()
    {
        _newConnected = true;
    }
}
