using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SanityGauge : MonoBehaviour
{

    public Image fillImage;

    public void SetFillAmount(float percentage)
    {
        fillImage.fillAmount = percentage / 100;
    }

}
