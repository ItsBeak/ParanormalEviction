using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRotation : MonoBehaviour
{

    public Transform target;
    public float speed;

    PlayerMovement move;

    private void Start()
    {
        move = GetComponentInParent<PlayerMovement>();
    }

    void Update()
    {
        if (move.rb.velocity != Vector3.zero && Time.timeScale != 0)
        {
            Vector3 targetDirection = target.position - transform.position;

            float step = speed * Time.deltaTime;

            Vector3 newDirection = Vector3.RotateTowards(transform.forward, targetDirection, step, 0.0f);

            Debug.DrawRay(transform.position, newDirection, Color.red);

            transform.rotation = Quaternion.LookRotation(newDirection);
        }

    }
}
