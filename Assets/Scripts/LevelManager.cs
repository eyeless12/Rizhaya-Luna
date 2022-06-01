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
    private float _timeToNextLevel = 3f;
    private static bool _loaded;
    
    private static List<GameObject> _spawnPoints;
    private static IndicatorManager _indicatorManager;
    private static List<GameObject> SpawnPointsOnScene => GameObject.FindGameObjectsWithTag("Spawnpoint").ToList();
    
    private void Start()
    {
        _spawnPoints = GameObject.FindGameObjectsWithTag("Spawnpoint").ToList();
        _indicatorManager = GetComponent<IndicatorManager>();
    }

    private void Update()
    {
        if (GameManager.InProgress && LevelFinished && _loaded)
        {
            StartCoroutine(GameManager.Players.UpdateScores());
            StartCoroutine(LoadRandomLevel());
            _loaded = false;
        }

        if (_spawnPoints.Count == 0)
            _spawnPoints = new List<GameObject>(SpawnPointsOnScene);
    }

    public IEnumerator LoadRandomLevel()
    {
        var level = levels[Random.Range(0, levels.Count)];
        yield return new WaitForSeconds(_timeToNextLevel);
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
                _indicatorManager.Attach(Indicators.Unready, player.Instance);
                _indicatorManager.Enable(player.Instance);
            }
        }

        _loaded = true;
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
