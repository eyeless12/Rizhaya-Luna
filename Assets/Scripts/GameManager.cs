using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private bool[] _ready;
    private PlayerInputManager _playerManager;
    void Start()
    {
        DontDestroyOnLoad(gameObject);
        _playerManager = GameObject.Find("PlayerManager").GetComponent<PlayerInputManager>();
    }
    
    void Update()
    {
        if (_playerManager.playerCount > 0 && _ready.All(x => x is true))
            Load("level_01");
    }

    public void SetReady(int id, bool state)
    {
        _ready[id] = state;
    }

    public void OnJoin()
    {
        _ready = new bool[_playerManager.playerCount];
    }

    public void Load(string level) => SceneManager.LoadScene(level);
}
