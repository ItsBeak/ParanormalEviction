using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using TMPro;

public class Doorway : MonoBehaviour
{
    [Header("Door Settings")]
    public Room targetRoom;
    public int locationID;

    public bool contact = false;
    BoxCollider trigger;

    [Header("Audio")]
    AudioSource source;
    public AudioClip doorClip;

    [Header("Debug Variables")]
    public Material contactTrue;
    public Material contactFalse;
    public MeshRenderer rend;

    public Text doorReadout;

    void Start()
    {
        rend.material = contactFalse;
        doorReadout.text = "";
        doorReadout = GameObject.Find("DoorReadout").GetComponent<Text>();
        source = GetComponent<AudioSource>();
    }

    void Update()
    {
        
        if (Input.GetKeyDown(KeyCode.E) && contact)
        {
            RoomManager.Instance.ChangeRoom(targetRoom, locationID);
            source.PlayOneShot(doorClip);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            contact = true;
            rend.material = contactTrue;
            doorReadout.text = "Press E to Enter Door";
            RoomManager.Instance.isInDoorway = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            contact = false;
            rend.material = contactFalse;
            doorReadout.text = "";
            RoomManager.Instance.isInDoorway = false;
        }
    }



}
