using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    [Header("Room - Data")]
    public string roomName;

    public Transform[] teleportPoints;

    [Header("Room - Camera Targets")]

    public GameObject cameraBase;
    public Transform roomCentre;


    void Update()
    {
        cameraBase.SetActive(RoomManager.Instance.activeRoom == this);
    }
}
