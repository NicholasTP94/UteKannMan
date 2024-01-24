using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.EventSystems.EventTrigger;

public class PlayerController2D : MonoBehaviour
{
    SpriteRenderer thisSpriteRenderer;
    Rigidbody2D thisRigidbody2D;
    Animator thisAnimator;
    CircleCollider2D thisCircleCollider2D;
    public GameObject attackPoint;
    public GameObject attackPointLeft;
    public GameObject attackPointRight;

    public float health = 100;
    public float maxHealth = 100f;

    [SerializeField] Image healthDisplay;

    float inpHor;
    float directionX;
    //bool isDead = false;
    bool isGrounded = false;
    int jumps = 0;

    public bool drankAtkSpdPotion = false;
    public float atkSpeed = 10; 
    [SerializeField] float speed = 5;
    [SerializeField] float jumpForce = 350;
    [SerializeField] float groundCheckDistance = 0.7f;
    public LayerMask Ground;

    void Start()
    {
        thisSpriteRenderer = GetComponent<SpriteRenderer>();
        thisRigidbody2D = GetComponent<Rigidbody2D>();
        thisAnimator = GetComponent<Animator>();
        thisCircleCollider2D = GetComponent<CircleCollider2D>();


    }

    void Update()
    {
        //isDead = deathCheck();
        isGrounded = groundCheck();
        inpHor = Input.GetAxis("Horizontal");
        directionX = inpHor * speed;

        if (isGrounded)
        {
            jumps = 0;
            if (Input.GetKeyDown(KeyCode.Space))
            {
                thisRigidbody2D.AddForce(Vector3.up * jumpForce);
                jumps++;
            }
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.Space) && jumps < 1)
            {
                thisRigidbody2D.AddForce(Vector3.up * jumpForce);
                thisAnimator.SetTrigger("DoubleJump");
                jumps++;
            }
        }

        if (inpHor < 0)
        {
            thisSpriteRenderer.flipX = true;
            attackPoint.transform.position = attackPointLeft.transform.position;
        }

        else if (inpHor > 0)
        {
            attackPoint.transform.position = attackPointRight.transform.position;

            thisSpriteRenderer.flipX = false;
        }

        thisAnimator.SetFloat("Speed", Mathf.Abs(inpHor));
        thisAnimator.SetBool("isGrounded", isGrounded);
    }

    private void FixedUpdate()
    {
        thisRigidbody2D.velocity = new Vector2(directionX, thisRigidbody2D.velocity.y);
    }

    //bool deathCheck()
    //{
    //    if (health <= 0)
    //    {
    //        return true;
    //        thisAnimator.SetTrigger("Death");
    //    }
    //    else return false;
    //}
    public bool groundCheck()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, groundCheckDistance, Ground);

        if (hit.collider != null)
        {
            jumps = 0;
            return true;
        }
        else return false;

        //OtherWay
        //RaycastHit2D[] hit = Physics2D.RaycastAll(transform.position, Vector2.down, groundCheckDistance, Ground);

        //if (hit[0].collider != null)
        //{
        //    jumps = 0;
        //    return true;
        //}

        //else
        //    Debug.Log(hit[0].collider.name);
        //    return false;
    }
}
