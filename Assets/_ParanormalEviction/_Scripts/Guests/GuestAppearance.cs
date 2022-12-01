using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuestAppearance : MonoBehaviour
{
    [Header("Character Materials")]
    public Material[] maleClothing;
    public Material[] femaleClothing;

    [Header("Character Base")]
    public SkinnedMeshRenderer baseRenderer;

    public GameObject maleClothes;
    public GameObject femaleClothes;

    [Header("Character Components - Male")]
    public SkinnedMeshRenderer belt;
    public SkinnedMeshRenderer hat;
    public SkinnedMeshRenderer malePants;
    public SkinnedMeshRenderer sleeveL;
    public SkinnedMeshRenderer sleeveR;

    [Header("Character Components - Female")]
    public SkinnedMeshRenderer hair;
    public SkinnedMeshRenderer femalePants;
    public SkinnedMeshRenderer skirt;
    public SkinnedMeshRenderer cuffs;

    [HideInInspector] public bool isMale;

    void Start()
    {
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
       
        int randomNumber = Random.Range(0, 2);

        if (randomNumber == 0)
        {
            baseRenderer.material = maleClothing[Random.Range(0, maleClothing.Length)];

            belt.material = maleClothing[Random.Range(0, maleClothing.Length)];
            hat.material = maleClothing[Random.Range(0, maleClothing.Length)];
            sleeveL.material = baseRenderer.material;
            sleeveR.material = baseRenderer.material;

            maleClothes.SetActive(true);
            femaleClothes.SetActive(false);

            isMale = true;

        }
        else
        {
            baseRenderer.material = femaleClothing[Random.Range(0, femaleClothing.Length)];

            hair.material = femaleClothing[Random.Range(0, femaleClothing.Length)];
            femalePants.material = femaleClothing[Random.Range(0, femaleClothing.Length)];
            skirt.material = femaleClothing[Random.Range(0, femaleClothing.Length)];
            cuffs.material = baseRenderer.material;

            maleClothes.SetActive(false);
            femaleClothes.SetActive(true);

            isMale = false;

        }
    }
}
