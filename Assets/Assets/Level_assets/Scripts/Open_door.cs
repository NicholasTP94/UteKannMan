using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Open_door : MonoBehaviour
{
    Animator thisAnimator;
    BoxCollider2D thisBoxCollider2D;

    //public GameObject firstDoor;
    //public GameObject secondDoor;
    //public GameObject thirdDoor;
    //public GameObject fourthDoor;
    
    //public int runesForOne = 3;
    //public int runesForTwo = 6;
    //public int runesForThree = 9;
    //public int runesForFour = 12;
    


    public void Start()
    {
        thisAnimator = GetComponent<Animator>();
        //thisAnimator.enabled = false;

        thisBoxCollider2D = GetComponent<BoxCollider2D>();
        GetComponent<Runes>();

        //firstDoor = GameObject.Find("door1");
        //secondDoor = GameObject.Find("door2");
        //thirdDoor = GameObject.Find("door3");
        //fourthDoor = GameObject.Find("door4");

    }

    //public void RuneActivated()
    //{
    //    numberOfRunes++;
    //    if (numberOfRunes == runesForOne)
    //    {
    //        DoorOpens(firstDoor);
    //    }

    //    if (numberOfRunes == runesForTwo)
    //    {
    //        DoorOpens(secondDoor);
    //    }

    //    if (numberOfRunes == runesForThree)
    //    {
    //        DoorOpens(thirdDoor);
    //    }

    //    if (numberOfRunes == runesForFour)
    //    {
    //        DoorOpens(fourthDoor);
    //    }
    //}

    void Update()
    {
        if (Runes.numberOfRunes == 3)
        {
            thisAnimator.SetTrigger("DoorOpens");
            thisBoxCollider2D.enabled = false;



        }
    }

    //private void DoorOpens(GameObject gameObject)
    //{
    //    thisAnimator.SetTrigger("DoorOpens");
    //    thisBoxCollider2D.enabled = false;
    //}



}



