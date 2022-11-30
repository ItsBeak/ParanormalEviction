using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DoorCollision : MonoBehaviour
{

    public FloatEditor Test;

    public void Toggle()
    {
        gameObject.SetActive(!gameObject.activeSelf);
    }

    void OnTriggerStay(Collider other)
    {
        
        Debug.Log("An object is still inside of the trigger");
        

    }
}
