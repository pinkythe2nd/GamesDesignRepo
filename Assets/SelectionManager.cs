using System;
using System.Collections.Generic;
using UnityEditor.ShaderKeywordFilter;
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
    public GameObject BuildingPlacer;

    public Boolean placingBuildingFlag;
    public Boolean ableToBuild;
    public Boolean initBuilding;

    public Texture2D cursorTexture;

    public GameObject buildingToPlace;
    public GameObject buildingInstance;
    public GameObject buildingPlot;

    private Boolean cursorSet;


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
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        cursorSet = false;

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, UnitLayer))
        {
            if (unitsSelected.Count > 0)
            {
                if (hit.collider.gameObject.GetComponent<Unit>().enemy)
                {
                    Cursor.SetCursor(cursorTexture, Vector2.zero, CursorMode.Auto);
                    cursorSet = true;
                }
            }
        }
        if (!cursorSet)
        {
            Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
        }

        if (placingBuildingFlag)
        {
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, ground))
            {
                if (placingBuildingFlag && initBuilding)
                {
                    Vector3 scale = buildingToPlace.GetComponent<Renderer>().bounds.size;
                    BuildingPlacer.SetActive(true);
                    BuildingPlacer.GetComponent<Transform>().localScale = new Vector3(scale.x, 1, scale.z);
                    ableToBuild = true;
                    initBuilding = false;
                }
                Vector3 PlacerhitPoint = new Vector3(hit.point.x, hit.point.y, hit.point.z);
                BuildingPlacer.transform.position = PlacerhitPoint;
            }

            if (Input.GetMouseButtonDown(1) && placingBuildingFlag && ableToBuild)
            {
                float height = buildingToPlace.GetComponent<MeshRenderer>().bounds.size.y;

                Vector3 hitPoint = new Vector3(hit.point.x, hit.point.y - height / 2, hit.point.z);
                Vector3 PlacerhitPoint = new Vector3(hit.point.x, hit.point.y, hit.point.z);

                buildingInstance = Instantiate(buildingToPlace, hitPoint, buildingToPlace.GetComponent<Transform>().rotation);
                buildingInstance.GetComponent<NavMeshObstacle>().enabled = false;
                buildingInstance.GetComponent<Rigidbody>().isKinematic = true;

                Vector3 scale = buildingToPlace.GetComponent<Renderer>().bounds.size;

                BuildingPlacer.SetActive(true);
                BuildingPlacer.GetComponent<Transform>().localScale = new Vector3(scale.x, 1, scale.z);

                GameObject plot = Instantiate(buildingPlot, PlacerhitPoint, Quaternion.Euler(0,0,0));
                plot.GetComponent<Transform>().localScale = new Vector3(scale.x, 1, scale.z);

                plot.GetComponent<SphereCollider>().radius = 1f;
                plot.GetComponent<BuildingPlot>().building = buildingInstance;
                buildingInstance.GetComponent<Building>().plotToDelete = plot;

                //after placed take away how much it cost.
                Unit buildingUnitScript = buildingInstance.GetComponent<Unit>();
                BuildingManager.instance.stone -= buildingUnitScript.stoneCost;
                BuildingManager.instance.wood -= buildingUnitScript.woodCost;

                BuildingPlacer.SetActive(false);
                buildingInstance.SetActive(true);
                buildingInstance = null;
                placingBuildingFlag = false;
                initBuilding = true;
            }
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                ableToBuild = false;
                BuildingPlacer.SetActive(false);
                buildingInstance = null;
                placingBuildingFlag = false;
                initBuilding = true;
                DeselectAll();
            }
        }
        else
        {
            if (Input.GetMouseButtonDown(0))
            {
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
                //if clicked on a building.
                else if (Physics.Raycast(ray, out hit, Mathf.Infinity, BuildingLayer))
                {
                    if (Physics.Raycast(ray, out hit))
                    {
                        // Check if the raycast hit a GameObject
                        GameObject hitObject = hit.collider.gameObject;
                        if (hitObject.GetComponent<Building>() != null)
                        {
                            BuildingManager.instance.whatBuilding = hitObject;
                            BuildingManager.instance.updateUI(hitObject.GetComponent<Building>().buildingType);
                        }
                    }
                }
            }
            if (Input.GetMouseButtonDown(1) && unitsSelected.Count > 0)
            {
                ray = cam.ScreenPointToRay(Input.mousePosition);

                if (Physics.Raycast(ray, out hit, Mathf.Infinity, UnitLayer) && cursorSet)
                {
                    foreach (GameObject obj in unitsSelected)
                    {
                        obj.GetComponent<Unit>().target = hit.collider.gameObject;
                    }
                }

                if (Physics.Raycast(ray, out hit, Mathf.Infinity, ground) && !cursorSet)
                {
                    groundMarker.transform.position = new Vector3(hit.point.x, hit.point.y + 2, hit.point.z);
                    foreach (GameObject obj in unitsSelected)
                    {
                        obj.GetComponent<Unit>().target = null;
                    }
                        groundMarker.SetActive(false);
                    groundMarker.SetActive(true);
                }

            }
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            DeselectAll();
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
        if (unit.GetComponent<Unit>().enemy) return;
        DeselectAll();
        SelectUnit(unit, true);
        unitsSelected.Add(unit);

    }

    private void MultiSelect(GameObject unit)
    {
        if (unit.GetComponent<Unit>().enemy) return;
        if (unitsSelected.Contains(unit) == false)
        {
            SelectUnit(unit, true);
            unitsSelected.Add(unit);

        }
        else
        {
            SelectUnit(unit, false);
            unitsSelected.Remove(unit);
        }
    }

    private void SelectUnit(GameObject unit, bool enabled)
    {
        if (unit.GetComponent<Unit>().enemy) return;
        if (unit.GetComponent<Light>() == null)
        {
            return;
        }
        if (enabled)
        {
            unit.GetComponent<Light>().enabled = true;
        }
        else
        {
            unit.GetComponent<Light>().enabled = false;
        }
        unit.GetComponent<unitMovement>().enabled = enabled;

        WhatUnit(unit);
    }
    internal void DragSelect(GameObject unit)
    {
        if (unit.GetComponent<Unit>().enemy) return;
        if (unitsSelected.Contains(unit) == false && unit.CompareTag("Unit"))
        {
                unitsSelected.Add(unit);
                SelectUnit(unit, true);
        }
    }
}
