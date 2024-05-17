using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ResourceTerrain : MonoBehaviour
{
    private Building building;
    // Start is called before the first frame update

    public enum Resource{
        stone,
        wood
    }
    public Resource resource;
    public float gatherRate = 0.01f;
    void Start()
    {

        switch (resource)
        {
            case Resource.stone:
                InvokeRepeating("gatherStone", 0f, 1f);
                break;
            case Resource.wood:
                InvokeRepeating("gatherWood", 0f, 1f);
                break;
        }
        building = GetComponent<Building>();
    }

    void gatherWood()
    {
        BuildingManager.instance.wood += building.currentGatherers * gatherRate;
    }

    void gatherStone()
    {
        BuildingManager.instance.stone += building.currentGatherers * gatherRate;
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<Unit>() == null) return;
        if (other.gameObject.GetComponent<Unit>().unitType == Unit.Type.Settler)
        {
            building.currentGatherers += 1;
            other.gameObject.GetComponent<Animator>().SetBool("IsDigging", true);
            other.gameObject.GetComponent<Animator>().SetBool("IsWalking", false);
            other.gameObject.GetComponent<unitMovement>().settlerIsBuilding = true;

            return;
        }
    }

    private void OnTriggerExit(Collider other)
    {

        if (other.gameObject.GetComponent<Unit>() == null) return;
        if (other.gameObject.GetComponent<Unit>().unitType == Unit.Type.Settler)
        {
            building.currentGatherers -= 1;
            other.gameObject.GetComponent<Animator>().SetBool("IsWalking", true);
            other.gameObject.GetComponent<Animator>().SetBool("IsDigging", false);
            other.gameObject.GetComponent<unitMovement>().settlerIsBuilding = false;
            return;
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
