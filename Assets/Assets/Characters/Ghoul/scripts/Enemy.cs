using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float health { get; set; }                           // Health.
    public float movementSpeed;                                 // movement speed
    public float attackDamage;                                  // attack damage
    public float attackRange;                                   // attack rannge
    public float attackSpeed;                                   // attack speed

    private Rigidbody2D rb;
    private Animator animator;                                  // Animator of enemy

    protected bool isAlive;                                     // check if enemy is alive
    protected bool isAwake;                                     // chack if enemy is awake (active when enemy enters the sphere cast)

}