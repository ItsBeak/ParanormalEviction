using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AITracker : MonoBehaviour
{
    public int WinCount = 0;
    public GameObject GuestPrefab;
    public Transform Entrance;

    // This code code be batched up with either the Sanity or Wander mangagers rather easily so look into that later or don't.

    /// <summary>
    /// Increase score
    /// create new guest
    /// </summary>
    /// <param name="Guest"></param>
    public void IncreaseWinCount(GameObject Guest)
    {
        WinCount++;
        Invoke("SpawnGuest", 5);
    }

    private void SpawnGuest()
    {
        GameObject newGuest = Instantiate(GuestPrefab, Entrance.position, Quaternion.identity, this.transform);
        Debug.Log("Spwaning new guest!");
    }
}