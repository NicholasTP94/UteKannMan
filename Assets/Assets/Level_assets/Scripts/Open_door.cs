using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Open_door : MonoBehaviour
{
    Animator thisAnimator;
    BoxCollider2D thisBoxCollider2D;

    


    void Start()
    {
        thisAnimator = GetComponent<Animator>();
        //thisAnimator.enabled = false;

        thisBoxCollider2D = GetComponent<BoxCollider2D>();
        GetComponent<Runes>();

    }

    void Update()
    {
        if (Runes.numberOfRunes == 3)
        {
            thisAnimator.SetTrigger("DoorOpens");
            thisBoxCollider2D.enabled = false;
            
        }
    }


}



//if (thisRunes.openDoor1)
//{
//    FirstDoor();
//}

//else
//{
//    door1IsOpen = false;
//}

//if (thisRunes.numberOfRunes == openDoor2)
//{
//    door2IsOpen = true;
//}

//if (thisRunes.numberOfRunes == openDoor3)
//{
//    door3IsOpen = true;
//}

//if (thisRunes.numberOfRunes == openDoor4)
//{
//    door4IsOpen = true;
//}

//if (door1IsOpen)
//{

//    FirstDoor();
//    //Open door 1 calling FirstDoor()
//    //Play opening sound
//    //Maybe UI reference?
//}

//if (door2IsOpen)
//{
//    //Open door 2 calling SecondDoor()
//    //Play opening sound
//    //Maybe UI reference?
//}

//if (door3IsOpen)
//{
//    //Open door 3 calling ThirdDoor()
//    //Play opening sound
//    //Maybe UI reference?
//}

//if (door4IsOpen)
//{
//    //Open door 4 calling FourthDoor()
//}
