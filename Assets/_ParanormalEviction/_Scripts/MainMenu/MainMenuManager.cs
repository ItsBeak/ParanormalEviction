using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    public GameObject MenuCanvas;
    public GameObject CreditsCanvas;
    public GameObject TutorialCanvas;
    public void Play()
    {
        MenuCanvas.SetActive(false);
        TutorialCanvas.SetActive(true);
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

    public void Continue()
    {
        SceneManager.LoadScene("GameScene");
    }
    public void Quit()
    {
        Application.Quit();
    }
}
