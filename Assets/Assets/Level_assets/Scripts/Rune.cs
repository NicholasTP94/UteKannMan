using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Runes : MonoBehaviour
{
    SpriteRenderer thisSpriteRenderer;
    CircleCollider2D thisCircleCollider;
    public string triggerPlayer = "Player";
    public KeyCode activationButton = KeyCode.E;
    private bool insideCollider = false;


    void Start()
    {
        thisSpriteRenderer = GetComponent<SpriteRenderer>();
       
    }

    void Update()
    {
        if (Input.GetKeyDown(activationButton))
        {
            if (insideCollider)
            {
                gameObject.GetComponent<SpriteRenderer>().sprite = Resources.Load("T_BirchTwig_Lit", typeof(Sprite)) as Sprite;
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
}
