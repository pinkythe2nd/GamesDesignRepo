using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class buildingPlacer : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }


    private void OnTriggerEnter(Collider other)
    {
        GetComponent<Renderer>().material.color = Color.red;
        SelectionManager.instance.ableToBuild = false;
    }

    private void OnTriggerExit(Collider other)
    {
        GetComponent<Renderer>().material.color = Color.white;
        SelectionManager.instance.ableToBuild = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
