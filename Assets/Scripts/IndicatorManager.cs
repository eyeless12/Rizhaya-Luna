using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum Indicators
{
   Unready = 0,
   Ready = 1
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
    private readonly List<Indicator> _attachedIndicators = new();
    private Dictionary<GameObject, GameObject> _playerComparison = new ();
    private Dictionary<Indicators, GameObject> _indicatorComparison;

    private void Awake()
    {
        _indicatorComparison = new Dictionary<Indicators, GameObject>
        {
            [Indicators.Unready] = prefabs[(int) Indicators.Unready],
            [Indicators.Ready] = prefabs[(int) Indicators.Ready]
        };
    }

    private void Update()
    {
        foreach (var indicator in _attachedIndicators)
        {
            indicator.FollowPlayer();
        }
    }

    public void Attach(Indicators type, GameObject player)
    {
        var old = _attachedIndicators.FirstOrDefault(at => at.AttachedPlayer == player);
        if (old != null)
        {
            _attachedIndicators.Remove(old);
            Destroy(old.Instance);
        }
        
        _attachedIndicators.Add(new Indicator(_indicatorComparison[type], player));
        if (_playerComparison.ContainsKey(player))
            _playerComparison[player] = _indicatorComparison[type];
        else 
            _playerComparison.Add(player, _indicatorComparison[type]);
    }

    public void DisableAll()
    {
        foreach (var indicator in _attachedIndicators)
            indicator.Instance.SetActive(false);
    }

    public void Enable(GameObject player)
    {
        var attached = _attachedIndicators.FirstOrDefault(at => at.AttachedPlayer == player);
        if (attached is null)
        {
            Debug.Log($"No indicators attached to player {player.name}");
            return;
        }
        
        attached.Instance.SetActive(true);
    }
}
