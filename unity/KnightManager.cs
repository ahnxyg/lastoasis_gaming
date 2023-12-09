using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnightManager : MonoBehaviour
{
    public float speed;
    public float maxHealth;
    public float health;
    public float attackRange;
    public float damage;
    public string selfTag;

    private float attackTimer = 0.0f; // 공격 간격 타이머
    private float attackCooldown = 1.0f; // 공격 간격 (1.0초)

    private bool canAttack = true;
    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        gameObject.tag = "Red";
    }
    void Start()
    {
        animator.SetTrigger("KnightIdle");
    }

    void FixedUpdate()
    {
        GameObject enemy = ClosestEnemy();
        float enemyPositionX = enemy.transform.position.x;
        float myPositionX = transform.position.x;
        attackTimer -= Time.deltaTime;
        if (gameObject.tag == "Blue")
        {
            if (enemyPositionX < myPositionX)
            {
                gameObject.transform.rotation = Quaternion.Euler(0, 0, 0);
            }
            else
            {
                gameObject.transform.rotation = Quaternion.Euler(0, 180, 0);
            }
        }
        if (gameObject.tag == "Red")
        {
            if (enemyPositionX > myPositionX)
            {
                gameObject.transform.rotation = Quaternion.Euler(0, 0, 0);
            }
            else
            {
                gameObject.transform.rotation = Quaternion.Euler(0, 180, 0);
            }
        }

        if (enemy != null && enemy.tag != gameObject.tag)
        {
            float distance = Vector3.Distance(transform.position, enemy.transform.position);
            if (distance <= attackRange)
            {
                if (attackTimer <= 0)
                {
                    animator.SetTrigger("KnightIdle");
                    /*Attack(enemy);
                    attackTimer = attackCooldown;*/
                    animator.SetTrigger("KnightAttack");
                    animator.SetTrigger("KnightWalk");
                    Attack(enemy);
                    attackTimer = attackCooldown;
                    Attack(enemy);
                    attackTimer = attackCooldown;
                }
            }
            else
            {
                //Walk(enemy);
                    animator.SetTrigger("KnightWalk");
                    Vector3 direction = (enemy.transform.position - transform.position).normalized;
                    direction.y = -direction.y;
                    transform.Translate(-direction * speed * Time.deltaTime);
            }
        }
        
    }

    public void Walk(GameObject closestEnem)
    {
        if (!canAttack) return;
        if(closestEnem != null)
        {
            animator.SetTrigger("KnightWalk");
            Vector3 direction = (closestEnem.transform.position - transform.position).normalized;
            direction.y = -direction.y;
            transform.Translate(-direction * speed * Time.deltaTime);
        }
    }

    void ApplyDamage()
    {
        GameObject enemy = ClosestEnemy();
        if (attackTimer <= 0)
        {
            Attack(enemy);
            attackTimer = attackCooldown;
        }
    }

    public void Attack(GameObject closestEnemy)
    {
       if(closestEnemy != null)
        {
            MobController mob = closestEnemy.GetComponent<MobController>();
            //animator.SetTrigger("KnightAttack");
            mob.takeHit(damage);
            
            canAttack = false;
            //StartCoroutine(AttackCoolDown());
        }
    }

     /*private IEnumerator AttackCoolDown()
     {
        yield return new WaitForSeconds(0.5f);
        canAttack = true;
     }*/

    private GameObject ClosestEnemy()
    {
        GameObject[] enemy = null;
        if(selfTag == "Blue")
        {
            enemy = GameObject.FindGameObjectsWithTag("Red");
        }
        else
        {
            enemy = GameObject.FindGameObjectsWithTag("Blue");
        }

        GameObject closestEnemy = null;
        float closestDistance = Mathf.Infinity;

        foreach(GameObject e in enemy)
        {
            float distance = Vector3.Distance(transform.position, e.transform.position);
            if(distance < closestDistance)
            {
                closestDistance = distance;
                closestEnemy = e;
            }
        }

        return closestEnemy;
    }
}
