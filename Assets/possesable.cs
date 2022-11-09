using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class possesable : MonoBehaviour
{
    [Header("Components")]
    public Animator[] animators;
    public AudioSource[] audioSources;
    public ParticleSystem[] particleSystems;
    public Text interactionDisplay;

    [Header("Parameters")]
    public float scariness;
    public float scareRadius;
    public LayerMask guestLayer;
    float timer;
    public float cooldownTime;

    public Image cooldownIndicator;

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

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player" && Possesion.Instance.Active == null)
        {
            canPossess = true;
            interactionDisplay.text = "Press E to Possess";
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
                particles.Play();
            }
        }

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
