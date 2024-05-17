using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;


public class BuildingManager : MonoBehaviour
{
    public static BuildingManager instance { get; set; }
    public Dictionary<string, Sprite> BuildingToModel = new();
    public GameObject whatBuilding;
    
    public float wood;
    public float stone;

    public TextMeshProUGUI text; 

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

    public Boolean spendResources(GameObject unitObj)
    {
        Unit unit = unitObj.GetComponent<Unit>();
        if (wood - unit.woodCost >= 0 && stone - unit.stoneCost >= 0)
        {
            wood -= unit.woodCost;
            stone -= unit.stoneCost;
            return true;
        }
        return false;
    }

    public void updateUI(Building.Type buildingType)
    {
        switch (buildingType)
        {
            case (Building.Type.House):
                UIManager.instance.ChangeToNone();
                break;
            case (Building.Type.Barracks):
                UIManager.instance.ChangeToNone();
                UIManager.instance.ChangeToTrainingUnits();
                break;
            case (Building.Type.Church):
                UIManager.instance.ChangeToNone();
                UIManager.instance.ChurchUI();
                break;
        }

    }


    public void PlaceBuilding(string building)
    {
        Debug.Log(building);
    }
    public void Update()
    {
        text.text = ((int)wood).ToString() + "\n" + ((int)stone).ToString();
    }
}
