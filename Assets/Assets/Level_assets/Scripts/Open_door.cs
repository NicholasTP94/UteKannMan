using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Open_door : MonoBehaviour
{
    Animator thisAnimator;
    BoxCollider2D thisBoxCollider2D;
    public Runes Runes;

    private void Start()
    {
        thisAnimator = GetComponent<Animator>();
        
        thisBoxCollider2D.enabled = true;

    }

    void Update()
    {
        if (Runes.door1IsOpen == true)
        {
            //Open door 1 calling FirstDoor()
            //Play opening sound
            //Maybe UI reference?
        }

        if (Runes.door2IsOpen == true)
        {
            //Open door 2 calling SecondDoor()
            //Play opening sound
            //Maybe UI reference?
        }

        if (Runes.door3IsOpen == true)
        {
            //Open door 3 calling ThirdDoor()
            //Play opening sound
            //Maybe UI reference?
        }

        if (Runes.door4IsOpen == true)
        {
            //Open door 4 calling FourthDoor()
        }
    }

    public void FirstDoor()
    {
        GameObject FirstDoor = GameObject.Find("door1");
        thisBoxCollider2D.enabled = false;
        
    }

    public void SecondDoor()
    {
        GameObject SecondDoor = GameObject.Find("door2");
        thisBoxCollider2D.enabled = false;
    }

    public void ThirdDoor()
    {
        GameObject ThirdDoor = GameObject.Find("door3");
        thisBoxCollider2D.enabled = false;
    }

    public void FourthDoor()
    {
        GameObject FourthDoor = GameObject.Find("door4");
        thisBoxCollider2D.enabled = false;
    }
}
