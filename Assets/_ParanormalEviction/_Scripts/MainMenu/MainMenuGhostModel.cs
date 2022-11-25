using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuGhostModel : MonoBehaviour
{

    bool toggle;

    float ghostFadeAmount = 1f;

    public float switchTimer;
    public SkinnedMeshRenderer rend;

    public AudioSource source;
    public AudioClip inClip;
    public AudioClip outClip;

    private void Start()
    {
        toggle = true;

        Invoke("StartFadeLoop", 2f);
    }

    public void StartFadeLoop()
    {
        InvokeRepeating("SwitchFade", 0f, switchTimer);
    }

    public void SwitchFade()
    {
        toggle = !toggle;

        if (!toggle)
        {
            source.PlayOneShot(inClip);
        }
        else
        {
            source.PlayOneShot(outClip);
        }
    }

    private void Update()
    {
        if (toggle)
        {
            if (ghostFadeAmount < 1)
            {
                ghostFadeAmount += Time.deltaTime * 2;
            }
            if (ghostFadeAmount > 1)
            {
                ghostFadeAmount = 1;
            }
        }
        else
        {
            if (ghostFadeAmount > 0)
            {
                ghostFadeAmount -= Time.deltaTime * 2;
            }
            if (ghostFadeAmount < 0)
            {
                ghostFadeAmount = 0;
            }
        }

        rend.material.SetFloat("DissolveAmount", ghostFadeAmount);

    }

}
