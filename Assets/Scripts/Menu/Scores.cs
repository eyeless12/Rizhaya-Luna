using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scores : MonoBehaviour
{
    [SerializeField] private GameObject pollPrefab;
    [SerializeField] private List<GameObject> points;
    private LevelManager _levelManager;
    
    private void Start()
    {
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

            tf.localScale = new Vector3(tf.localScale.x, player.BoardScore * 10f / 15f);
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
            tf.localScale = new Vector3(tf.localScale.x, player.CurrentScore * 10f / 15f);
            tf.position = new Vector3(tf.position.x, tf.position.y + tf.localScale.y / 2f);
            player.BoardScore = player.CurrentScore;
        }

        yield return new WaitForSeconds(1f);
        StartCoroutine(_levelManager.LoadRandomLevel());
    }
}
