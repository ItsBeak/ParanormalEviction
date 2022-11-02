using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DoorCollision : MonoBehaviour
{

    private void OnTriggerStay(Collider other)
    {
        Debug.Log("An object is still inside of the trigger");
        gameObject.SetActive(!gameObject.activeSelf);

    }


    void Start()
    {
        
    }

    
    void Update()
    {
        
    }
}
