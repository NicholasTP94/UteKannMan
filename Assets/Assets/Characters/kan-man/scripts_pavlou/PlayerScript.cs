using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    [SerializeField] private float walkSpeed = 5f;
    private Animator animator;

    private float xAxis;
    private float yAxis;
    private Rigidbody rb2d;
    private bool isJumpPressed;
    private float jumpForce = 350;
    private int groundMask;
    private bool isGrounded;
    private string currentAnimation;
    private bool isAttackPressed;
    private bool isAttacking;

    [SerializeField]
    private float attackDelay = 0.3f;

    const string PLAYER_IDLE = "Player_idle";
    const string PLAYER_RUN = "Player_run";
    const string PLAYER_JUMP = "Player_jump";
    const string PLAYER_ATTACK = "Player_attack";
    const string PLAYER_AIR_ATTACK = "Player_air_attack";

    private void Start()
    {
        animator = GetComponent<Animator>();
        rb2d = GetComponent<Rigidbody>();
        groundMask = 1 << LayerMask.NameToLayer("Ground");
    }

    private void Update()
    {
        xAxis = Input.GetAxis("Horizontal");
        if (Input.GetKeyDown(KeyCode.Space))
        {
            isJumpPressed = true;
        }
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            isAttackPressed = true;
        }
    }

    private void FixedUpdate()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 0.1f, groundMask);

        if (hit.collider != null)
        {
            isGrounded = true;
        }
        else
        {
            isGrounded = false;
        }


        Vector2 vel = new Vector2(0,rb2d.velocity.y);
        if (xAxis < 0)
        {
            vel.x = -walkSpeed;
            transform.localScale = new Vector2(-1, 1);
        }
        else if (xAxis > 0)
        {
            vel.x = walkSpeed;
            transform.localScale = new Vector2(1, 1);
        }
        else
        {
            vel.x = 0;
        }

        if (isJumpPressed && isGrounded)
        {
            rb2d.AddForce(new Vector2(0, jumpForce));
            isJumpPressed = false;
        }
        rb2d.velocity = vel;

        if (isAttackPressed)
        {
            isAttackPressed = false;
            if(isAttacking)
            {
                isAttacking = true;
            }
        }
    }

    void AttackComplete()
    {

    }
}
