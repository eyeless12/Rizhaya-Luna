using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public class PlayerInputHandler : MonoBehaviour
{
    private Player_Movement _player;
    [SerializeField] private GameObject playerVariance;
    private List<GameObject> _spawnpoints = new List<GameObject>();
    private GameManager _manager;
    private PlayerInput _playerInfo;

    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(gameObject);
        
        _spawnpoints = GameObject.FindGameObjectsWithTag("Spawnpoint").ToList();
        Debug.Log("Handler");
        _manager = GameObject.Find("GameManager").GetComponent<GameManager>();
        _playerInfo = GetComponent<PlayerInput>();
        
        var start = _spawnpoints[Random.Range(0, _spawnpoints.Count)];
        _player = Instantiate(
            playerVariance, 
            start.transform.position, 
            Quaternion.identity
            ).GetComponent<Player_Movement>();
        Destroy(start);
    }

    public void Move(InputAction.CallbackContext context)
    {
        if (_player)
        {
            _player.Move(context.ReadValue<Vector2>());
        }
            
    }

    public void Jump(InputAction.CallbackContext context)
    {
        if (_player)
            _player.Jump(context);
    }

    public void Shoot(InputAction.CallbackContext context)
    {
        if (_player)
        {
            //Debug.Log("Handler");
            _player.Shoot(context);
        }
    }

    public void Ready(InputAction.CallbackContext context)
    {
        if (_player && context.performed)
            _manager.SetReady(_playerInfo.playerIndex, true);
        
        if (_player && context.canceled)
            _manager.SetReady(_playerInfo.playerIndex, false); 
    }

    public void Use(InputAction.CallbackContext context)
    {
        if (_player)
            _player.Use(context);
    }
}
