using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    public GameObject cameraRig;
    public Transform roomCentre;

    void Update()
    {
        cameraRig.SetActive(RoomManager.Instance.activeRoom == this);
    }
}
