using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class Unit : MonoBehaviour
{
    public string toolTip;
    public string name;

    public float woodCost;
    public float stoneCost;

    public float health;
    public float maxHealth;

    public float damage;
    public GameObject target;

    public Sprite Icon;
    public Texture2D IconFile;

    public Boolean trainable;
    public Boolean enemy;
    
    public Boolean ranged;
    public float range;
    public GameObject projectile;
    public GameObject spawnpoint;

    public float trainTime;
    public int value;

    private NavMeshAgent agent;
    private Animator animator;

    public enum Type
    {
        Settler,
        Warrior,
        Archer,
        Building,
    }

    public Type unitType;
    // Start is called before the first frame update

    void Start()
    {
        SelectionManager.instance.allUnitsList.Add(gameObject);
        Icon = Sprite.Create(IconFile, new Rect(0, 0, IconFile.width, IconFile.height), Vector2.zero);
        if (unitType != Type.Building)
        {
            agent = GetComponent<NavMeshAgent>();
            animator = GetComponent<Animator>();
        }

    }

    void Update()
    {
        if (unitType == Type.Building) return;
        if (target == null && animator.GetBool("IsAttacking")) animator.SetBool("IsAttacking", false);
        if (target == null) return;
        
        agent.SetDestination(target.transform.position);
        float distanceToTarget = Vector3.Distance(transform.position, target.transform.position);

        if(ranged)
        {
            if (distanceToTarget < range)
            {
                animator.SetBool("IsAttacking", true);
                agent.ResetPath();

                Vector3 direction = (target.transform.position - transform.position).normalized;
                direction.y = 0; 

                Quaternion targetRotation = Quaternion.LookRotation(direction);
                targetRotation *= Quaternion.Euler(0, 90, 0);
                transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, agent.angularSpeed * Time.deltaTime);

            }
            if (distanceToTarget > range)
            {
                animator.SetBool("IsAttacking", false);
            }
        }

        float targetRadius = 0f;
        Collider targetCollider = target.GetComponent<Collider>();
        if (targetCollider != null)
        {
            targetRadius = targetCollider.bounds.extents.magnitude;
        }


        float threshold = targetRadius + 1.0f; 

        if (distanceToTarget < threshold) 
        {
            animator.SetBool("IsAttacking", true);
        }

    }

    public void deathCheck(GameObject target)
    {
        Unit unit = target.GetComponent<Unit>();

        if (unit.health <= 0)
        {
            if (unit.unitType == Type.Building)
            {
                unit.GetComponent<Building>().killBuilding();
            }
            target.GetComponent<Animator>().SetBool("IsDead", true);
        }
    }

    private void DeathEnd()
    {
        Destroy(gameObject);
    }

    private void PunchEnd()
    {
        if (target == null) return;
        target.GetComponent<Unit>().health -= damage;
        deathCheck(target);
    }

    private void StabEnd()
    {
        if (target == null) return;

        target.GetComponent<Unit>().health -= damage;
        deathCheck(target);
    }

    private void SlashEnd()
    {
        if (target == null) return;

        target.GetComponent<Unit>().health -= damage;
        deathCheck(target);
    }

    private void ShootEnd()
    {
        if (target == null) return;
        GameObject arrowObject = Instantiate(projectile, spawnpoint.transform.position, (Quaternion.identity * Quaternion.Euler(-90,0,0)));
        Projectile arrow = arrowObject.GetComponent<Projectile>();


        arrow.target = target;
        arrow.damage = damage;

        //target.GetComponent<Unit>().health -= damage;
        //deathCheck(target);
    }    

    private void OnDestroy()
    {
        SelectionManager.instance.allUnitsList.Remove(gameObject);
        SelectionManager.instance.unitsSelected.Remove(gameObject);

    }

}
