using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
    [SerializeField] private List<string> levels;
    [SerializeField] private GameManager gameManager;

    private bool LevelFinished => GameManager.Players.AliveCount <= 1 && GameManager.Players.Count > 1;
    private static float _timeToNextLevel = 3f;
    [HideInInspector] public static bool Loaded;
    public static bool Ended;
    
    private static List<GameObject> _spawnPoints;
    private static IndicatorManager _indicatorManager;
    private static List<GameObject> SpawnPointsOnScene => GameObject.FindGameObjectsWithTag("Spawnpoint").ToList();

    [SerializeField] private List<AudioSource> _music;
    private AudioSource playingNow;

    private AudioSource RandomMusic() => _music[Random.Range(0, _music.Count)];

    private void Start()
    {
        _spawnPoints = GameObject.FindGameObjectsWithTag("Spawnpoint").ToList();
        _indicatorManager = GetComponent<IndicatorManager>();
        playingNow = RandomMusic();
    }

    private void LateUpdate()
    {
        if (GameManager.InProgress && LevelFinished && Loaded)
        {
            StartCoroutine(GameManager.Players.UpdateScores());
            GameManager.PassedRound += 1;

            StartCoroutine(GameManager.PassedRound % GameManager.IntermissionFreq == 0
                ? LoadLevelWithDelay("Intermission", _timeToNextLevel)
                : LoadRandomLevel());
            
            Loaded = false;
        }

        if (GameManager.Endgame && Ended)
        {
            StartCoroutine(EndGame());
            Ended = false;
        }
              
        
        if (_spawnPoints.Count == 0)
            _spawnPoints = new List<GameObject>(SpawnPointsOnScene);
        var currentScene = SceneManager.GetActiveScene().name;

        if (!playingNow.isPlaying && currentScene != "Menu"
                                  && currentScene != "Start")
        {
            playingNow = RandomMusic();
            playingNow.Play();
        }

        if (playingNow.isPlaying && currentScene == "Menu")
        {
            playingNow.Stop();
        }
    }

    public IEnumerator LoadRandomLevel()
    {
        var level = levels[Random.Range(0, levels.Count)];
        StartCoroutine(LoadLevelWithDelay(level, _timeToNextLevel));
        yield return null;
    }

    private IEnumerator LoadLevelWithDelay(string level, float delay)
    {
        yield return new WaitForSeconds(delay);
        StartCoroutine(LoadLevel(level));
    }

    public static IEnumerator LoadLevel(string levelName)
    {
        yield return new WaitUntil(ClearMisc);
        SceneManager.LoadScene(levelName);
        yield return new WaitForFixedUpdate();
        
        _spawnPoints = new List<GameObject>(SpawnPointsOnScene);

        foreach (var player in GameManager.Players.players)
        {
            player.IGS_State = GameManager.PlayerIGS.Alive;
            SpawnPlayer(player.Instance, SpawnMode.Default);

            if (levelName == "Menu")
            {
                GameManager.InProgress = false;
                _indicatorManager.Attach(Indicators.Unready, player.Instance);
                _indicatorManager.Enable(player.Instance);
                Debug.Log("In menu loading");
            }
        }

        Loaded = true;
    }
    
    private IEnumerator EndGame()
    {
        yield return new WaitForSeconds(7f);
        GameManager.InProgress = false;
        foreach (var player in GameManager.Players.players)
        {
            player.CurrentScore = 0;
            player.BoardScore = 0;
        }

        yield return new WaitForEndOfFrame();
        StartCoroutine(LoadLevel("Menu"));
    }

    public static void SpawnPlayer(GameObject player, SpawnMode mode)
    {
        var point = _spawnPoints[Random.Range(0, _spawnPoints.Count)];
        var position = point.transform.position;
        player.GetComponent<Transform>().position = position;

        if (mode == SpawnMode.Default)
        {
            _spawnPoints.Remove(point);
        }
            
    }

    private static bool ClearMisc()
    {
        var misc = FindObjectsOfType<Item>();
        foreach (var item in misc)
        {
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
