using System;
using System.Collections;
using System.Collections.Generic;
using Unity;
using UnityEngine;
using UnityEngine.UIElements;

public class ContextMenu : MonoBehaviour, IUsable
{
    private Animator _animator;
    private Player_Movement _player;
    [SerializeField] private GameObject canvas;
    private Canvas objCanvas;
    private static readonly int Inside = Animator.StringToHash("inside");

    private void Start()
    {
        objCanvas = canvas.GetComponent<Canvas>();
        _animator = GetComponent<Animator>();
    }

    private void Update()
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
