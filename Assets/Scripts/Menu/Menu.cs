using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    private bool isOpened;
    private Canvas canvas;

    private void Start()
    {
        canvas = GetComponent<Canvas>();
        DontDestroyOnLoad(gameObject);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            ShowHideMenu();
    }
    
    public void ShowHideMenu()
    {
        if (SceneManager.GetActiveScene().name == "Menu")
            return;
        
        if (GameObject.Find("SettingsCanvas").GetComponent<Canvas>().enabled)
            return;
        
        isOpened = !isOpened;
        canvas.enabled = isOpened;
    }
    
    public void GoToMain()
    {
        if (SceneManager.GetActiveScene().name != "Menu")
        {
            StartCoroutine(LevelManager.LoadLevel("Menu"));
            GameManager.InProgress = false;
        }

        canvas.enabled = false;
    }

    public void ShowSettings()
    {
        GetComponent<Canvas>().enabled = false;
        GameObject.Find("SettingsCanvas").GetComponent<Canvas>().enabled = true;
    }
}
