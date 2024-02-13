using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Ghoul : MonoBehaviour
{
    SpriteRenderer thisSpriteRenderer;
    Rigidbody2D thisRigidbody2D;
    Animator thisAnimator;
    BoxCollider2D thisCollider2D;
    PlayerController2D thisPlayerController2D;

    // Ghoul_Heralth
    [Range(0, 60)] public float health = 60;
    public float lastHealth;
    bool isAlive = true;

    bool attackingPlayer = false;

    public float animationDelay;
    public float movementDelayAfterHit = 0.4f;

    float damage = 20;

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

        startPosition = transform.position;
        endPosition = transform.position + Vector3.right * patrolDistance;

        lastHealth = health;

        enemyState = EnemyState.Patrol;
    }

    // Update is called once per frame
    void Update()
    {
        #region RAYCASTS
        //distanceToPlayer = Vector2.Distance(transform.position, player.transform.position);

        //if (Vector2.Distance(transform.position, player.transform.position) <= attackRange)
        //{
        //    Debug.DrawRay(transform.position, player.transform.position - transform.position, Color.red);
        //}
        //else if (Vector2.Distance(transform.position, player.transform.position) <= chaseDistance)
        //{
        //    Debug.DrawRay(transform.position, player.transform.position - transform.position, Color.magenta);
        //}
        //else
        //{
        //    Debug.DrawRay(transform.position, player.transform.position - transform.position, Color.green);
        //}
        #endregion

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

    private void FixedUpdate()
    {

        if (isAlive)
        {
            thisRigidbody2D.velocity = new Vector2(directionX, thisRigidbody2D.velocity.y);
        }
    }

    void Logic()
    {

        if (health <= 0)
        {
            enemyState = EnemyState.Dead;
            return;
        }
        else if (lastHealth > health)
        {
            enemyState = EnemyState.Hit;
            lastHealth = health;
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

    public void LookAtRightDirection()
    {
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
    }

    //Takes Damage from player
    public void TakeDamage(float _damage)
    {
        if (Time.time > lastHit + 0.1f)
        {
            AudioManager.instance.PlayOneShot(FMODEvents.instance.ghouGetsHit, this.transform.position);
            health -= _damage;
            lastHit = Time.time;
        }
    }
    //Creates animation delay
    IEnumerator PlayAnimationWithDelay(string animationTrigger, float delay)
    {
        // Add a delay before playing the specified animation
        yield return new WaitForSeconds(delay);

        // Trigger the specified animation
        thisAnimator.SetTrigger(animationTrigger);
    }

    //Creates movement delay
    IEnumerator MovementDelay()
    {
        yield return new WaitForSeconds(movementDelayAfterHit);
        directionX = 1; // or any other default value you want after the delay
    }
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
    // Plays the hit animation
    public void Hit()
    {
        attackingPlayer = false;
        animationDelay = 0.4f;
        directionX = 0;
        thisAnimator.SetBool("attackingPlayer", attackingPlayer);
        StartCoroutine(PlayAnimationWithDelay("GhoulHit", animationDelay));
        StartCoroutine(MovementDelay());
    }

    void Chase()
    {
        attackingPlayer = false;
        thisAnimator.SetBool("attackingPlayer", attackingPlayer);

        LookAtRightDirection();

        // To fix twitching when alignd on X axis but not on Y
        float distX = transform.position.x - player.position.x;

        if (Mathf.Abs(distX) < 0.1f)
        {
            directionX = 0;
        }

    }

    void Attack()
    {
        attackingPlayer = true;
        directionX = 0;
        thisAnimator.SetBool("attackingPlayer", attackingPlayer);

        LookAtRightDirection();
        StartCoroutine(MovementDelay());

        if (Time.time >= lastAttack)
        {
            AudioManager.instance.PlayOneShot(FMODEvents.instance.ghoulAttacks, this.transform.position);
            StartCoroutine(PlayAnimationWithDelay("GhoulAttack", animationDelay));
            lastAttack = Time.time + attackDelay;
            player.GetComponent<PlayerController2D>().Damage(damage);
        }
    }

    void Dead()
    {
        attackingPlayer = false;
        thisAnimator.SetBool("attackingPlayer", attackingPlayer);
        directionX = 0;
        animationDelay = 0.05f;
        StartCoroutine(PlayAnimationWithDelay("GhoulDeath", animationDelay));
        StartCoroutine(MovementDelay());
        Destroy(gameObject, 1.2f);
        return;
    }
    #endregion
}