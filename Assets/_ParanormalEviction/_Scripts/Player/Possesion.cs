using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Possesion : MonoBehaviour
{
    public Possesable Active;
    public GameObject mesh;
    PlayerMovement movement;
    bool Possesing;
    public static Possesion Instance { get; private set;}

    [Header("Audio")]
    AudioSource source;
    public AudioClip possessEnter;
    public AudioClip possessExit;

    public void Start()
    {
        movement = GetComponent<PlayerMovement>();
        Possesing = false;
        source = GetComponent<AudioSource>();
    }

    
    public void LateUpdate()
    {

        if (Active)
        {
            mesh.SetActive(false);
            movement.CanMove = false;

            Active.interactionDisplay.text = "Press Space to Scare";

            if (Input.GetKeyDown(KeyCode.Q))
            {
                if (Possesing == true)
                {
                    Active.interactionDisplay.text = "Press Q to Possess";
                    Active = null;
                    Possesing = false;
                    Debug.LogWarning("2nd");
                    source.PlayOneShot(possessExit);
                }

                else
                {
                    Debug.LogWarning("1st");
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
            mesh.SetActive(true);
            movement.CanMove = true;
        }

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
