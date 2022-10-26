using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuManager : MonoBehaviour
{
    public GameObject MenuCanvas;
    public GameObject CreditsCanvas;
    public void Play()
    {

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
