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
    public string[] fileNameArray;


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
        Texture2D texture;
        for (int i = 0; i < fileNameArray.Length; i++)
        {
            texture = Resources.Load<Texture2D>("UIicons/" + fileNameArray[i]);
            if (texture == null)
            {
                Debug.LogError("Texture " + fileNameArray[i] + " not found!");
                continue;
            }

            Sprite sprite = Sprite.Create(texture,
                new Rect(0, 0, texture.width, texture.height), Vector2.zero);

            SpriteDictionary.Add(fileNameArray[i], sprite);

        }

        Sprite[] tempSpriteArray = new Sprite[18];
        for (int i = 0; i < tempSpriteArray.Length; i++)
        {
            tempSpriteArray[i] = SpriteDictionary["backgrounds/bg_black_frame"];
        }
        UIButtonImages.Add("None", tempSpriteArray);

        //init building buttons
        Sprite[] buildingSpriteArray = new Sprite[18];
        for (int i = 0; i < tempSpriteArray.Length; i++)
        {
            buildingSpriteArray[i] = SpriteDictionary["backgrounds/bg_black_frame"];
        }
        buildingSpriteArray[0] = SpriteDictionary["items/arrow_basic"];
        buildingSpriteArray[1] = SpriteDictionary["items/bone_skull"];
        buildingSpriteArray[2] = SpriteDictionary["items/bone_white"];
        buildingSpriteArray[3] = SpriteDictionary["items/book_closed_red"];
        UIButtonImages.Add("Buildings", buildingSpriteArray);

        //init training unit buttons
        Sprite[] unitSpriteArray = new Sprite[18];
        for (int i = 0; i < tempSpriteArray.Length; i++)
        {
            unitSpriteArray[i] = SpriteDictionary["backgrounds/bg_black_frame"];
        }
        unitSpriteArray[0] = SpriteDictionary["items/sword_basic4_blue"];
        unitSpriteArray[1] = SpriteDictionary["items/fish_green"];
        unitSpriteArray[2] = SpriteDictionary["items/sword_basic_blue"];
        unitSpriteArray[3] = SpriteDictionary["items/bow_wood1"];
        UIButtonImages.Add("BarracksTraining", unitSpriteArray);



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
        }
    }

    public void BuidlingMethods()
    {
        Debug.Log("Building Button clicked!");
        SelectionManager.instance.placingBuildingFlag = true;
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
                child.GetComponent<UIButtons>().functionPointer = BuidlingMethods;
            }
        }
    }

    public void TrainingMethods()
    {
        Debug.Log("Training....");
    }
    public void ChangeToTrainingUnits()
    {
        int childCount = panel.childCount;
        Sprite[] sprites = UIButtonImages["BarracksTraining"];

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
                child.GetComponent<UIButtons>().functionPointer = TrainingMethods;
            }
        }
    }


    public void ChangeToUnitTraining()
    {

    }

    public void IterateChildren()
    {
        int childCount = panel.childCount;
    }

    // Update is called once per frame
    void Update()
    {
    }
}
