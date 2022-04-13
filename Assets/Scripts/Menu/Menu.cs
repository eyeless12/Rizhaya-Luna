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
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            ShowHideMenu();
    }
    
    
    public void ShowHideMenu()
    {
        if (GameObject.Find("SettingsCanvas").GetComponent<Canvas>().enabled)
            return;
        isOpened = !isOpened;
        canvas.enabled = isOpened;
    }
    
    public void GoToMain()
    {
        SceneManager.LoadScene("Main Menu");
    }

    public void ShowSettings()
    {
        GetComponent<Canvas>().enabled = false;
        GameObject.Find("SettingsCanvas").GetComponent<Canvas>().enabled = true;
    }
}
