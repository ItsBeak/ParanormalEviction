using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerScare : MonoBehaviour
{

    public float scariness;

    public float scareRadius;
    public GameObject scareRadiusIndicator;

    public LayerMask guestLayer;
    public Text radiusReadout;

    void Update()
    {

        scareRadiusIndicator.transform.localScale = new Vector3(scareRadius * 2, 0.05f, scareRadius * 2);
        radiusReadout.text = "Radius: " + scareRadius.ToString();

        if (Input.GetKeyDown(KeyCode.Space))
        {
            PossessionScare(scareRadius);
        }

        if (Input.GetKeyDown(KeyCode.Backslash))
        {
            scareRadiusIndicator.SetActive(!scareRadiusIndicator.activeSelf);
        }

        if (Input.GetKeyDown(KeyCode.LeftBracket))
        {
            scareRadius -= 0.25f;
        }

        if (Input.GetKeyDown(KeyCode.RightBracket))
        {
            scareRadius += 0.25f;
        }

    }

    void PossessionScare(float radius)
    {
        Collider[] hitGuests = Physics.OverlapSphere(transform.position, radius, guestLayer);

        if (hitGuests.Length == 0)
        {
            return;
        }

        foreach (Collider guest in hitGuests)
        {
            guest.GetComponent<SanityManager>().Scare(scariness);
        }

    }

}
