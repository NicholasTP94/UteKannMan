using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Ghoul : MonoBehaviour
{
    SpriteRenderer thisSpriteRenderer;
    Rigidbody2D thisRigidbody2D;
    Animator thisAnimator;
    BoxCollider2D thisCollider2D;

    // Ghoul_Heralth
    [Range(0, 60)] public float health = 60;
    float lastHealth = 60;
    bool isAlive = true;

    bool attackingPlayer = false;

    float damage;

    float directionX = 1;

    public Vector3 startPosition;
    public Vector3 endPosition;

    public float patrolDistance = 3;

    // Enemy States
    public enum EnemyState
    {
        Patrol,
        Hit,
        Chase,
        Attack,
        Dead
    }

    public EnemyState enemyState;

    public Transform player;

    float distanceToPlayer;
    public float chaseDistance = 10f;

    // Attack stats
    public float attackRange = 1.5f;
    public float attackDelay = 1f; // in seconds
    float lastAttack = 0;

    float lastHit = 0;

    // Start is called before the first frame update
    void Start()
    {
        thisSpriteRenderer = GetComponent<SpriteRenderer>();
        thisRigidbody2D = GetComponent<Rigidbody2D>();
        thisAnimator = GetComponent<Animator>();
        thisCollider2D = GetComponent<BoxCollider2D>();

        //thisAnimator.SetBool("isGrounded", true);

        startPosition = transform.position;
        endPosition = transform.position + Vector3.right * patrolDistance;

        enemyState = EnemyState.Patrol;
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(player.GetComponent<PlayerController2D>().enemyHit);
        distanceToPlayer = Vector2.Distance(transform.position, player.transform.position);

        if (Vector2.Distance(transform.position, player.transform.position) <= attackRange)
        {
            Debug.DrawRay(transform.position, player.transform.position - transform.position, Color.red);
        }
        else if (Vector2.Distance(transform.position, player.transform.position) <= chaseDistance)
        {
            Debug.DrawRay(transform.position, player.transform.position - transform.position, Color.magenta);
        }
        else
        {
            Debug.DrawRay(transform.position, player.transform.position - transform.position, Color.green);
        }


        if (isAlive)
        {
            Logic();

            switch (enemyState)
            {
                case EnemyState.Patrol:
                    Patrol();
                    break;
                case EnemyState.Hit:
                    Hit();
                    break;
                case EnemyState.Chase:
                    Chase();
                    break;
                case EnemyState.Attack:
                    Attack();
                    break;
                case EnemyState.Dead:
                    Dead();
                    break;
                default:
                    break;
            }
        }
    }

    void Logic()
    {
        if (health <= 0)
        {
            enemyState = EnemyState.Dead;
            return;
        }
        else if (player.GetComponent<PlayerController2D>().enemyHit)
        {
            enemyState = EnemyState.Hit;
            player.GetComponent<PlayerController2D>().enemyHit = false;
            return;
        }
        

        if (player != null)
        {
            distanceToPlayer = Vector3.Distance(transform.position, player.position);

            if (distanceToPlayer <= attackRange)
            {
                enemyState = EnemyState.Attack;
            }
            else if (distanceToPlayer > chaseDistance)
            {
                enemyState = EnemyState.Patrol;
            }
            else
            {
                enemyState = EnemyState.Chase;
            }
        }
    }
    #region StateMethods

    void Patrol()
    {
        attackingPlayer = false;
        thisAnimator.SetBool("attackingPlayer", attackingPlayer);

        if (directionX > 0)
        {
            if (transform.position.x >= endPosition.x)
            {
                directionX = -1;
            }
        }
        else if (directionX < 0)
        {
            if (transform.position.x <= startPosition.x)
            {
                directionX = 1;
            }
        }

        if (directionX < 0) thisSpriteRenderer.flipX = true;
        else if (directionX > 0) thisSpriteRenderer.flipX = false;

        thisAnimator.SetFloat("GhoulSpeed", Mathf.Abs(directionX));
    }

    //Takes Damage from player
    public void TakeDamage(float _damage) 
    {
        if (Time.time > lastHit + 0.1f)
        {
            health -= _damage;
            lastHit = Time.time;
        }
    }

    // Plays the hit animation
    public void Hit()
    {
        attackingPlayer = false;
        directionX = 0;
        thisAnimator.SetBool("attackingPlayer", attackingPlayer);

        if (!isAnimationState("GhoulHit"))
            thisAnimator.SetTrigger("GhoulHit");
    }

    void Chase()
    {
        attackingPlayer = false;
        thisAnimator.SetBool("attackingPlayer", attackingPlayer);

        if (transform.position.x > player.position.x)
        {
            directionX = -1;
        }
        else if (transform.position.x < player.position.x)
        {
            directionX = 1;
        }

        // To fix twitching when alignd on X axis but not on Y
        float distX = transform.position.x - player.position.x;

        if (Mathf.Abs(distX) < 0.1f)
        {
            directionX = 0;
        }

        if (directionX < 0) thisSpriteRenderer.flipX = true;
        else if (directionX > 0) thisSpriteRenderer.flipX = false;
    }

    void Attack()
    {
        attackingPlayer = true;
        thisAnimator.SetBool("attackingPlayer", attackingPlayer);

        // Determine direction to player
        if (transform.position.x > player.position.x)
        {
            directionX = -1;
        }
        else if (transform.position.x < player.position.x)
        {
            directionX = 1;
        }

        // Make sure we look at player
        if (directionX < 0) thisSpriteRenderer.flipX = true;
        else if (directionX > 0) thisSpriteRenderer.flipX = false;

        // Enemy won't move
        directionX = 0;

        //Stop the animation of Motion state
        thisAnimator.SetFloat("GhoulSpeed", 0);

        if (Time.time >= lastAttack)
        {
            thisAnimator.SetTrigger("GhoulAttack");
            lastAttack = Time.time + attackDelay;
            player.GetComponent<PlayerController2D>().Damage(20);
        }
    }

    void Dead()
    {
        attackingPlayer = false;
        directionX = 0;
        thisAnimator.SetBool("attackingPlayer", attackingPlayer);
        // Will only run once
        //thisRigidbody2D.velocity = new Vector2(thisRigidbody2D.velocity.x, 5);
        thisAnimator.SetTrigger("GhoulDeath");
        //thisCollider2D.isTrigger = true;
        Destroy(gameObject, 1.2f);
        return;
    }
    #endregion

    private void FixedUpdate()
    {

        if (isAlive)
        {
            thisRigidbody2D.velocity = new Vector2(directionX, thisRigidbody2D.velocity.y);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // So the enemy can't see us from above
            if (collision.transform.position.y >= transform.position.y - 0.1f)
            {
                player = collision.transform;
            }
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // So the enemy can't see us from above
            if (collision.transform.position.y >= transform.position.y - 0.1f)
            {
                player = collision.transform;
            }
        }
    }
    bool isAnimationState(string stateName)
    {
        if (thisAnimator.GetCurrentAnimatorStateInfo(0).IsName(stateName))
        { return true; }
        else return false;
    }
}