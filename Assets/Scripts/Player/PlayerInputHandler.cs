using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public class PlayerInputHandler : MonoBehaviour
{
    private Player_Movement _playerMovement;
    [SerializeField] private GameObject playerVariance;
    private GameManager _manager;
    private LevelManager _levelManager;
    private GameObject _player;
    private PlayerInput _playerInfo;
    
    void Awake()
    {
        DontDestroyOnLoad(gameObject);
        
        var manager = GameObject.Find("GameManager");
        _manager = manager.GetComponent<GameManager>();
        _levelManager = manager.GetComponent<LevelManager>();
        
        _playerInfo = GetComponent<PlayerInput>();

        _player = Instantiate(
            playerVariance,
            transform.position,
            Quaternion.identity
        );
        
        _levelManager.SpawnPlayer(_player);
        _playerMovement = _player.GetComponent<Player_Movement>();
    }

    public void Move(InputAction.CallbackContext context)
    {
        if (_playerMovement)
        {
            _playerMovement.Move(context.ReadValue<Vector2>());
        }
    }
    
    public void GoDownThroughPlatform(InputAction.CallbackContext context)
    {
        
        if(_playerMovement && context.performed)
            _playerMovement.GoDownThroughPlatform();
    }

    public void Jump(InputAction.CallbackContext context)
    {
        if (_playerMovement)
            _playerMovement.Jump(context);
    }

    public void Ready(InputAction.CallbackContext context)
    {
        if (_playerMovement && context.performed)
        {
            GameManager.Players.SetReady(_playerInfo.playerIndex, GameManager.PlayerOGS.Ready);
        }
            
        
        if (_playerMovement && context.canceled)
            GameManager.Players.SetReady(_playerInfo.playerIndex, GameManager.PlayerOGS.Unready); 
    }
    
    public void Pickup_Drop(InputAction.CallbackContext context)
    {
        if (_playerMovement && context.performed)
        {
            _playerMovement.Pickup_Drop();
        }
    }
    
    public void Use(InputAction.CallbackContext context)
    {
        if (_playerMovement && context.performed)
        {
            _playerMovement.Use();
            _playerMovement.Shoot();
        }
            
    }
}
