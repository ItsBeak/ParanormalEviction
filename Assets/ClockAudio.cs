using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClockAudio : MonoBehaviour
{

    AudioSource source;
    int state = 0;

    public AudioClip tickingClip;

    [Space(20)]

    public AudioClip endBuildupClip;
    public int endBuildupTime;

    [Space(20)]

    public AudioClip endClip;
    public int endTime;

    private void Start()
    {
        source = GetComponent<AudioSource>();

        source.clip = tickingClip;
        source.loop = true;
        state = 0;
        source.Play();
    }

    void Update()
    {
        if (GameManager.Instance.timer <= endBuildupTime && state == 0)
        {
            source.Stop();
            source.PlayOneShot(endBuildupClip);
            state++;
        }

        if (GameManager.Instance.timer <= endTime && state == 1)
        {
            source.PlayOneShot(endClip);
            state++;
            source.loop = false;

        }
    }
}
