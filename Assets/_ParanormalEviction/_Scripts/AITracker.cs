using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AITracker : MonoBehaviour
{
    public int WinCount = 0;
    public GameObject GuestPrefab;
    public Transform Entrance;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
     
    }

    public void IncreaseWinCount(GameObject Guest)
    {
        WinCount++;
        Destroy(Guest);
        Instantiate(GuestPrefab, Entrance);
        // create new guest as child of the object this script is attached to at the exit
        // or discuss with developers if performing a reset and dumping the guest back in is better.
    }
}
