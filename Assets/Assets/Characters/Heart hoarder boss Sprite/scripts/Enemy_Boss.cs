using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Boss : MonoBehaviour
{
    SpriteRenderer thisSpriteRenderer;
    Rigidbody2D thisRigidbody2D;
    Animator thisAnimator;

    // Boss_Heralth
    [Range(0, 240)] public float health = 240;
    public float lastHealth;

   public bool BossIsAlive = true;
    bool isMovable = true;

    float animationDelay;

    bool AttackPlayer = false;

    public float damage = 50;

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

        lastHealth = health;

        Physics2D.IgnoreLayerCollision(3, 8);

        enemyState = EnemyState.Idle;
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


        if (BossIsAlive)
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
        #region Boss movement;
        if (BossIsAlive)
        {
            if (isMovable)
            {
                thisRigidbody2D.velocity = new Vector2(directionX, thisRigidbody2D.velocity.y);
            }
            else
            {
                thisRigidbody2D.velocity = new Vector2(0, 0);
            }
        }
        #endregion
    }
    private void FixedUpdate()
    {

        if (BossIsAlive)
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
                enemyState = EnemyState.Idle;
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
    // Disables movement
    IEnumerator DisableMovement(float duration)
    {
        Debug.Log("DISABLING DA MOVEMENT...");
        isMovable = false;
        yield return new WaitForSeconds(duration);
        isMovable = true;
    }

    void Idle()
    {
        AttackPlayer = false;
        thisAnimator.SetBool("AttackPlayer", AttackPlayer);
        thisAnimator.SetTrigger("Idle");

        directionX = 0;
    }

    // Plays the hit animation
    public void Hit()
    {
        StartCoroutine(DisableMovement(0.6f));
        AttackPlayer = false;
        directionX = 0;
        animationDelay = 0.4f;
        thisAnimator.SetBool("attackingPlayer", AttackPlayer);
        StartCoroutine(PlayAnimationWithDelay("BossHit", animationDelay));
    }

    void Chase()
    {
        AttackPlayer = false;
        thisAnimator.SetBool("attackingPlayer", AttackPlayer);
        StartCoroutine(PlayAnimationWithDelay("BossChase", animationDelay));

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
        StartCoroutine(DisableMovement(3f));
        AttackPlayer = true;
        thisAnimator.SetBool("AttackPlayer", AttackPlayer);

        LookAtRightDirection();

        // Enemy won't move
        directionX = 0;

        if (Time.time >= lastAttack)
        {
            AudioManager.instance.PlayOneShot(FMODEvents.instance.bossAttacks, this.transform.position);
            StartCoroutine(PlayAnimationWithDelay("BossAttack", animationDelay));
            lastAttack = Time.time + attackDelay;
            player.GetComponent<PlayerController2D>().Damage(damage);
        }
    }

    void Dead()
    {
        StartCoroutine(DisableMovement(1.2f));
        BossIsAlive = false;
        AttackPlayer = false;
        directionX = 0;
        animationDelay = 0.4f;
        thisAnimator.SetBool("AttackPlayer", AttackPlayer);
        StartCoroutine(PlayAnimationWithDelay("BossDeath", animationDelay));
        Destroy(gameObject, 1.2f);
        return;
    }
    #endregion
}