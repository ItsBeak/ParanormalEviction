using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Possesion : MonoBehaviour
{
    public possesable Active;
    public GameObject mesh;
    PlayerMovement movement;
    public static Possesion Instance { get; private set;}
    public float timeRemaining = 10;
    public bool cooldown = false;

    public void Start()
    {
        movement = GetComponent<PlayerMovement>();
    }

    
    public void Update()
    {

        if (Active)
        {
            mesh.SetActive(false);
            movement.CanMove = false;

            if (Input.GetKeyDown(KeyCode.Q))
            {
                Active = null;
            }

            if (Input.GetKeyDown(KeyCode.Space) && cooldown == false)
            {
                Active.TriggerScare();
                cooldown = true;

            }

        }
        else
        {
            mesh.SetActive(true);
            movement.CanMove = true;
        }

        if (cooldown == true)
        {
            if (timeRemaining > 0 && cooldown)
            {
                timeRemaining -= Time.deltaTime;
            }
            else
            {
                Debug.Log("cooldown expired");
                cooldown = false;
                timeRemaining = 10;
            }
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
