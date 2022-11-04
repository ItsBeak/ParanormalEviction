using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    public GameObject MenuCanvas;
    public GameObject CreditsCanvas;
    public void Play()
    {
        SceneManager.LoadScene("GameLevel");
    }

    public void Credits()
    {
        MenuCanvas.SetActive(false);
        CreditsCanvas.SetActive(true);
    }

    public void ReturnToMenu()
    {
        MenuCanvas.SetActive(true);
        CreditsCanvas.SetActive(false);
    }

    public void Quit()
    {
        Application.Quit();
    }
}
