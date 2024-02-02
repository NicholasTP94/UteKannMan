using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using FMOD.Studio;
using static UnityEngine.EventSystems.EventTrigger;

public class PlayerController2D : MonoBehaviour
{
    SpriteRenderer thisSpriteRenderer;
    Rigidbody2D thisRigidbody2D;
    Animator thisAnimator;
    CircleCollider2D thisCircleCollider2D;

    private EventInstance playerFootsteps;
    
    public GameObject attackPointLeft;
    public GameObject attackPointRight;

    public GameObject attackPointLeftHeavy;
    public GameObject attackPointRightHeavy;

    public float health = 100;
    public float maxHealth = 100f;

    [SerializeField] Image healthDisplay;

    float inpHor;
    float directionX;

    bool isDead = false;
    bool isGrounded = false;
    bool doubleJump = false;

    public bool drankAtkSpdPotion = false;
    public float atkSpeed = 10;
    [SerializeField] float speed = 4;
    [SerializeField] float jumpForce = 10;
    [SerializeField] float groundCheckDistance = 0.7f;
    public LayerMask Ground;

    //Attack Script Integrated
    public Transform attackPoint;
    public Transform attackPointHeavy;

    public Transform leftLeg;
    public Transform rightLeg;

    public float attackRange = 1f;
    public int lightAttackDamage = 20;
    public int heavyAttackDamage = 40;
    public float lightAttackCooldown = 1f;
    public float heavyAttackCooldown = 3f;
    private float lastLightAttackTimestamp = 0f;
    private float lastHeavyAttackTimestamp = 0f;
    public float heavyAttackThreshold = 0.5f;
    public float attackTriggerTimeStamp;
    public bool heavyAttacked = false;

    float lastHit = 0; // for the hit method //
    public bool enemyHit = false; // Checks if the enemy is hit

    void Start()
    {
        playerFootsteps = AudioManager.instance.CreateEventInstance(FMODEvents.instance.playerFootsteps);
        thisSpriteRenderer = GetComponent<SpriteRenderer>();
        thisRigidbody2D = GetComponent<Rigidbody2D>();
        thisAnimator = GetComponent<Animator>();
        thisCircleCollider2D = GetComponent<CircleCollider2D>();
    }

    void Update()
    {
        isDead = deathCheck();
        isGrounded = groundCheck();
        inpHor = Input.GetAxis("Horizontal");

        directionX = inpHor * speed;

        if (Input.GetMouseButtonDown(0))
        {
            attackTriggerTimeStamp = Time.time;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            if (!heavyAttacked)
            {
                LightAttack();
            }
            heavyAttacked = false;
        }

        if (Input.GetMouseButton(0))
        {
            if (!heavyAttacked)
            {
                checkForHeavyAttack();
            }
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (isGrounded)
            {
                AudioManager.instance.PlayOneShot(FMODEvents.instance.jump, this.transform.position);
                thisRigidbody2D.velocity = new Vector2(thisRigidbody2D.velocity.x, jumpForce);
                doubleJump = true;
            }
            else if (doubleJump)
            {
                AudioManager.instance.PlayOneShot(FMODEvents.instance.jump, this.transform.position);
                thisRigidbody2D.velocity = new Vector2(thisRigidbody2D.velocity.x, jumpForce);
                doubleJump = false;

            }
        }

        if (inpHor < 0)
        {
            thisSpriteRenderer.flipX = true;
            attackPoint.transform.position = attackPointLeft.transform.position;
            attackPointHeavy.transform.position = attackPointLeftHeavy.transform.position;

        }

        else if (inpHor > 0)
        {
            thisSpriteRenderer.flipX = false;
            attackPoint.transform.position = attackPointRight.transform.position;
            attackPointHeavy.transform.position = attackPointRightHeavy.transform.position;
        }
        thisAnimator.SetFloat("yVelocity", thisRigidbody2D.velocity.y);
        thisAnimator.SetFloat("Speed", Mathf.Abs(inpHor));
        thisAnimator.SetBool("isGrounded", isGrounded);
    }

    private void FixedUpdate()
    {
        thisRigidbody2D.velocity = new Vector2(directionX, thisRigidbody2D.velocity.y);
        UpdateSound();
    }

   

