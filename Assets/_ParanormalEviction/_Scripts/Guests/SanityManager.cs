using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SanityManager : MonoBehaviour
{

    public SanityGauge gauge;
    [HideInInspector] public AIMovement Guest;

    public float sanityLevel;

    public float sanityLevelMax = 100f;


    private void Start()
    {
        sanityLevel = sanityLevelMax;
        Guest = GetComponent<AIMovement>();
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
        if (Guest.Scareable == true)
        {
            if (Guest.Idle)
                sanityLevel -= amount;
            sanityLevel -= amount;

            gauge.SetFillAmount(sanityLevel);          
        }
        Guest.Scared = true;
    }

    public void Calm(float amount)
    {
        sanityLevel += amount;
        gauge.SetFillAmount(sanityLevel);
    }
}
