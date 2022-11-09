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
    float timer;

    [Header("Guests")]
    public int guestTarget;
    [HideInInspector]public int guestsScared;

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
        guestDisplay.text = tracker.WinCount + "/" + guestTarget.ToString();
        timerFill.fillAmount = (timer / 4) / gameTimeMax;

        timerHand.rotation = Quaternion.Euler(0, 0, ((timer / gameTimeMax) * 90 * -1) + 90);

        timer -= 1 * Time.deltaTime;

        if (guestsScared >= guestTarget)
        {
            SceneManager.LoadScene("WinScene");
        }
        else if (timer < 0)
        {
            SceneManager.LoadScene("LoseScene");
        }

    }
}
