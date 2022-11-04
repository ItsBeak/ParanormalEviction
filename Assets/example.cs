using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class example : MonoBehaviour
{
    public UnityEvent events;

    void Start()
    {
        
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            events.Invoke();
        }

        
    }
}
