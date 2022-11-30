using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    public float speed = 3f; //Controls velocity multiplier
    [HideInInspector] public Rigidbody rb; //Tells script there is a rigidbody, we can use variable rb to reference it in further script
    public bool CanMove;

    [HideInInspector] Vector3 moveDir;

    public Transform characterModel;

    void Start()
    {
        rb = GetComponent<Rigidbody>(); //rb equals the rigidbody on the player
    }

    void Update()
    {
        if (!CanMove)
        {
            rb.velocity = Vector3.zero;
            return;
        }
        Vector3 ForwardDir = Vector3.zero;
#if !UNITY_ANDROID
        moveDir.x = Input.GetAxisRaw("Horizontal"); // d key changes value to 1, a key changes value to -1
        moveDir.z = Input.GetAxisRaw("Vertical"); // w key changes value to 1, s key changes value to -1
#endif
        ForwardDir = moveDir;
        ForwardDir.Normalize();

        if (Camera.main)
            rb.velocity = (ForwardDir.x * Camera.main.transform.right + ForwardDir.z * Camera.main.transform.forward) * speed;// Creates velocity in direction of value equal to keypress (WASD).

        if (rb.velocity != Vector3.zero && Time.timeScale != 0)
            characterModel.rotation = Quaternion.LookRotation(rb.velocity);
            

    }

    public void MoveUp()
    {
        moveDir.z += 1;
    }

    public void MoveLeft()
    {
        moveDir.x -= 1; 
    }

    public void MoveDown()
    {
        moveDir.z -= 1;
    }

    public void MoveRight()
    {
        moveDir.x += 1;
    }
}
