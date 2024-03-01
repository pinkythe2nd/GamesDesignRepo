using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;

public class SelectionManager : MonoBehaviour
{
    public static SelectionManager instance { get; set; }

    public List<GameObject> allUnitsList = new List<GameObject>();
    public List<GameObject> unitsSelected = new List<GameObject>();

    Camera cam;
    public LayerMask UnitLayer;
    public LayerMask UILayer;
    public LayerMask BuildingLayer;
    public LayerMask ground;
    public GameObject groundMarker;

    public Boolean placingBuildingFlag;
    public GameObject buildingToPlace;
    public GameObject buildingInstance;


    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
        } 
        else
        {
            instance = this;
        }
    }
   
    
    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;

    }

    // Update is called once per frame
    void Update()
    {
        if (placingBuildingFlag)
        {
                // Cast a ray from the camera to the mouse position
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;

                // Check if the ray hits something on the map layer
                if (Physics.Raycast(ray, out hit, Mathf.Infinity, ground))
                {
                // Get the hit point


                    // Instantiate the building prefab at the hit point if it's not already instantiated
                    if (buildingInstance == null)
                    {
                        float height = buildingToPlace.GetComponent<MeshRenderer>().bounds.size.y;
                        Vector3 hitPoint = new Vector3(hit.point.x, hit.point.y + height, hit.point.z);

                        buildingInstance = Instantiate(buildingToPlace, hitPoint, Quaternion.identity);
                    }
                }

            // Move the building instance along with the mouse position projected onto the map
            if (buildingInstance != null)
            {
                // Cast a ray from the camera to the mouse position
                ray = Camera.main.ScreenPointToRay(Input.mousePosition);


                // Check if the ray hits something on the map layer
                if (Physics.Raycast(ray, out hit, Mathf.Infinity, ground))
                {
                    // Get the hit point

                    float height = buildingToPlace.GetComponent<MeshRenderer>().bounds.size.y;
                    Vector3 hitPoint = new Vector3(hit.point.x, hit.point.y + height / 2, hit.point.z);

                    // Update the position of the building instance to the hit point
                    buildingInstance.transform.position = hitPoint;
                }
                if (Input.GetMouseButtonDown(1) && placingBuildingFlag)
                {
                    buildingInstance = null;
                    placingBuildingFlag = false;
                }
            }
        }
        else
        {
            if (Input.GetMouseButtonDown(0))
            {
                RaycastHit hit;
                Ray ray = cam.ScreenPointToRay(Input.mousePosition);

                if (EventSystem.current.IsPointerOverGameObject())
                {
                    Debug.Log("UI element clicked!");
                    return;
                }

                // if clicked on a unit
                if (Physics.Raycast(ray, out hit, Mathf.Infinity, UnitLayer))
                {
                    if (Input.GetKey(KeyCode.LeftShift))
                    {
                        MultiSelect(hit.collider.gameObject);
                    }
                    else
                    {
                        SelectByClicking(hit.collider.gameObject);
                    }

                }
                else
                {
                    if (!Input.GetKey(KeyCode.LeftShift))
                    {
                        DeselectAll();
                    }
                }
                //if clicked on a building.
                if (Physics.Raycast(ray, out hit, Mathf.Infinity, BuildingLayer))
                {
                    BuildingManager.instance.updateBuildings();
                }


            }
            if (Input.GetMouseButtonDown(1) && unitsSelected.Count > 0)
            {
                RaycastHit hit;
                Ray ray = cam.ScreenPointToRay(Input.mousePosition);

                if (Physics.Raycast(ray, out hit, Mathf.Infinity, ground))
                {
                    groundMarker.transform.position = new Vector3(hit.point.x, hit.point.y + 2, hit.point.z);

                    groundMarker.SetActive(false);
                    groundMarker.SetActive(true);
                }
            }
        }
    }


    public void WhatUnit(GameObject gameObject)
    {
        if (gameObject.GetComponent<Unit>().unitType == Unit.Type.Settler)
        {
            UIManager.instance.ChangeToBuildings();
        }
    }


    public void DeselectAll()
    {
        foreach (var unit in unitsSelected)
        {
            SelectUnit(unit, false);
        }
        unitsSelected.Clear();
        groundMarker.SetActive(false);
        UIManager.instance.ChangeToNone();

        placingBuildingFlag = false;
        unitsSelected.Remove(buildingInstance);
        Destroy(buildingInstance);

    }

    private void SelectByClicking(GameObject unit)
    {
        DeselectAll();
        unitsSelected.Add(unit);
        SelectUnit(unit, true);



    }

    private void MultiSelect(GameObject unit)
    {
        if (unitsSelected.Contains(unit) == false)
        {
            unitsSelected.Add(unit);
            SelectUnit(unit, true);
        }
        else
        {
            SelectUnit(unit, false);
            unitsSelected.Remove(unit);
        }
    }

    private void SelectUnit(GameObject unit, bool enabled)
    {
        if (enabled)
        {
            unit.GetComponent<Renderer>().material.color = Color.green;
        }
        else
        {
            unit.GetComponent<Renderer>().material.color = Color.white;
        }
        unit.GetComponent<unitMovement>().enabled = enabled;
        WhatUnit(unit);
    }
    internal void DragSelect(GameObject unit)
    {
        if (unitsSelected.Contains(unit) == false && unit.CompareTag("Unit"))
        {
            unitsSelected.Add(unit);
            SelectUnit(unit, true);
        }
    }
}
