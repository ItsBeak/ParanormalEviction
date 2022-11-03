using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    void Start()
    {
        rend.material = contactFalse;
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
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            contact = false;
            rend.material = contactFalse;
        }
    }



}
