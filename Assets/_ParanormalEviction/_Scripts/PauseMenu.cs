using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public GameObject PauseCanvas;
    public GameObject HUDCanvas;


    public void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            HUDCanvas.SetActive(false);
            PauseCanvas.SetActive(true);
            Time.timeScale = 0f;
        }
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
        SceneManager.LoadScene("MainMenu");
    }

}
