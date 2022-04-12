using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class Menu : MonoBehaviour
{
    public bool isOpened;

    private void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            ShowHideMenu();
    }
    
    
    private void ShowHideMenu()
    {
        isOpened = !isOpened;
        GetComponent<Canvas>().enabled = isOpened;
    }
    
    public void GoToMain()
    {
        SceneManager.LoadScene("Main Menu"); //Переход на сцену с названием Menu
    }
}
