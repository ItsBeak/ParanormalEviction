using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShaderDistance : MonoBehaviour
{

    Material mat;
    MeshRenderer rend;

    Transform player;

    public Vector3 playerPos;
    public Vector3 objectPos;

    void Start()
    {
        rend = GetComponent<MeshRenderer>();
        mat = Instantiate(rend.material);
        rend.material = mat;
        player = GameObject.Find("Player").transform;
    }

    void Update()
    {

        if (rend == null)
        {
            Debug.LogError("No renderer found on " + gameObject.name + ".");
            return;
        }

        playerPos = player.position;
        playerPos.y = 0;

        objectPos = transform.position;
        objectPos.y = 0;

        rend.material.SetFloat("PlayerToObjectDistance", Vector3.Distance(playerPos, objectPos));


    }
}
