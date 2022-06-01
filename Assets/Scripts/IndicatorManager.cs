using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum Indicators
{
   Unready = 0,
   Ready = 1,
   Winner = 2
}

public class IndicatorManager : MonoBehaviour
{
    private class Indicator
    {
        public GameObject Instance;
        public GameObject AttachedPlayer;
        public Indicator(GameObject instance, GameObject player)
        {
            AttachedPlayer = player;
            Instance = Instantiate(
                instance,
                player.transform.position,
                Quaternion.identity);
            
            if (GameManager.InProgress)
                Instance.GetComponent<SpriteRenderer>().material =
                    player.GetComponent<SpriteRenderer>().material;
            
            Instance.SetActive(false);
        }

        public void FollowPlayer()
        {
            if (!Instance) return;
            
            var position = AttachedPlayer.transform.position;
            Instance.GetComponent<Transform>().position = new Vector3(
                position.x,
                position.y + 1.5f);
        }
    }
    
    private GameObject[] ActualPlayers => GameManager.Players.Alive;
    [SerializeField] private List<GameObject> prefabs;
    private readonly Dictionary<GameObject, Indicator> _attachedIndicators = new ();
    private Dictionary<Indicators, GameObject> _indicatorComparison;

    private void Awake()
    {
        _indicatorComparison = new Dictionary<Indicators, GameObject>
        {
            [Indicators.Unready] = prefabs[(int) Indicators.Unready],
            [Indicators.Ready] = prefabs[(int) Indicators.Ready],
            [Indicators.Winner] = prefabs[(int) Indicators.Winner]
        };
    }

    private void Update()
    {
        foreach (var indicator in _attachedIndicators.Values)
        {
            indicator.FollowPlayer();
        }
    }

    public void Attach(Indicators type, GameObject player)
    {
        if (_attachedIndicators.ContainsKey(player))
        {
            Destroy(_attachedIndicators[player].Instance);
            _attachedIndicators[player] = new Indicator(
                _indicatorComparison[type], 
                player);
        }
        else
        {
            _attachedIndicators.Add(player, new Indicator(
                _indicatorComparison[type], 
                player)); 
        }
    }

    public void DisableAll()
    {
        foreach (var indicator in _attachedIndicators.Values)
            indicator.Instance.SetActive(false);
    }

    public void Enable(GameObject player)
    {
        _attachedIndicators.TryGetValue(player, out var attached);
        if (attached is null)
        {
            Debug.Log($"No indicators attached to player {player.name}");
            return;
        }
        
        attached.Instance.SetActive(true);
    }
}
