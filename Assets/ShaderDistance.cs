using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShaderDistance : MonoBehaviour
{

    Material mat;
    MeshRenderer rend;

    Transform player;

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

        rend.material.SetFloat("PlayerPositionX", player.position.x);
        rend.material.SetFloat("PlayerPositionZ", player.position.z);
        rend.material.SetFloat("ObjectPositionX", transform.position.x);
        rend.material.SetFloat("ObjectPositionZ", transform.position.z);
    }
}
