using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WizardAttack : MonoBehaviour
{
    [Header("Reference")]
    [SerializeField] private Rigidbody2D rb;


    [Header("Attributes")]
    [SerializeField] private float attackDamage;
    [SerializeField] private float damageRate;
    private float damageTimer = 0f;
    private List<MobController> targets = new List<MobController>();

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log(other.gameObject.tag);
        if (other.gameObject.CompareTag("Red"))
        {
            Debug.Log("C");
            MobController mob = other.gameObject.GetComponent<MobController>();
            if (mob != null && !targets.Contains(mob))
            {
                targets.Add(mob);
            }
        }
    }


    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Red"))
        {
            MobController mob = other.gameObject.GetComponent<MobController>();
            if (mob != null && targets.Contains(mob))
            {
                targets.Remove(mob);
            }
        }
    }


    // Update is called once per frame
    void Update()
    {
        damageTimer -= Time.deltaTime;
        try
        {
            if (damageTimer <= 0f && targets != null)
            {
                List<MobController> mobs = new List<MobController>(targets);

                foreach (MobController mob in mobs)
                {


                    Debug.Log(mob.name);
                    mob.takeHit(attackDamage);
                }
                damageTimer = damageRate;
            }
        }
        catch
        {

        }
    }
}
