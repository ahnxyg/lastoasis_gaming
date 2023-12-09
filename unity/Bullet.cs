using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private float bulletSpeed = 0.3f;
    [SerializeField] private float bulletDamage = 10f;
    private Transform target;
    // Start is called before the first frame update

    public void SetTarget(Transform _target, float damage)
    {
        bulletDamage = damage;
        target = _target;
    }
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (target==null) 
        {
            Destroy(gameObject);
            return; 
        }

        Vector2 direction = target.position - transform.position;
        rb.velocity = direction * bulletSpeed;
    }
    private void OnTriggerEnter2D(Collider2D col)
    {
        try
        {
            if (col == null || target == null) return;
            if (col.tag != target.tag) return;
            if (target == null)
            {
                Destroy(gameObject);
            }
            if (target.gameObject.layer == 7 || target.gameObject.layer==10)
            {
                if (target.name == "Knight(Clone)")
                {
                    target.gameObject.GetComponent<KnightController>().takeHit(bulletDamage);
                    Destroy(gameObject);
                }
                else if (target.name == "Wizard(Clone)")
                {
                    target.gameObject.GetComponent<WizardController>().takeHit(bulletDamage);
                    Destroy(gameObject);
                }
                else if (target.name == "Bullet1(Clone)") return;
                else
                {
                    target.gameObject.GetComponent<MobController>().takeHit(bulletDamage);
                    Destroy(gameObject);
                }
            }
            if (target.gameObject.layer == 9 || target.gameObject.layer == 8)
            {
                target.gameObject.GetComponent<Building>().TakeDamage(bulletDamage);
                Destroy(gameObject);
            }
        }
        catch (MissingReferenceException)
        {
            Destroy(gameObject);
            return;
        }
    }
    /*private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }*/
}