    //Part of the attack script
    void checkForHeavyAttack()
    {
        float keyDownTime = Time.time - attackTriggerTimeStamp;
        Debug.Log("KeyDown Time: " + keyDownTime);


        if (keyDownTime >= heavyAttackThreshold)
        {
            HeavyAttack();
        }

    }
    void LightAttack()
    {
        if (lastLightAttackTimestamp == 0 || (Time.time - lastLightAttackTimestamp >= lightAttackCooldown))
        {
            Debug.Log("Quick Slash!");
            thisAnimator.SetTrigger("Attack");

            AudioManager.instance.PlayOneShot(FMODEvents.instance.lightAttack, this.transform.position);

            Collider2D[] enemies = Physics2D.OverlapBoxAll(attackPoint.position, new Vector2(1.8f, 1.2f), 0f);

            foreach (Collider2D collider in enemies)
            {
                if (collider.CompareTag("Enemy"))
                {
                   // collider.GetComponent<Enemies>().TakeDamage(lightAttackDamage);
                    Debug.Log("We hit " + collider.name + " for " + lightAttackDamage + " damage.");
                    collider.GetComponent<Enemy_Ghoul>().TakeDamage(lightAttackDamage); // Hits Enemy
                    enemyHit = true;
                }
            }
            lastLightAttackTimestamp = Time.time;
        }
        else
        {
            Debug.Log("Can't attack yet.");
        }
    }
    void HeavyAttack()
    {
        if (lastHeavyAttackTimestamp == 0 || (Time.time - lastHeavyAttackTimestamp >= heavyAttackCooldown))
        {
            heavyAttacked = true;
            attackTriggerTimeStamp = 0;

            Debug.Log("Heavy Slash!");
            thisAnimator.SetTrigger("HeavyAttack");

            AudioManager.instance.PlayOneShot(FMODEvents.instance.heavyAttack, this.transform.position);

            Collider2D[] enemies = Physics2D.OverlapBoxAll(attackPointHeavy.position, new(2.1f, 2.4f), 0f);

            foreach (Collider2D collider in enemies)
            {
                if (collider.CompareTag("Enemy"))
                {
                    //collider.GetComponent<Enemies>().TakeDamage(heavyAttackDamage);
                    Debug.Log("We hit " + collider.name + " for " + heavyAttackDamage + " damage.");
                    collider.GetComponent<Enemy_Ghoul>().TakeDamage(heavyAttackDamage); // Hits enemy
                    enemyHit = true;
                }
            }
            lastHeavyAttackTimestamp = Time.time;
        }
        else
        {
            Debug.Log("Can't attack yet.");
        }
    }
    private void OnDrawGizmos()
    {
        //light attack tinkering
        Gizmos.DrawWireCube(attackPoint.position, new Vector3(1.8f, 1.2f, 0f));
        //heavy attack tinkering
        Gizmos.DrawWireCube(attackPointHeavy.position, new Vector3(2.1f, 2.4f, 0f));
    }

    bool groundCheck()
    {
        Debug.DrawRay(leftLeg.position, Vector2.down, Color.cyan);
        RaycastHit2D leftLegHit = Physics2D.Raycast(leftLeg.position, Vector2.down, groundCheckDistance, Ground);

        Debug.DrawRay(rightLeg.position, Vector2.down, Color.cyan);
        RaycastHit2D rightLegHit = Physics2D.Raycast(rightLeg.position, Vector2.down, groundCheckDistance, Ground);

        if (leftLegHit.collider == null && rightLegHit.collider == null)
        {
            return false; // Character is not grounded
        }
        return true;
    }
    bool deathCheck()
    {
        if (health <= 0)
        {
            thisAnimator.SetTrigger("Dead");
            return true;
        }
        else return false;
    }
    private void UpdateSound()
    {
        if(thisRigidbody2D.velocity.x != 0 && isGrounded)
        {
            PLAYBACK_STATE playbackState;
            playerFootsteps.getPlaybackState(out playbackState);
            if (playbackState.Equals(PLAYBACK_STATE.STOPPED))
            {
                playerFootsteps.start();
            }
        }
        else
        {
            playerFootsteps.stop(STOP_MODE.ALLOWFADEOUT);
        }
    }

    public void Damage(float _damage) // Hit method //
    {
        if (Time.time > lastHit + 0.1f)
        {
            health -= _damage;
            lastHit = Time.time;
        }
    }
}
