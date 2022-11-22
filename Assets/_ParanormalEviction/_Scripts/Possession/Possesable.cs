using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Possesable : MonoBehaviour
{
    [Header("Scare Settings")]
    /// <summary>
    /// The amount to scare guests by
    /// </summary>
    public float scariness;
    /// <summary>
    /// The radius in units the scare reaches
    /// </summary>
    public float scareRadius;
    /// <summary>
    /// The time this object is on cooldown once triggered
    /// </summary>
    public float cooldownTime;
    /// <summary>
    /// Once the scare is triggered, the time before the scare collision check happens around the object (used to time with animations)
    /// </summary>
    public float timeBeforeScareTrigger;

    [Header("Components")]

    public Animator[] animators;
    public AudioSource[] audioSources;
    public ParticleSystem[] particleSystems;
    [HideInInspector] public Text interactionDisplay;
    public Image cooldownIndicator;

    [Header("Parameters")]
    
    public LayerMask guestLayer;

    float timer;
    bool canPossess;
    bool cooldown;

    public void Start()
    {
        timer = 0;
        interactionDisplay = GameObject.Find("DoorReadout").GetComponent<Text>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && canPossess)
        {
            Possesion.Instance.Active = this;
            Debug.LogWarning("Setting active object");
        }

        timer -= Time.deltaTime;

        cooldown = timer > 0;

        cooldownIndicator.enabled = timer > 0;
        cooldownIndicator.fillAmount = (timer / cooldownTime);

    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Player" && Possesion.Instance.Active == null)
        {
            if (RoomManager.Instance.isInDoorway == false)
            {
                canPossess = true;
                interactionDisplay.text = "Press E to Possess";
            }
            else
            {
                canPossess = false;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            canPossess = false;
            interactionDisplay.text = "";
        }
    }

    public void TriggerScare()
    {

        if (cooldown)
            return;

        timer = cooldownTime;

        Debug.Log("Scaring from object: " + gameObject);

        if (animators.Length != 0)
        {
            foreach (Animator anim in animators)
            {
                anim.SetTrigger("Scare");
            }
        }

        if (audioSources.Length != 0)
        {
            foreach (AudioSource source in audioSources)
            {
                source.Play();
            }
        }

        if (particleSystems.Length != 0)
        {
            foreach (ParticleSystem particles in particleSystems)
            {
                particles.Stop();
                particles.time = 0;
                particles.Play();
            }
        }

        Invoke("Scare", timeBeforeScareTrigger);

    }

    void Scare()
    {
        Collider[] hitGuests = Physics.OverlapCapsule(new Vector3(transform.position.x, transform.position.y - 3, transform.position.z), new Vector3(transform.position.x, transform.position.y + 3, transform.position.z), scareRadius, guestLayer);
        
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
