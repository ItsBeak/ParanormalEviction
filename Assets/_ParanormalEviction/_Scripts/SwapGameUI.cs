using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class SwapGameUI : MonoBehaviour
{
    public GameObject GameUI;
    public GameObject MobileUI;
    public GameObject Rooms;

    public Image timerFill;
    public Transform timerHand;
    public Text guestDisplay;
    // Start is called before the first frame update
#if UNITY_ANDROID
  void Start()
    {
        GameManager Access = FindObjectOfType<GameManager>();
        Access.timerHand = timerHand;
        Access.guestDisplay = guestDisplay;
        Access.timerFill = timerFill;
        MobileUI.SetActive(true);
        GameUI.SetActive(false);

        //Rooms.GetComponentsInChildren<Possesable> 
    }

#endif
}
