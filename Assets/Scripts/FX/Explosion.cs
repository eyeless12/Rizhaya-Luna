using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Explosion : MonoBehaviour
{
    private void Awake()
    {
        SceneManager.sceneUnloaded += arg0 => Destroy(gameObject);
    }

    public void Cancel()
    {
        Destroy(gameObject);
    }
}
