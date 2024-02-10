using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Runes : MonoBehaviour
{
    //SpriteRenderer thisSpriteRenderer;
    //CircleCollider2D thisCircleCollider;
    public string triggerPlayer = "Player";
    public KeyCode activationButton = KeyCode.E;
    private bool insideCollider = false;
    public bool door1IsOpen = false;
    public bool door2IsOpen = false;
    public bool door3IsOpen = false;
    public bool door4IsOpen = false;
    int numberOfRunes;
    int openDoor1 = 3;
    int openDoor2 = 6;
    int openDoor3 = 9;
    int openDoor4 = 12;


    void Start()
    {
        //thisSpriteRenderer = GetComponent<SpriteRenderer>();
       
    }

    void Update()
    {
        if (Input.GetKeyDown(activationButton))
        {
            if (insideCollider)
            {
                gameObject.GetComponent<SpriteRenderer>().sprite = Resources.Load("T_Harvest_Lit", typeof(Sprite)) as Sprite;
                numberOfRunes++;
            }
        }
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(triggerPlayer))
        {
            insideCollider = true;
        }

    }

    void OpenDoor()
    {
        if (numberOfRunes == openDoor1)
        {
            door1IsOpen=true;
        }

        if (numberOfRunes == openDoor2)
        {
            door2IsOpen=true;
        }

        if(numberOfRunes == openDoor3)
        {
            door3IsOpen=true;
        }

        if (numberOfRunes == openDoor4)
        {
            door4IsOpen = true;
        }
    }
}
