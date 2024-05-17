using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;


public class UIManager : MonoBehaviour
{
    public static UIManager instance { get; set; }

    public Transform panel; // Reference to the panel whose children you want to iterate through
    public Dictionary<string, Sprite[]> UIButtonImages = new();
    public Dictionary<string, Sprite> SpriteDictionary = new();

    public Texture2D background;

    public GameObject[] buildings = new GameObject[4];
    public GameObject[] churchUnits = new GameObject[1];
    public GameObject[] blackSmithUnits = new GameObject[4];

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
    void Start()
    {
        Sprite backgroundSprite = Sprite.Create(background, new Rect(0, 0, background.width, background.height), Vector2.one * 0.5f);
        Sprite[] tempSpriteArray = new Sprite[18];
        for (int i = 0; i < tempSpriteArray.Length; i++)
        {
            tempSpriteArray[i] = backgroundSprite;
        }
        UIButtonImages.Add("None", tempSpriteArray);

        //init Settler UI buttons
        Sprite[] settlerUIArray = new Sprite[18];
        for (int i = 0; i < tempSpriteArray.Length; i++)
        {
            settlerUIArray[i] = backgroundSprite;
        }

        buildings[0] = GameObject.Find("Blacksmith_BlueTeam");
        settlerUIArray[0] = buildings[0].GetComponent<Unit>().Icon;

        buildings[1] = GameObject.Find("House_Level1_BlueTeam");
        settlerUIArray[1] = buildings[1].GetComponent<Unit>().Icon;

        buildings[2] = GameObject.Find("Church_BlueTeam");
        settlerUIArray[2] = buildings[2].GetComponent<Unit>().Icon;

        buildings[3] = GameObject.Find("House_Level1_BlueTeam");
        settlerUIArray[3] = buildings[3].GetComponent<Unit>().Icon;

        UIButtonImages.Add("Buildings", settlerUIArray);

        //init church UI buttons
        Sprite[] churchUIArray = new Sprite[18];
        for (int i = 0; i < tempSpriteArray.Length; i++)
        {
            churchUIArray[i] = backgroundSprite;
        }
        churchUnits[0] = GameObject.Find("peasant_2");
        churchUIArray[0] = churchUnits[0].GetComponent<Unit>().Icon;

        UIButtonImages.Add("ChurchTraining", churchUIArray);
        //init blacksmith UI buttons
        Sprite[] blackSmithUIArray = new Sprite[18];
        for (int i = 0; i < tempSpriteArray.Length; i++)
        {
            blackSmithUIArray[i] = backgroundSprite;
        }

        blackSmithUnits[0] = GameObject.Find("peasant_1");
        blackSmithUIArray[0] = blackSmithUnits[0].GetComponent<Unit>().Icon;

        blackSmithUnits[1] = GameObject.Find("king");
        blackSmithUIArray[1] = blackSmithUnits[1].GetComponent<Unit>().Icon;

        blackSmithUnits[2] = GameObject.Find("bow");
        blackSmithUIArray[2] = blackSmithUnits[2].GetComponent<Unit>().Icon;

        UIButtonImages.Add("blackSmithTraining", blackSmithUIArray);

        ChangeToNone();
    }

    public void ChangeToNone()
    {
        int childCount = panel.childCount;
        Sprite[] sprites = UIButtonImages["None"];

        for (int i = 0; i < childCount && i < sprites.Length; i++)
        {
            Transform child = panel.GetChild(i);
            child.GetComponent<Image>().sprite = sprites[i];
            child.GetComponent<Button>().enabled = false;
            child.GetComponent<UIButtons>().building = null;

            if(child.childCount > 1)
            {
                Destroy(child.GetChild(1).gameObject);
            }
        }
    }

    public void BuidlingMethods()
    {
        Debug.Log("Building Button clicked!");
        SelectionManager.instance.placingBuildingFlag = true;
    }

    public void ChurchUI()
    {
        int childCount = panel.childCount;
        Sprite[] sprites = UIButtonImages["ChurchTraining"];

        for (int i = 0; i < childCount && i < sprites.Length; i++)
        {
            Transform child = panel.GetChild(i);
            child.GetComponent<Image>().sprite = sprites[i];

            if (i > 0)
            {
                child.GetComponent<Button>().enabled = false;

            }
            else
            {
                child.GetComponent<Button>().enabled = true;
                child.GetComponent<UIButtons>().building = churchUnits[i];
            }
        }
    }

    public void ChangeToBuildings()
    {
        int childCount = panel.childCount;
        Sprite[] sprites = UIButtonImages["Buildings"];

        for (int i = 0; i < childCount && i < sprites.Length; i++)
        {
            Transform child = panel.GetChild(i);
            child.GetComponent<Image>().sprite = sprites[i];

            if (i > 3)
            {
                child.GetComponent<Button>().enabled = false;

            }
            else
            {
                child.GetComponent<Button>().enabled = true;
                child.GetComponent<UIButtons>().building = buildings[i];
            }
        }
    }

    public void ChangeToTrainingUnits()
    {
        int childCount = panel.childCount;
        Sprite[] sprites = UIButtonImages["blackSmithTraining"];

        for (int i = 0; i < childCount && i < sprites.Length; i++)
        {
            Transform child = panel.GetChild(i);
            child.GetComponent<Image>().sprite = sprites[i];

            if (i > 2)
            {
                child.GetComponent<Button>().enabled = false;

            }
            else
            {
                child.GetComponent<Button>().enabled = true;
                child.GetComponent<UIButtons>().building = blackSmithUnits[i];
            }
        }
    }


    public void ChangeToUnitTraining()
    {

    }

    void Update()
    {
    }
}
