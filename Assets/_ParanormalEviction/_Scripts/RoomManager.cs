using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoomManager : MonoBehaviour
{
    public static RoomManager Instance { get; private set;}


    [Header("Player")]
    public GameObject player;

    [Header("UI")]
    public Image black;

    [Header("Rooms")]
    public Room activeRoom;
    public Room[] rooms;

    public Text roomNameReadout;

    int nextRoomIndex;
    int targetTeleportLocationID;
    bool isBusy;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    public void ChangeRoom(Room targetRoom, int teleportID)
    {
        isBusy = true;
        CallFadeOut();

        for (int i = 0; i < rooms.Length; i++)
        {
            if (targetRoom == rooms[i])
            {
                nextRoomIndex = i;
                targetTeleportLocationID = teleportID;
                break;
            }
        }
    }

    public void CallFadeOut()
    {
        StartCoroutine(FadeOut());
    }

    public IEnumerator FadeOut(float speed = 3f)
    {

        Color fadeColour = Color.clear;
        float fadeAmount;

        while (black.color.a < 1)
        {
            fadeAmount = fadeColour.a + (speed * Time.deltaTime);

            fadeColour = new Color(fadeColour.r, fadeColour.g, fadeColour.b, fadeAmount);
            black.color = fadeColour;
            yield return null;
        }

        CallFadeIn();
    }

    public void CallFadeIn()
    {
        StartCoroutine(FadeIn());

        activeRoom = rooms[nextRoomIndex];
        roomNameReadout.text = rooms[nextRoomIndex].roomName;

        player.transform.position = activeRoom.teleportPoints[targetTeleportLocationID].position;

    }

    public IEnumerator FadeIn(float speed = 3f)
    {

        Color fadeColour = Color.black;
        float fadeAmount;

        while (black.color.a > 0)
        {
            fadeAmount = fadeColour.a - (speed * Time.deltaTime);

            fadeColour = new Color(fadeColour.r, fadeColour.g, fadeColour.b, fadeAmount);
            black.color = fadeColour;
            yield return null;
        }

        isBusy = false;

    }

}
