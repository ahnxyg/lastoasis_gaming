using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Building : MonoBehaviour
{
    public static Building instance;
    public float maxHealth = 100; // ???? ????
    public float currentHealth;    // ???? ????
    public int defense = 0;     // ??????
    public GameObject specialUnit;

    public GameObject dmgtxt;
    public TMP_Text popupText;

    public bool alive = true;

    private bool canDealDamage = true; // ?????? ???? ?????????? ????

    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        currentHealth = maxHealth; 
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (canDealDamage)
        {
            if (gameObject.tag == "RedBuilding" && collision.tag=="Blue")
            {
                int damage = (int)collision.GetComponent<MobController>().damage;
                TakeDamage(damage);

                // ???? ?? ???? ???? ???? ???? ????
                canDealDamage = false;
                StartCoroutine(ResetDamageCooldown(collision.GetComponent<MobController>().attackSpeed));
            }
            else if(gameObject.tag=="BlueBuilding" && collision.tag=="Red")
            {
                int damage = (int)collision.GetComponent<MobController>().damage;
                TakeDamage(damage);

                // ???? ?? ???? ???? ???? ???? ????
                canDealDamage = false;
                StartCoroutine(ResetDamageCooldown(collision.GetComponent<MobController>().attackSpeed));
            }
        }
    }

    private IEnumerator ResetDamageCooldown(float attackSpeed)
    {
        yield return new WaitForSeconds(1f / attackSpeed);
        canDealDamage = true;
    }

    public void TakeDamage(float damage)
    {

        float damageTaken = Mathf.Max(0, damage - defense);
        popupText.text = damageTaken.ToString();
        Vector3 pos = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y + 2, 0);
        Instantiate(dmgtxt, pos, Quaternion.identity);
        currentHealth -= damageTaken;

        if (currentHealth <= 0)
        {
            if(this == null) { return; }

            if (gameObject.layer == 8)
            {
                if (gameObject.tag == "Red")
                {
                    GameManager.instance.resource += 500;
                }
                else
                {
                    GameManager.instance.redResource += 500;
                }
                if (GameObject.Find(specialUnit.name + "(Clone)") != null) return;
                GameObject InsPre = Instantiate(specialUnit, transform.position, Quaternion.identity);
                InsPre.tag = gameObject.tag;
                InsPre.transform.localScale = new Vector3(8.0f, 8.0f, 8.0f);
            }
            DestroyBuilding();
        }
    }

    void DestroyBuilding()
    {
        Destroy(gameObject);
        if(gameObject.layer==9 && gameObject.tag=="Blue")
        {
            GameManager.instance.GameOver();
        }
        else if(gameObject.layer==9 && gameObject.tag=="Red")
        {
            GameManager.instance.GameVictory();
        }
    }
}


