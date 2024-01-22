using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemies : MonoBehaviour
{
    public int maxHealth = 100;
    private int currentHealth;
    void Start()
    {
        currentHealth = maxHealth;
    }
    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        Debug.Log("Enemy health remaining " + currentHealth.ToString() + ".");
        if (currentHealth < 0)
        {
            Die();
        }
        

    }
    void Die()
    {
        //Death animation
        Destroy(this.gameObject, 0.5f);
        Debug.Log("Enemy died!");
    }
    
}
