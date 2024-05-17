using System;
using UnityEngine;
using UnityEngine.AI;

public class SwordsMan : MonoBehaviour
{
    private NavMeshAgent agent;
    private Animator animator;
    private Boolean somethingsColliding;
    private GameObject collidingWith;
    private Unit unit;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        unit = GetComponent<Unit>();

        collidingWith = null;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (unit.target == null)
        {
            if (other.gameObject.GetComponent<ResourceTerrain>() == null &&
                other.gameObject.GetComponent<Unit>().enemy == false)
            {
                unit.target = other.gameObject;
            }
        }

    }

    private void OnTriggerExit(Collider other)
    {
        unit.target = null;
    }

    // Update is called once per frame
    void Update()
    {
    }
}
