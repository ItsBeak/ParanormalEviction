using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SanityGauge : MonoBehaviour
{

    public Image fillImage;

    public Material happy;
    public Material flat;
    public Material sad;

    public MeshRenderer rend;

    public void SetFillAmount(float percentage)
    {
        fillImage.fillAmount = percentage / 100;

        if (percentage < 33)
        {
            rend.material = sad;
        }
        else if (percentage < 66)
        {
            rend.material = flat;
        }
        else
        {
            rend.material = happy;
        }

    }

}
