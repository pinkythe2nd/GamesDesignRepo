using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed = 20f;
    public float damage = 10;
    public GameObject target;


    void Update()
    {
        if (target == null)
        {
            Destroy(gameObject);
            return;
        }

        Vector3 direction = (target.transform.position - transform.position).normalized;
        transform.Translate(direction * speed * Time.deltaTime, Space.World);

        Quaternion lookRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, lookRotation, speed * Time.deltaTime);


        if (target != null && Vector3.Distance(transform.position, target.transform.position) <= 5.0f)
        {
            Debug.Log("arrow collision");
            {
                Unit targetUnit = target.GetComponent<Unit>();
                if (targetUnit != null)
                {
                    targetUnit.health -= damage;
                    target.GetComponent<Unit>().deathCheck(target);
                }

                Destroy(gameObject);
            }
        }

    }
}
