using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScareRadiusIndicator : MonoBehaviour
{

    public GameObject particle;
    public ParticleSystem system;
    public Possesion playerPossession;

    void Start()
    {
        playerPossession = GameObject.Find("Player").GetComponent<Possesion>();
    }

    void Update()
    {
        if (playerPossession.Active)
        {
            transform.position = new Vector3(playerPossession.Active.gameObject.transform.position.x, 0, playerPossession.Active.gameObject.transform.position.z);

            var shape = system.shape;
            shape.scale = new Vector3(playerPossession.Active.scareRadius * 0.4f, 1, playerPossession.Active.scareRadius * 0.4f);
        }
        else
        {
            transform.position = Vector3.zero;

            var shape = system.shape;
            shape.scale = new Vector3(1,1,1);
        }

        particle.transform.Rotate(Vector3.up, 45f * Time.deltaTime);  
    }
}
