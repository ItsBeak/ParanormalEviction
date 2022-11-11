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

    public void Start()
    {
        movement = GetComponent<PlayerMovement>();
        Possesing = false;
    }

    
    public void Update()
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
                    Active.interactionDisplay.text = "";
                    Active = null;
                    Possesing = false;
                    Debug.LogWarning("2nd");
                }

                else
                {
                    Debug.LogWarning("1st");
                    Possesing = true;
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
