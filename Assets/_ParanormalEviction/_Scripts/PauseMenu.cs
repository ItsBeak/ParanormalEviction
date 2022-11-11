using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;

public class PauseMenu : MonoBehaviour
{
    public GameObject PauseCanvas;
    public GameObject HUDCanvas;
    public AudioMixer Master;
    public Slider VolumeSlider;


    public void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        { 
            HUDCanvas.SetActive(false);
            PauseCanvas.SetActive(true);
            Time.timeScale = 0f;
        }
    }

    public void AudioVolume()
    {
        AudioListener.volume = VolumeSlider.value;
    }

    private void Load()
    {
        VolumeSlider.value = PlayerPrefs.GetFloat("musicVolume");
    }

    private void Save()
    {
        PlayerPrefs.SetFloat("musicVolume", VolumeSlider.value);
        Save();
    }

    public void resume()
    {
        PauseCanvas.SetActive(false);
        HUDCanvas.SetActive(true);
        Time.timeScale = 1f;
    }

    public void ReturnToMain()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Main Menu");
    }

    private void Start()
    {
        if(!PlayerPrefs.HasKey("musicVolume"))
        {
            PlayerPrefs.SetFloat("musicVolume", 1);
            Load();

        }

        else
        {
            Load();
        }
    }
}
