using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class KnightController : MonoBehaviour
{
    public float speed;
    public float maxHealth;
    public float health;
    public float attackRange;
    public float damage;
    public string selfTag;

    private float attackTimer = 0.0f; // 공격 간격 타이머
    private float attackCooldown = 1f; // 공격 간격 (1.0초)

    private bool performAttack = true;
    private bool canAttack = true;
    private Animator animator;
    public GameObject enemy;
    public GameObject selfNexus;

    public GameObject dmgtxt;
    public TMP_Text popupText;

    private Vector3 initialPosition;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        initialPosition = transform.position;
        if (GameManager.instance.gameTime < 60)
        {
            maxHealth = 3000;
            health = 3000;
        }
        else if (GameManager.instance.gameTime >= 60 && GameManager.instance.gameTime < 180)
        {
            maxHealth = 7000;
            health = 7000;
        }
        else if (GameManager.instance.gameTime >= 180 && GameManager.instance.gameTime < 300)
        {
            maxHealth = 9000;
            health = 9000;

        }
        else if (GameManager.instance.gameTime >= 300)
        {
            maxHealth = 15000;
            health = 15000;
        }
    }

    void Update()
    {
        attackTimer += Time.deltaTime;

        if (attackTimer > attackCooldown)
        {
            attackTimer = 0.0f;
            canAttack = true;
        }
        enemy = ClosestEnemy();
        if (enemy == null) return;
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

            if (canAttack && performAttack && distance <= attackRange && !animator.GetCurrentAnimatorStateInfo(0).IsName("KnightAttack"))
            {
                performAttack = false;
                canAttack = false;
                Debug.Log("can perform and is performing");
                StartCoroutine(AttackInterval());
            }
            else
            {
                if (!animator.GetCurrentAnimatorStateInfo(0).IsName("KnightAttack") && distance > attackRange)
                {
                    animator.SetBool("KnightWalk", true);

                   /* // 아래 코드를 추가하여 초기 위치로 돌아가도록 합니다.
                    if (distance > attackRange * 1.5f)
                    {
                        Vector3 directionToInitialPosition = (initialPosition - transform.position).normalized;
                        Vector3 moveVector = directionToInitialPosition * speed * Time.deltaTime;
                        transform.position += moveVector;
                    }
                    else
                    {*/
                        Vector3 targetPosition = enemy.transform.position;
                        Vector3 direction = (targetPosition - transform.position).normalized;
                        Vector3 moveVector = direction * speed * Time.deltaTime;
                        transform.position += moveVector;
                    //}
                }
            }
        }
    }

    IEnumerator AttackInterval()
    {
        performAttack = false;

        animator.SetBool("KnightAttack", true);
        float animationLength = animator.GetCurrentAnimatorStateInfo(0).length;
        yield return new WaitForSeconds(7 * animationLength / 8);

        Attack(enemy);

        yield return new WaitForSeconds(animationLength / 8);
        animator.SetBool("KnightAttack", false);

        performAttack = true;

    }
    private GameObject ClosestEnemy()
    {
        GameObject[] enemy = GameObject.FindGameObjectsWithTag("Blue");
        GameObject closestEnemy = null;
        float closestDistance = Mathf.Infinity;

        foreach (GameObject e in enemy)
        {
            float distance = Vector3.Distance(transform.position, e.transform.position);
            if (distance < closestDistance && e.name != "Bullet1(Clone)")
            {
                closestDistance = distance;
                closestEnemy = e;
            }
        }

        return closestEnemy;
    }

    public void Attack(GameObject closestEnemy)
    {
        string temp = closestEnemy.gameObject.name;
        Debug.Log("LAYER: " + closestEnemy.layer);
        if (closestEnemy != null && temp == "Wizard(Clone)")
        {
            WizardController wiz = closestEnemy.GetComponent<WizardController>();
            wiz.takeHit(damage);
        }
        else if (closestEnemy != null && (closestEnemy.layer == 8 || closestEnemy.layer==9)) //건물일경우
        {
            Building building = closestEnemy.GetComponent<Building>();
            building.TakeDamage(damage);
        }
        
        else if(closestEnemy != null && (closestEnemy.layer==7 ||  closestEnemy.layer == 10)) //몬스터일경우
        {
            Debug.Log("ATTACKING RANGED MOB");
            MobController mob = closestEnemy.GetComponent<MobController>();
            mob.takeHit(damage);
        }
    }

    public void takeHit(float damage)
    {
        try
        {
            popupText.text = (damage).ToString();
            Vector3 pos = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y + 2, 0);
            Instantiate(dmgtxt, pos, Quaternion.identity);
            health -= damage;
            if (health <= 0)
            {
                Destroy(gameObject);
                GameObject selfNexus = GameObject.Find("RedNexus");
                selfNexus.tag = "Red";
            }
        }
        catch (MissingReferenceException)
        {
            return;
        }
    }
    
}
