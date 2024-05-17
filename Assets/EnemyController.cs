using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using static UnityEngine.GraphicsBuffer;

public class EnemyController : MonoBehaviour
{
    public float moveRange = 10.0f; 

    private Vector3 targetPosition;
    private NavMeshAgent agent;
    private Animator animator;
    Boolean idleing;


    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        MoveToRandomPosition();
        idleing = false;
    }

    void Update()
    {
        if (!agent.pathPending && agent.remainingDistance < 2.5f && !agent.isStopped)
        {
            animator.SetBool("IsWalking", false);
            if (!idleing)
            {
                Invoke("MoveToRandomPosition", UnityEngine.Random.Range(2f, 10f));
                idleing = true;
            }

        }
    }

    void MoveToRandomPosition()
    {
        idleing = false;
        Vector3 randomDirection = UnityEngine.Random.insideUnitSphere * moveRange;
        randomDirection += transform.position;
        NavMeshHit hit;
        NavMesh.SamplePosition(randomDirection, out hit, moveRange, 1);
        Vector3 finalPosition = hit.position;

        agent.SetDestination(finalPosition);
        animator.SetBool("IsWalking", true);
    }
}
