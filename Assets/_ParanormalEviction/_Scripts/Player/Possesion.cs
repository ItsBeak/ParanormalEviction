using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Possesion : MonoBehaviour
{
    public Possesable Active;
    public SkinnedMeshRenderer rend;
    PlayerMovement movement;
    bool Possesing;
    public static Possesion Instance { get; private set;}

    [Header("Audio")]
    AudioSource source;
    public AudioClip possessEnter;
    public AudioClip possessExit;

    float ghostFadeAmount;

    public void Start()
    {
        movement = GetComponent<PlayerMovement>();
        Possesing = false;
        source = GetComponent<AudioSource>();
        ghostFadeAmount = 1f;
    }

    
    public void LateUpdate()
    {

        if (Active)
        {
            movement.CanMove = false;

            Active.interactionDisplay.text = "Press Space to Scare";

            if (Input.GetKeyDown(KeyCode.E))
            {
                if (Possesing)
                {
                    Active.interactionDisplay.text = "Press E to Possess";
                    Active = null;
                    Possesing = false;
                    source.PlayOneShot(possessExit);
                }
                else
                {
                    Possesing = true;
                    source.PlayOneShot(possessEnter);
                }
            }
            

            if (Input.GetKeyDown(KeyCode.Space))
            {
                Active.TriggerScare();
                

            }

        }
        else
        {
            movement.CanMove = true;
        }

    }

    private void Update()
    {
        if (Possesing)
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

    public void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

}
