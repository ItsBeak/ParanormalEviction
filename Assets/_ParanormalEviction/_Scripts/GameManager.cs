using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("Component References")]
    public Image timerFill;
    public Transform timerHand;
    public Text guestDisplay;
    public AITracker tracker;

    [Header("Timer")]
    public float gameTimeMax;
    public float timer;

    [Header("Guests")]
    public int guestTarget;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }

        timer = gameTimeMax;
    }

    void Update()
    {
        guestDisplay.text = (guestTarget - tracker.WinCount).ToString(); // + "/" + guestTarget.ToString()
        timerFill.fillAmount = timer / gameTimeMax;

        timerHand.rotation = Quaternion.Euler(0, 0, ((timer / gameTimeMax) * 90) - 90);

        timer -= 1 * Time.deltaTime;

        if (tracker.WinCount >= guestTarget)
        {
            SceneManager.LoadScene("WinScene");
        }
        else if (timer < 0)
        {
            SceneManager.LoadScene("LoseScene");
        }

    }
}
