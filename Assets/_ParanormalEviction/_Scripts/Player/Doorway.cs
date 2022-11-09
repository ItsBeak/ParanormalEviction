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

    bool contact;
    BoxCollider trigger;

    [Header("Temporary Variables")]
    public Material contactTrue;
    public Material contactFalse;
    public MeshRenderer rend;

    public Text doorReadout;

    void Start()
    {
        rend.material = contactFalse;
        doorReadout = GameObject.Find("DoorReadout").GetComponent<Text>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && contact)
        {
            RoomManager.Instance.ChangeRoom(targetRoom, locationID);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            contact = true;
            rend.material = contactTrue;
            doorReadout.text = "Press E to Enter Door";
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            contact = false;
            rend.material = contactFalse;
            doorReadout.text = "";
        }
    }



}
