using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Room : MonoBehaviour
{
    [Header("Room - Data")]
    public string roomName;

    public Transform[] teleportPoints;

    [Header("Room - Camera Targets")]

    public GameObject cameraBase;
    public Transform roomCentre;

    [Header("Room - UI")]

    public Image roomIndicator;
    public Color activeColour;
    public Color inactiveColour;

    void Update()
    {
        cameraBase.SetActive(RoomManager.Instance.activeRoom == this);

        if (RoomManager.Instance.activeRoom == this)
        {
            roomIndicator.color = activeColour;
        }
        else
        {
            roomIndicator.color = inactiveColour;
        }

    }
}
