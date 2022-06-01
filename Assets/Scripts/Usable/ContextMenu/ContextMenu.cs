using System;
using System.Collections;
using System.Collections.Generic;
using Unity;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using Usable.ContextMenu;

public class ContextMenu : MonoBehaviour, IUsable
{
    private Animator _animator;
    private Player_Movement _player;
    private GameObject canvas;
    private Canvas objCanvas;
    [SerializeField] private UsableType _type;
    private static readonly int Inside = Animator.StringToHash("inside");

    private void Awake()
    {
        _animator = GetComponent<Animator>();

        if (_type is UsableType.Grm)
            canvas = GameObject.Find("SettingsCanvas");

        if (_type is UsableType.Listic)
        {
            canvas = GameObject.Find("Ctrl_canvas");
            canvas.GetComponent<Canvas>().worldCamera = Camera.current;
        }

        objCanvas = canvas.GetComponent<Canvas>();
    }

    private void FixedUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            objCanvas.enabled = false;
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            _animator.SetBool(Inside, true);
        }
    }

    public void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            _animator.SetBool(Inside, false);
            objCanvas.enabled = false;
        }
    }

    public void OnUse()
    {
        if (objCanvas.enabled)
            objCanvas.enabled = false;
        else
            canvas.GetComponent<Canvas>().enabled = true;
    }
}
