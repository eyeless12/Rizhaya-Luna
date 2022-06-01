using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class PickPlayerVariance : MonoBehaviour
{
    [SerializeField] private List<GameObject> playerVariance;
    private List<GameObject> _pfbs;
    
    public GameObject RandomPlayerPrefab
    {
        get
        {
            var obj = _pfbs[Random.Range(0, _pfbs.Count)];
            _pfbs.Remove(obj);
            return obj;
        }
    }

    private void Awake()
    {
        _pfbs = new List<GameObject>(playerVariance);
    }
}
