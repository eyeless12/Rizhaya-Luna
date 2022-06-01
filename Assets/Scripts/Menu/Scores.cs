using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Scores : MonoBehaviour
{
    [SerializeField] private GameObject pollPrefab;
    [SerializeField] private List<GameObject> points;
    private LevelManager _levelManager;

    private readonly Func<bool> _loaded = () => LevelManager.Loaded;

    private void Start()
    {
        StartCoroutine(WaitForLoading());
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
            var tf = poll.GetComponent<Transform>();

            tf.localScale = new Vector3(tf.localScale.x, player.BoardScore);
            tf.position = new Vector3(tf.position.x, tf.position.y + tf.localScale.y / 2);
        }
    }

    private IEnumerator EditScores()
    {
        foreach (var player in GameManager.Players.players)
        {
            Vector2 position = new Vector2(player.Instance.GetComponent<Transform>().position.x, 4.5f);
            var poll = Instantiate(pollPrefab, position, Quaternion.identity);
            poll.GetComponent<SpriteRenderer>().material = player.Instance.GetComponent<SpriteRenderer>().material;
            var tf = poll.GetComponent<Transform>();

            yield return new WaitForSeconds(1f);
            tf.localScale = new Vector3(tf.localScale.x, player.CurrentScore);
            tf.position = new Vector3(tf.position.x, tf.position.y + tf.localScale.y / 2f);
            player.BoardScore = player.CurrentScore;
        }

        yield return new WaitForSeconds(1f);
        StartCoroutine(GameManager.Endgame ? LevelManager.LoadLevel("Menu") :_levelManager.LoadRandomLevel());
    }
}
