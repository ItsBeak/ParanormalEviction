using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class possesable : MonoBehaviour
{
    [Header("Components")]
    public Animator[] animators;
    public AudioSource[] audioSources;

    [Header("Parameters")]
    public float scariness;
    public float scareRadius;
    public LayerMask guestLayer;
    public float scareCooldown;


    bool canPossess;
    bool isOnCooldown;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && canPossess)
        {
            Possesion.Instance.Active = this;
            Debug.LogWarning("Setting active object");
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
