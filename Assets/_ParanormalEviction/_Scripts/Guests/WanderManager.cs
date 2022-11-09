using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WanderManager : MonoBehaviour
{
    Transform[] rooms;
    Dictionary<Transform, GameObject> PointRequests;

    private void Awake()
    {
        PointRequests = new Dictionary<Transform, GameObject>();
        rooms = new Transform[transform.childCount];

        for (int i = 0; i < rooms.Length; i++)
        {
            rooms[i] = transform.GetChild(i);
        }
    }

    public int GetRoom()
    {
        int pickroom = Mathf.FloorToInt(Random.Range(0f, rooms.Length));
        return pickroom;
    }

    public Transform RandPointInRoom(int room, GameObject AI)
    {

        Transform Record = null;
        int pickpoint;
        foreach (var t in PointRequests)
        {
            if (t.Value == AI)
            {
                Record = t.Key;
                PointRequests.Remove(t.Key);
                break;
            }
        }

        GameObject OtherAI;
        do
        {
            pickpoint = Mathf.FloorToInt(Random.Range(0f, rooms[room].childCount));
            Record = rooms[room].GetChild(pickpoint);
        }
        while (PointRequests.TryGetValue(Record, out OtherAI) && OtherAI != null);
        PointRequests.Remove(Record);
        PointRequests.Add(Record, AI);
        return Record;
    }
}
