using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    public float speed = 3f; //Controls velocity multiplier
    Rigidbody rb; //Tells script there is a rigidbody, we can use variable rb to reference it in further script
    public bool CanMove;

    Vector3 moveDir;

    public Transform characterModel;

    void Start()
    {
        rb = GetComponent<Rigidbody>(); //rb equals the rigidbody on the player
    }

    void Update()
    {
        if (!CanMove)
        {
            moveDir.x = 0;
            moveDir.y = 0;
            rb.velocity = Vector3.zero;
            return;
        }   

        moveDir.x = Input.GetAxisRaw("Horizontal"); // d key changes value to 1, a key changes value to -1
        moveDir.z = Input.GetAxisRaw("Vertical"); // w key changes value to 1, s key changes value to -1
        moveDir = moveDir.normalized;

        if (Camera.main)
            rb.velocity = (moveDir.x * Camera.main.transform.right + moveDir.z * Camera.main.transform.forward) * speed;// Creates velocity in direction of value equal to keypress (WASD).

        if (rb.velocity != Vector3.zero && Time.timeScale != 0)
            characterModel.rotation = Quaternion.LookRotation(rb.velocity);

    }
}
