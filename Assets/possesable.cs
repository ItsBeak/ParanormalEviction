using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class possesable : MonoBehaviour
{
    [Header("Components")]
    public Animator[] animators;
    public AudioSource[] audioSources;
    public ParticleSystem[] particleSystems;

    [Header("Parameters")]
    public float scariness;
    public float scareRadius;
    public LayerMask guestLayer;
    public float timeRemaining;
    float timeStore;


    bool canPossess;
    bool cooldown = false;

    public void Start()
    {
        timeStore = timeRemaining;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && canPossess)
        {
            Possesion.Instance.Active = this;
            Debug.LogWarning("Setting active object");
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
                timeRemaining = timeStore;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player" && Possesion.Instance.Active == null)
        {
            canPossess = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            canPossess = false;
        }
    }

    public void TriggerScare()
    {
        cooldown = true;

        Debug.Log("Scaring from object: " + gameObject);

        Collider[] hitGuests = Physics.OverlapSphere(transform.position, scareRadius, guestLayer);

        if (hitGuests.Length == 0)
        {
            Debug.Log("No guests, early outing");

            return;
        }

        Debug.Log(hitGuests.Length + " guests hit");

        foreach (Collider guest in hitGuests)
        {
            guest.GetComponent<SanityManager>().Scare(scariness);
        }

    }

}
