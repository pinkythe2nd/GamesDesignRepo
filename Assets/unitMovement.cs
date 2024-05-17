using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class unitMovement : MonoBehaviour
{
    Camera cam;
    NavMeshAgent agent;
    unitMovement movementScript;
    Unit unitScript;
    public Animator animator;
    private Vector3 previousPosition;
    public Boolean settlerIsBuilding;

    public LayerMask ground;
    public float movementThreshold = 0.5f; // Adjust this threshold as needed

    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        movementScript = GetComponent<unitMovement>();
        previousPosition = transform.position;
        settlerIsBuilding = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            RaycastHit hit;
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit, Mathf.Infinity, ground))
            {

                if (!animator.GetBool("IsWalking"))
                {
                    InvokeRepeating("stopMoving", 0.1f, 0.1f); 
                }
                agent.isStopped = false;
                agent.SetDestination(hit.point);
                animator.SetBool("IsWalking", true);


                previousPosition = transform.position;
            }
        }

    }

    void stopMoving()
    {
        float distanceMoved = Vector3.Distance(transform.position, previousPosition);
        float movementThreshold = agent.velocity.magnitude;

        if (distanceMoved > movementThreshold) 
        {
            // Unit has not moved much, set boolean to false
            if (settlerIsBuilding)
            {
                animator.SetBool("IsDigging", true);
                animator.SetBool("IsWalking", false);
                agent.isStopped = true;
                CancelInvoke("stopMoving");
                return;
            }
            animator.SetBool("IsWalking", false);

            CancelInvoke("stopMoving");
            agent.isStopped = true;
        }

        // Update the previous position for the next frame
        previousPosition = transform.position;


    }

}
