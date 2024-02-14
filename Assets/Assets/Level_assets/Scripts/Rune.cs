using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMOD.Studio;


public class Runes : MonoBehaviour
{
    SpriteRenderer thisSpriteRenderer;
    CircleCollider2D thisCircleCollider;
    BoxCollider2D thisBoxCollider2D;

    Open_door open_Door;


    public KeyCode activationButton = KeyCode.E;
    public bool insideCollider;
    public static int numberOfRunes;




    void Start()
    {

        thisSpriteRenderer = GetComponent<SpriteRenderer>();
        thisCircleCollider = GetComponent<CircleCollider2D>();
        thisBoxCollider2D = GetComponent<BoxCollider2D>();
        open_Door = GetComponent<Open_door>();
        insideCollider = false;

    }

    void Update()
    {

        if (insideCollider && Input.GetKeyDown(activationButton))
        {
            AudioManager.instance.PlayOneShot(FMODEvents.instance.rune, this.transform.position);
            gameObject.GetComponent<SpriteRenderer>().sprite = Resources.Load("T_Harvest_Lit", typeof(Sprite)) as Sprite;

            numberOfRunes++;

            thisCircleCollider.enabled = false;
            insideCollider = false;
        }


    }




    private void OnTriggerEnter2D(Collider2D other)
    {

        if (other.CompareTag("Player"))
        {
            Debug.Log("Player has entered");

            insideCollider = true;






        }

    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            insideCollider = false;
            Debug.Log("Player has left");
        }
    }
}











    //public void FirstDoor()
    //{
    //    GameObject.Find("door1");
    //    Destroy(gameObject);

    //}

    //void SecondDoor()
    //{
    //    GameObject SecondDoor = GameObject.Find("Door (2)");
    //    thisBoxCollider2D.enabled = false;
    //}

    //void ThirdDoor()
    //{
    //    GameObject ThirdDoor = GameObject.Find("Door (3)");
    //    thisBoxCollider2D.enabled = false;
    //}

    //void FourthDoor()
    //{
    //    GameObject FourthDoor = GameObject.Find("Door (4)");
    //    thisBoxCollider2D.enabled = false;
    //}

