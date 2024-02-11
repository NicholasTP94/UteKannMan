using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Open_door4 : MonoBehaviour
{
    Animator thisAnimator;
    BoxCollider2D thisBoxCollider2D;


    void Start()
    {
        thisAnimator = GetComponent<Animator>();
        

        thisBoxCollider2D = GetComponent<BoxCollider2D>();
        GetComponent<Runes>();

    }

    void Update()
    {
        if (Runes.numberOfRunes == 12)
        {
            thisBoxCollider2D.enabled = false;
            thisAnimator.SetTrigger("DoorOpens");
        }
    }
}
