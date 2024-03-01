using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;



public class BuildingManager : MonoBehaviour
{
    public static BuildingManager instance { get; set; }
    public Dictionary<string, Sprite> BuildingToModel = new();

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

    public void updateBuildings()
    {
        foreach (var unit in SelectionManager.instance.allUnitsList)
        {
            Building buildingType = unit.GetComponent<Building>();
            if (buildingType != null)
            {
                switch (buildingType.buildingType)
                {
                    case Building.Type.House:
                        Debug.Log("House");
                        break;
                    case Building.Type.Barracks:
                        Debug.Log("Barracks");
                        UIManager.instance.ChangeToTrainingUnits();
                        break;
                }
            }
        }
    }

    public void PlaceBuilding(string building)
    {
        Debug.Log(building);
    }
    public void Update()
    {
        return;
    }
}
