using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;
using UnityEngine.UI; //Работа с интерфейсами
using UnityEngine.SceneManagement; //Работа со сценами
using UnityEngine.Audio;

public class Settings : MonoBehaviour
{
    public float volume; 
    public AudioMixer audioMixer; 
    public Dropdown resolutionDropdown; 
    private Resolution[] resolutions;
    private int currResolutionIndex;
    private Canvas canvas;

    private void Start()
    {
        AddResolutions();
        canvas = GetComponent<Canvas>();
        DontDestroyOnLoad(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            GoToMain();
        }
    }

    public void GoToMain()
    {
        if (SceneManager.GetActiveScene().name == "Menu")
        {
            canvas.enabled = false;
            return;
        }

        if (!canvas.enabled)
            return;
        canvas.enabled = false;
        GameObject.Find("MenuCanvas").GetComponent<Canvas>().enabled = true;
    }

    public void ChangeVolume(float val)
    {
        volume = val;
    }

    public void ChangeResolution(int index)
    {
        currResolutionIndex = index;
    }
    
    public void SaveSettings()
    {
        audioMixer.SetFloat("MasterVolume", volume); 
        Screen.SetResolution
            (Screen.resolutions[currResolutionIndex].width, Screen.resolutions[currResolutionIndex].height, true); //Изменения разрешения
    }

    public void AddResolutions()
    {
        resolutionDropdown.ClearOptions();
        resolutions = Screen.resolutions; 
        var options = new List<string> (); 

        for(var i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions [i].width + " x " + resolutions [i].height;
            options.Add(option);

            if(resolutions[i].Equals(Screen.currentResolution)) 
            {
                currResolutionIndex = i;
            }
        }

        resolutionDropdown.AddOptions(options); 
        resolutionDropdown.value = currResolutionIndex;
        resolutionDropdown.RefreshShownValue(); 
    }
}
