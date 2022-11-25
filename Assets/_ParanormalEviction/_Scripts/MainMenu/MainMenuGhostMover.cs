using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuGhostMover : MonoBehaviour
{

    public Transform ghostModel;

    public Transform[] positions;

    void Start()
    {
        InvokeRepeating("MoveGhost", 7f, 8f);
    }

    public void MoveGhost()
    {
        int rand = Random.Range(0, positions.Length);

        ghostModel.position = positions[rand].position;
        ghostModel.rotation = positions[rand].rotation;

    }

}
