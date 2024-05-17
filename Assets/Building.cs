using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class Building : MonoBehaviour
{

    public int maxGatherers;
    public int currentGatherers;
    public enum Type
    {
        House,
        Barracks,
        Tree,
        Rock,
        Church,
    }


    [SerializeField] private Queue<GameObject> unitQueue = new Queue<GameObject>();
    [SerializeField] private GameObject currentUnit;

    public Type buildingType;
    public GameObject plotToDelete;

    private Transform transform;

    public float maxY;
    public Boolean built;
    public Boolean buildingFlag;
    public float height;


    private Image rectangleImage;
    private GameObject UnitIcon;
    private GameObject count;
    private GameObject scrollbar;
    private Unit unit;

    private Coroutine fillCoroutine;
    private float targetWidth;

    // Start is called before the first frame update
    void Start()
    {
        transform = GetComponent<Transform>();
        unit = GetComponent<Unit>();
        if (transform.childCount > 1)
        {
            scrollbar = transform.GetChild(1).gameObject;
            rectangleImage = scrollbar.transform.GetChild(0).gameObject.GetComponent<Image>();
            UnitIcon = scrollbar.transform.GetChild(1).gameObject;
            count = scrollbar.transform.GetChild(2).gameObject;
        }

        height = GetComponent<MeshRenderer>().bounds.size.y;
        SelectionManager.instance.allUnitsList.Add(gameObject);
        targetWidth = GetComponent<MeshRenderer>().bounds.size.x;
        SetFilledImageWidth(0);
        UnitIcon.SetActive(false);
        count.SetActive(false);
    }


    void buildBuilding()
    {
        unit.health += currentGatherers * 1.5f;
        if (unit.maxHealth - unit.health < currentGatherers * 1.5)
        {
            plotToDelete.GetComponent<BuildingPlot>().hack = true;
            plotToDelete.GetComponent<SphereCollider>().radius = 0.1f;
        }
        if (unit.health > unit.maxHealth)
        {
            unit.health = unit.maxHealth;
            built = true;
            transform.position = new Vector3(transform.position.x, (transform.position.y + (float)((currentGatherers * 1.5) / height)), transform.position.z);

            CancelInvoke("buildBuilding");
            Destroy(plotToDelete);
            currentGatherers = 0;
            buildingFlag = false;
            return;
        }
        transform.position = new Vector3(transform.position.x, (transform.position.y + (float)((currentGatherers * 1.5) / height)), transform.position.z);
    }


    public void killBuilding()
    {
        GetComponent<Rigidbody>().isKinematic = false;
        Invoke("goodnightSweetPrince", 5.0f);
    }

    private void goodnightSweetPrince()
    {
        Destroy(gameObject);
    }

    public void EnqueueUnit(GameObject unitPrefab)
    {
        unitQueue.Enqueue(unitPrefab);
        if (currentUnit == null)
        {
            TrainNextUnit();
        }
    }

    private void TrainNextUnit()
    {
        if (unitQueue.Count > 0)
        {
            GameObject nextUnitPrefab = unitQueue.Dequeue();
                currentUnit = Instantiate(nextUnitPrefab, transform.GetChild(0).transform.position, Quaternion.identity);
                currentUnit.SetActive(false);
                StartFilling();
        }
        else
        {
            currentUnit = null;
        }
    }

    public void StartFilling()
    {
        if (fillCoroutine != null)
            StopCoroutine(fillCoroutine);


        fillCoroutine = StartCoroutine(FillCoroutine());
    }

    private IEnumerator FillCoroutine()
    {
        float elapsedTime = 0f;
        float startWidth = 0;

        UnitIcon.GetComponent<Image>().sprite = currentUnit.GetComponent<Unit>().Icon;
        UnitIcon.SetActive(true);
        count.SetActive(true);

        while (elapsedTime < currentUnit.GetComponent<Unit>().trainTime)
        {
            // Incrementally increase the fill amount over time
            float newWidth = Mathf.Lerp(startWidth, targetWidth, elapsedTime / currentUnit.GetComponent<Unit>().trainTime);
            SetFilledImageWidth(newWidth);

            count.GetComponent<TextMeshProUGUI>().text = (unitQueue.Count + 1).ToString();
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        SetFilledImageWidth(targetWidth);

        currentUnit.SetActive(true);
        Vector3 spawnLocation = transform.GetChild(0).position;
        currentUnit.GetComponent<NavMeshAgent>().SetDestination(spawnLocation + new Vector3(3, 0, 0));

        fillCoroutine = null;
        SetFilledImageWidth(0);
        UnitIcon.SetActive(false);
        count.SetActive(false);
        TrainNextUnit();
    }

    // Method to set the width of the filled image
    private void SetFilledImageWidth(float width)
    {
        rectangleImage.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, width);
    }

    private void OnDestroy()
    {
        SelectionManager.instance.allUnitsList.Remove(gameObject);
    }
}
