using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public class PlayerInputHandler : MonoBehaviour
{
    private Player_Movement _playerMovement;
    [SerializeField] private List<GameObject> playerVariance;
    private List<GameObject> _pfbs;
    private GameManager _manager;
    private LevelManager _levelManager;
    private GameObject _player;
    private PlayerInput _playerInfo;

    private GameObject RandomPlayerPrefab
    {
        get
        {
            var obj = _pfbs[Random.Range(0, playerVariance.Count)];
            _pfbs.Remove(obj);
            return obj;
        }
    }

    void Awake()
    {
        DontDestroyOnLoad(gameObject);

        _pfbs = new List<GameObject>(playerVariance);
        var manager = GameObject.Find("GameManager");
        _manager = manager.GetComponent<GameManager>();
        _levelManager = manager.GetComponent<LevelManager>();
        
        _playerInfo = GetComponent<PlayerInput>();

        _player = Instantiate(
            RandomPlayerPrefab,
            transform.position,
            Quaternion.identity
        );
        
        _levelManager.SpawnPlayer(_player, SpawnMode.Default);
        _playerMovement = _player.GetComponent<Player_Movement>();
    }

    public void Move(InputAction.CallbackContext context)
    {
        if (_playerMovement && !_playerMovement.IsDead)
        {
            _playerMovement.Move(context.ReadValue<Vector2>());
        }
    }
    
    public void GoDownThroughPlatform(InputAction.CallbackContext context)
    {
        
        if(_playerMovement && context.performed && !_playerMovement.IsDead)
            _playerMovement.GoDownThroughPlatform();
    }

    public void Jump(InputAction.CallbackContext context)
    {
        if (_playerMovement && !_playerMovement.IsDead)
            _playerMovement.Jump(context);
    }

    public void Ready(InputAction.CallbackContext context)
    {
        if (_playerMovement && context.performed && !_playerMovement.IsDead)
        {
            GameManager.Players.SetReady(_playerInfo.playerIndex, GameManager.PlayerOGS.Ready);
        }
            
        
        if (_playerMovement && context.canceled && !_playerMovement.IsDead)
            GameManager.Players.SetReady(_playerInfo.playerIndex, GameManager.PlayerOGS.Unready); 
    }
    
    public void Pickup_Drop(InputAction.CallbackContext context)
    {
        if (_playerMovement && context.performed && !_playerMovement.IsDead)
        {
            _playerMovement.Pickup_Drop();
        }
    }
    
    public void Use(InputAction.CallbackContext context)
    {
        if (_playerMovement && context.performed && !_playerMovement.IsDead)
        {
            _playerMovement.Use();
            _playerMovement.Shoot();
        }
            
    }
}
