using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//SCRIPTAKI POU ELEGXEI TO ATK SPEED BOOST OTAN O PLAYER
//PINEI ATK SPEED POTION
//XRHSIMOPOIOUME COROUTINE WSTE TO BUFF NA FEYGEI META APO 5 SECONDS
//MENEI NA TA KANW NA MH STACKAROUN ALLA PARAEIMAI KOMMATIA GIA NA BGEI TWRA

public class AtkSpdCheck : MonoBehaviour
{



    private float duration = 5f;
    private bool isTriggered = false;

    void Start()
    {

    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (!isTriggered)
        {
            PlayerController2D player = other.GetComponent<PlayerController2D>();

            if (player != null)

            {
                gameObject.GetComponent<Renderer>().enabled = false;
                StartCoroutine(DoubleAttackSpeed(player));
                Debug.Log("Player touched the Attack Speed Potion!");
                isTriggered = true;

            }
        }



    }
    private IEnumerator DoubleAttackSpeed(PlayerController2D player)
    {
        player.drankAtkSpdPotion = true;

        player.atkSpeed *= 2;

        yield return new WaitForSeconds(duration);

        player.atkSpeed /= 2;
        player.drankAtkSpdPotion = false;
        Destroy(gameObject);
    }

}




