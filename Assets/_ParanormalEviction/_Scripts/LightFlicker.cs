using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightFlicker : MonoBehaviour
{

    public float minIntensity, maxIntensity;

    float target;

    public float lerpSpeed;

    Light l;

    void Start()
    {
        l = GetComponent<Light>();
        InvokeRepeating("GetTarget", 0, 0.2f);
    }

    void Update()
    {
        l.intensity = Mathf.Lerp(l.intensity, target, lerpSpeed * Time.deltaTime);
    }

    void GetTarget()
    {
        target = Random.Range(minIntensity, maxIntensity);
    }
}
