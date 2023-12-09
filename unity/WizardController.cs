using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WizardController : MonoBehaviour
{
    public float speed;
    public float maxHealth;
    public float health;
    public float attackRange;
    public float damage;
    public string selfTag;
    public GameObject c;
    private float attackTimer = 0.0f; // 공격 간격 타이머
    private float attackCooldown = 10f; // 공격 간격 (1.0초)

    private bool performAttack = true;
    private bool canAttack = true;
    private Animator animator;
    public GameObject enemy;
    public GameObject selfNexus;

    private Vector3 initialPosition;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        initialPosition = transform.position;
        if(GameManager.instance.gameTime < 60)
        {
            maxHealth = 3000;
            health = 3000;
        }else if(GameManager.instance.gameTime >=60 && GameManager.instance.gameTime < 180)
        {
            maxHealth = 7000;
            health = 7000;
        }else if(GameManager.instance.gameTime >= 180 && GameManager.instance.gameTime < 300)
        {
            maxHealth = 9000;
            health = 9000;

        }else if(GameManager.instance.gameTime >= 300)
        {
            maxHealth = 15000;
            health = 15000;
        }

    }

    // Update is called once per frame
    void Update()
    {
        
        if (attackTimer > attackCooldown)
        {
            attackTimer = 0.0f;
            canAttack = true;
        }
        enemy = ClosestEnemy();
        if(enemy == null) return;
        float enemyPositionX = enemy.transform.position.x;
        float myPositionX = transform.position.x;

        if (enemyPositionX > myPositionX)
        {
            gameObject.transform.rotation = Quaternion.Euler(0, 0, 0);
        }
        else
        {
            gameObject.transform.rotation = Quaternion.Euler(0, 180, 0);
        }
        if (enemy != null)
        {
            float distance = Vector3.Distance(transform.position, enemy.transform.position);

            if (canAttack && performAttack && distance <= attackRange && !animator.GetCurrentAnimatorStateInfo(0).IsName("WizardAttack"))
            {
                c.SetActive(true);
                performAttack = false;
                //canAttack = false;
                StartCoroutine(AttackInterval());
            }
            else
            {
                if (!animator.GetCurrentAnimatorStateInfo(0).IsName("WizardAttack") && distance > attackRange)
                {

                    c.SetActive(false);
                    /*if (distance > attackRange * 1.5f)
                    {
                        Vector3 directionToInitialPosition = (initialPosition - transform.position).normalized;
                        Vector3 moveVector = directionToInitialPosition * speed * Time.deltaTime;
                        transform.position += moveVector;
                    }*/
                    /* else
                        {*/
                    animator.SetBool("WizardWalk", true);
                    Vector3 targetPosition = enemy.transform.position;
                    Vector3 direction = (targetPosition - transform.position).normalized;
                    Vector3 moveVector = direction * speed * Time.deltaTime;
                    transform.position += moveVector;
                    // }
                }
            }
        }
        else
        {
            /* // 적이 없는 경우 초기 위치로 이동
                Vector3 directionToInitialPosition = (initialPosition - transform.position).normalized;
                Vector3 moveVector = directionToInitialPosition * speed * Time.deltaTime;
                transform.position += moveVector;*/
        }
        
        
    }
    private GameObject ClosestEnemy()
    {
        GameObject[] enemy = GameObject.FindGameObjectsWithTag("Red");
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

    IEnumerator AttackInterval()
    {
        performAttack = false;

        animator.SetBool("WizardAttack", true);
        float animationLength = animator.GetCurrentAnimatorStateInfo(0).length;
        yield return new WaitForSeconds(7 * animationLength / 8);
        Attack(enemy);

        yield return new WaitForSeconds(animationLength / 8);
        animator.SetBool("WizardAttack", false);

        performAttack = true;

    }

    public void Attack(GameObject closestEnemy)
    {
        try
        {
            string temp = closestEnemy.gameObject.name;

            if (closestEnemy != null && temp == "Knight(Clone)")
            {
                KnightController knight = closestEnemy.GetComponent<KnightController>();
                knight.takeHit(damage);
            }
            else if (closestEnemy != null && (closestEnemy.layer == 8 || closestEnemy.layer == 9)) //건물일경우
            {
                Building building = closestEnemy.GetComponent<Building>();
                building.TakeDamage(damage);
            }
            else if (closestEnemy != null && closestEnemy.layer == 7) //몬스터일경우
            {
                MobController mob = closestEnemy.GetComponent<MobController>();
                mob.takeHit(damage);
            }
        }
        catch
        {

        }
    }

    public void takeHit(float damage)
    {
        try
        {
            health -= damage;
            if (health <= 0)
            {
                Destroy(gameObject);
                GameObject selfNexus = GameObject.Find("BlueNexus");
                selfNexus.tag = "Blue";
            }
        }
        catch (MissingReferenceException)
        {
            return;
        }
    }
}
