using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{

    PlayerMovement move;

    Animator anim;

    float blend;

    void Start()
    {
        move = GetComponentInParent<PlayerMovement>();
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        if (move.rb.velocity != Vector3.zero)
        {
            if (blend < 1)
            {
                blend += 4 * Time.deltaTime;
            }
        }
        else
        {
            if (blend > 0)
            {
                blend -= 4 * Time.deltaTime;
            }
        }

        anim.SetFloat("MoveBlend", blend);
    }
}
