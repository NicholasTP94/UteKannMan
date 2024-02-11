using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Boss : MonoBehaviour
{
    SpriteRenderer thisSpriteRenderer;
    Rigidbody2D thisRigidbody2D;
    Animator thisAnimator;
    BoxCollider2D thisCollider2D;

    // Boss_Heralth
    [Range(0, 240)] public float health = 240;
    bool isAlive = true;

    bool AttackPlayer = false;

    float damage;

    float directionX = 1;


    // Enemy States
    public enum EnemyState
    {
        Idle,
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

        Physics2D.IgnoreLayerCollision(3, 8);

        enemyState = EnemyState.Idle;
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(player.GetComponent<PlayerController2D>().enemyHit);
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
                case EnemyState.Idle:
                    Idle();
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
                enemyState = EnemyState.Idle;
            }
            else
            {
                enemyState = EnemyState.Chase;
            }
        }
    }
    #region StateMethods

    void Idle()
    {
        AttackPlayer = false;
        thisAnimator.SetBool("AttackPlayer", AttackPlayer);
        thisAnimator.SetTrigger("Idle");

        directionX = 0;
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
        AttackPlayer = false;
        directionX = 0;
        thisAnimator.SetBool("AttackPlayer", AttackPlayer);

        if (!isAnimationState("BossHit"))
            thisAnimator.SetTrigger("BossHit");
    }

    void Chase()
    {
        AttackPlayer = false;
        thisAnimator.SetBool("AttackPlayer", AttackPlayer);
        thisAnimator.SetTrigger("BossChase");

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
        AttackPlayer = true;
        thisAnimator.SetBool("AttackPlayer", AttackPlayer);

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
        thisAnimator.SetFloat("BossSpeed", 0);

        if (Time.time >= lastAttack)
        {
            thisAnimator.SetTrigger("BossAttack");
            lastAttack = Time.time + attackDelay;
            player.GetComponent<PlayerController2D>().Damage(20);
        }
    }

    void Dead()
    {
        AttackPlayer = false;
        directionX = 0;
        thisAnimator.SetBool("AttackPlayer", AttackPlayer);
        thisAnimator.SetTrigger("BossDeath");
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