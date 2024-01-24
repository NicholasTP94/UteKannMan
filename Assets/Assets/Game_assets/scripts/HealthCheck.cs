using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//OBJECT NAME -> Health Potion
//HealthCheck script -> Checks if the player entered the health potion's collider,
//heals the player and destroys the potion


public class HealthCheck : MonoBehaviour
{
    public int healthRestore = 30;

    void Start()
    {
        
    }
    void OnTriggerEnter2D(Collider2D other)
    {

        if (other.gameObject.CompareTag("Player"))
        {
            PlayerController2D player = other.GetComponent<PlayerController2D>();
            if (player != null && player.health < player.maxHealth)
            {
                player.health = Mathf.Min(player.health + healthRestore, player.maxHealth);
                Debug.Log("Player touched the Health Potion!");
                Destroy(gameObject);
            }
            

            else 
            {
                Debug.Log("Health is already full!");
            }
        }

        
        


    }
    
}
