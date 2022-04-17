using System;
using System.Collections;
using System.Collections.Generic;
using Unity;
using UnityEngine;
using UnityEngine.UIElements;

public class List : MonoBehaviour, IUsable
{
    private Animator _animator;
    private Player_Movement _player;
    private HashSet<Collider2D> _overlaps;
    void Start()
    {
        _overlaps = new HashSet<Collider2D>();
        _animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            _overlaps.Add(other);
            _animator.SetBool("inside", true);
        }
    }

    public void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            _overlaps.Remove(other);
            _animator.SetBool("inside", false);
        }
    }

    public void OnUse()
    {
        GameObject.Find("SettingsCanvas").GetComponent<Canvas>().enabled = true;
    }
}
