using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SanityManager : MonoBehaviour
{

    public SanityGauge gauge;

    public float sanityLevel;

    public float sanityLevelMax = 100f;


    private void Start()
    {
        sanityLevel = sanityLevelMax;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            Scare(10);
        }

        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            Calm(10);
        }

        if (sanityLevel > sanityLevelMax)
        {
            sanityLevel = sanityLevelMax;
        }
        if (sanityLevel < 0)
        {
            sanityLevel = 0;
        }

    }


    public void Scare(float amount)
    {

        sanityLevel -= amount;

        gauge.SetFillAmount(sanityLevel);

        // LOCHLAN - CALL CHECK STATE FUNCTION ON GUEST
        
    }

    public void Calm(float amount)
    {
        sanityLevel += amount;
        gauge.SetFillAmount(sanityLevel);
    }
}
