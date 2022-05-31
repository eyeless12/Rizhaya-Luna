using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Afterdeath : MonoBehaviour
{
    private AudioSource _audio;

    private void Awake()
    {
        _audio = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if (!_audio.isPlaying)
            Destroy(gameObject);
    }
}
