using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Scores : MonoBehaviour
{
    [SerializeField] private GameObject pollPrefab;
    [SerializeField] private List<GameObject> points;
    [SerializeField] private GameObject crown;
    private LevelManager _levelManager;
    private GameObject _winner;
    private int _shows;
    private bool _showed;
    private bool ShowWinner => _shows == GameManager.Players.players.Count && _winner;
    private List<GameObject> _polls;

    private readonly Func<bool> _loaded = () => LevelManager.Loaded;

    private void Start()
    {
        _polls = new List<GameObject>();
        StartCoroutine(WaitForLoading());
    }

    private void LateUpdate()
    {
        if (ShowWinner && !_showed)
        {
            var instance = Instantiate(crown, Vector2.up * 20f, Quaternion.identity);
            instance.GetComponent<SpriteRenderer>().material = _winner.GetComponent<SpriteRenderer>().material;
            instance.GetComponent<Transform>().rotation = Quaternion.Euler(0, 0, 10f) * instance.GetComponent<Transform>().rotation;
            _showed = true;
        }
    }

    private IEnumerator WaitForLoading()
    {
        yield return new WaitUntil(_loaded);
        
        _levelManager = GameObject.Find("GameManager").GetComponent<LevelManager>();
        SortPlayers();
        DrawScores();
        StartCoroutine(EditScores());
    }

    private void SortPlayers()
    {
        foreach (var player in GameManager.Players.players)
        {
            player.Instance.GetComponent<Transform>().position = 
                points[player.ID].GetComponent<Transform>().position;
            Debug.Log(player.Instance.transform.position);
        }
    }

    private void DrawScores()
    {
        foreach (var player in GameManager.Players.players)
        {
            Vector2 position = new Vector2(player.Instance.GetComponent<Transform>().position.x, 4.5f);
            var poll = Instantiate(pollPrefab, position, Quaternion.identity);
            poll.GetComponent<SpriteRenderer>().material = player.Instance.GetComponent<SpriteRenderer>().material;
            poll.GetComponent<SpriteRenderer>().sortingOrder = 10;
            var tf = poll.GetComponent<Transform>();

            tf.localScale = new Vector3(tf.localScale.x, player.BoardScore);
            tf.position = new Vector3(tf.position.x, tf.position.y + tf.localScale.y / 2);
            
            _polls.Add(poll);
        }
    }

    private IEnumerator EditScores()
    {
        foreach (var player in GameManager.Players.players)
        {
            var position = new Vector2(_polls[player.ID].transform.position.x, 4.5f);
            var poll = Instantiate(pollPrefab, position, Quaternion.identity);
            poll.GetComponent<SpriteRenderer>().sortingOrder = 8;
            var tf = poll.GetComponent<Transform>();

            if (player.CurrentScore > 10)
                Camera.main.orthographicSize = 10;
            
            yield return new WaitForSeconds(1f);
            
            tf.localScale = new Vector3(tf.localScale.x, player.CurrentScore);
            tf.position = new Vector3(tf.position.x, tf.position.y + tf.localScale.y / 2f);
            player.BoardScore = player.CurrentScore;
            _shows += 1;
        }

        _winner = GameManager.Players.players.OrderByDescending(pl => pl.BoardScore).FirstOrDefault(pi => pi.BoardScore >= GameManager.MaxRounds)?.Instance;

        yield return new WaitForSeconds(1f);
        LevelManager.Ended = true;

        if (!GameManager.Endgame)
        {
            StartCoroutine(_levelManager.LoadRandomLevel());
        }
    }
}
