using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public Animator animator;
    public Transform attackPoint;
    public float attackRange = 0.5f;
    public int attackDamage = 40;
    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            Attack();
        }
    }
   
    void Attack()
    {
        animator.SetTrigger("Attack");

        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

        foreach (GameObject enemy in enemies)
        {
            Enemies enemyScript = enemy.GetComponent<Enemies>();

            if (enemyScript != null)
            {
                enemyScript.TakeDamage(attackDamage);
                Debug.Log("We hit " + enemy.name + " for " + attackDamage + " damage.");
            }
        }
    }
    private void OnDrawGizmos()
    {
        if (attackPoint == null)
            return;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}
