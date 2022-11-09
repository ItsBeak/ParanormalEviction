using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuestAppearance : MonoBehaviour
{

    public Material[] skinMaterials;
    public Material[] clothingMaterials;

    SkinnedMeshRenderer rend;

    void Start()
    {
        rend = GetComponentInChildren<SkinnedMeshRenderer>();
        RandomizeAppearance();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            RandomizeAppearance();
        }
    }

    public void RandomizeAppearance()
    {
        Material[] newMats = new Material[2];

        newMats[0] = skinMaterials[Random.Range(0, skinMaterials.Length)];
        newMats[1] = clothingMaterials[Random.Range(0, clothingMaterials.Length)];

        rend.materials = newMats;
    }
}
