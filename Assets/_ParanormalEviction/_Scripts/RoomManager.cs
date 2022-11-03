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

    int nextRoomIndex;
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

    private void Update()
    {
        if (!isBusy)
        {
            if (Input.GetKeyDown(KeyCode.Alpha0))
            {
                ChangeRoom(0);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                ChangeRoom(1);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                ChangeRoom(2);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                ChangeRoom(3);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha4))
            {
                ChangeRoom(4);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha5))
            {
                ChangeRoom(5);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha6))
            {
                ChangeRoom(6);
            }
        }

    }

    public void ChangeRoom(int targetRoomIndex)
    {
        isBusy = true;
        CallFadeOut();
        nextRoomIndex = targetRoomIndex;
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

        //Temporary player teleport
        player.transform.position = activeRoom.roomCentre.position;

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
