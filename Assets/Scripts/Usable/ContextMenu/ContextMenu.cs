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

    private void Start()
    {
        _animator = GetComponent<Animator>();
    }
    
    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            _animator.SetBool("inside", true);
        }
    }

    public void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            _animator.SetBool("inside", false);
        }
    }

    public void OnUse()
    {
        //Debug.Log("USED");
        canvas.GetComponent<Canvas>().enabled = true;
    }
}
