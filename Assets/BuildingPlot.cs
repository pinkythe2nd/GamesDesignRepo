using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BuildingPlot : MonoBehaviour
{
    public GameObject building;
    private Building actualBuilding;
    public bool hack;
    // Start is called before the first frame update
    void Start()
    {
        actualBuilding = building.GetComponent<Building>();
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<Unit>() == null) return;
        if (other.gameObject.GetComponent<Unit>().unitType == Unit.Type.Settler)
        {
            actualBuilding.currentGatherers += 1;
            other.gameObject.GetComponent<Animator>().SetBool("IsDigging", true);
            other.gameObject.GetComponent<Animator>().SetBool("IsWalking", false);
            other.gameObject.GetComponent<unitMovement>().settlerIsBuilding = true;
            if (!actualBuilding.buildingFlag)
            {
                actualBuilding.InvokeRepeating("buildBuilding", 0f, 1f); // Replace 5f with your desired interval time
                actualBuilding.buildingFlag = true;
            }
            return;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.GetComponent<Unit>() == null) return;
        if (other.gameObject.GetComponent<Unit>().unitType == Unit.Type.Settler)
        {
            if (hack)
            {
                other.gameObject.GetComponent<Animator>().SetBool("IsWalking", false);
                other.gameObject.GetComponent<Animator>().SetBool("IsDigging", false);
                other.gameObject.GetComponent<unitMovement>().settlerIsBuilding = false;
                actualBuilding.GetComponent<NavMeshObstacle>().enabled = true;
                return;
            }
            actualBuilding.currentGatherers -= 1;
            other.gameObject.GetComponent<Animator>().SetBool("IsWalking", false);
            other.gameObject.GetComponent<Animator>().SetBool("IsDigging", false);
            other.gameObject.GetComponent<unitMovement>().settlerIsBuilding = true;
            if (actualBuilding.currentGatherers == 0)
            {
                actualBuilding.CancelInvoke("buildBuilding");
                actualBuilding.buildingFlag = false;
            }
            return;
        }
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
