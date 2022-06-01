using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputHandler : MonoBehaviour
{
    private Player_Movement _playerMovement;
    private GameObject _playerManager;
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
        _playerManager = GameObject.Find("PlayerManager");
        
        _playerInfo = GetComponent<PlayerInput>();

        _player = Instantiate(
            _playerManager.GetComponent<PickPlayerVariance>().RandomPlayerPrefab,
            transform.position,
            Quaternion.identity
        );

        GameManager.GameStarted = true;
        LevelManager.SpawnPlayer(_player, SpawnMode.Default);
        _playerMovement = _player.GetComponent<Player_Movement>();
        
        Debug.Log(_playerMovement.IsDead);
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
        if (_playerMovement && !_playerMovement.IsDead)
        {
            _playerMovement.Shoot(context);
            _playerMovement.Use();
        }
    }
}
