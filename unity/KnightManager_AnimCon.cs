using Spine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//넣을거 -> attack, walk, idle
public class KnightManager_AnimCon : MonoBehaviour
{
    [Header ("# Stats")]
    public float speed;
    public float maxHealth;
    public float health;
    public float attackRange;
    public float damage;
    public string selfTag;

    private float attackTimer = 0.0f; // 공격 간격 타이머
    private float attackCooldown = 1.0f; // 공격 간격 (1.0초)

    private bool canAttack = true;

    public RuntimeAnimatorController animCon;
    Rigidbody2D rigid;
    SpriteRenderer spriter;
    Animator anim;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        spriter = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        gameObject.tag = "Red";
    }

    void Start()
    {
        anim.SetTrigger("KnightIdle");
        selfTag = gameObject.tag;
    }

    private void OnEnable()
    {
        anim.runtimeAnimatorController = animCon;
    }

    void Update()
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

        if (enemy != null)
        {
            float distance = Vector2.Distance(transform.position, enemy.transform.position);
            if (distance <= attackRange)
            {
                if (enemy.tag != gameObject.tag && attackTimer <= 0)
                {
                    Attack(enemy);
                }
            }
            else
            {
                if (enemy.tag != gameObject.tag)
                {
                    Walk(enemy);
                }
            }
        }
        else
        {
            anim.SetTrigger("KnightIdle");
        }
    }

    private GameObject ClosestEnemy()
    {
        GameObject[] enemy = null;
        if (selfTag == "Blue")
        {
            enemy = GameObject.FindGameObjectsWithTag("Red");
        }
        else
        {
            enemy = GameObject.FindGameObjectsWithTag("Blue");
        }

        GameObject closestEnemy = null;
        float closestDistance = Mathf.Infinity;

        foreach (GameObject e in enemy)
        {
            float distance = Vector3.Distance(transform.position, e.transform.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestEnemy = e;
            }
        }

        return closestEnemy;
    }

    public void Walk(GameObject closestEnem)
    {
        if (closestEnem != null)
        {
            anim.SetTrigger("KnightWalk");
            Vector3 direction = (closestEnem.transform.position - transform.position).normalized;
            direction.y = -direction.y;
            transform.Translate(-direction * speed * Time.deltaTime);
        }
    }

    public void Attack(GameObject closestEnemy)
    {
        if (closestEnemy != null && canAttack)
        {
            MobController mob = closestEnemy.GetComponent<MobController>();

            if (!anim.GetCurrentAnimatorStateInfo(0).IsName("KnightAttack"))
            {
                anim.SetTrigger("KnightAttack");
            }

            mob.takeHit(damage);

            attackTimer = attackCooldown;
            canAttack = false;
            //StartCoroutine(AttackCoolDown());
        }
    }

    /*private IEnumerator AttackCoolDown()
    {
       yield return new WaitForSeconds(0.5f);
       canAttack = true;
    }*/
}
