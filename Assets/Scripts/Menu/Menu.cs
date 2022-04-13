using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    public bool isOpened;
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            ShowHideMenu();
    }
    
    
    private void ShowHideMenu()
    {
        if (GameObject.Find("SettingsCanvas").GetComponent<Canvas>().enabled)
            return;
        
        isOpened = !isOpened;
        GetComponent<Canvas>().enabled = isOpened;
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
