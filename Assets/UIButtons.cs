using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIButtons : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [SerializeField] private GameObject tooltipPrefab;
    [SerializeField] private GameObject unitToSpawn;
    [SerializeField] private float delayBeforeShowingTooltip = 1.0f;

    private GameObject currentTooltip;
    private bool isPointerInside;
    public GameObject building;
   
    public TextMeshProUGUI notice;
    public float fadeDuration = 2.0f;
    public float spawnDelay = 4.0f;

    private Color originalColor;
    private Coroutine fadeCoroutine;
    // Start is called before the first frame update
    void Start()
    {
        isPointerInside = false;
        originalColor = notice.color;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        // Instantiate the tooltip prefab
        if (building != null)
        {
            isPointerInside = true;
            Invoke("ShowTooltip", delayBeforeShowingTooltip);
        }
    }

    private void ShowTooltip()
    {
        if (isPointerInside)
        {
            if (currentTooltip == null)
            {
                if (building == null) return;
                currentTooltip = Instantiate(tooltipPrefab, transform.position, Quaternion.identity);
                currentTooltip.transform.SetParent(transform, true);

                RectTransform tooltipRectTransform = currentTooltip.GetComponent<RectTransform>();
                float shiftAmount = tooltipRectTransform.rect.width / 1.2f;
                tooltipRectTransform.anchoredPosition -= new Vector2(shiftAmount, 0f);

                //currentTooltip.transform.SetParent(transform.parent, true);
                GameObject textChild = currentTooltip.transform.GetChild(0).gameObject;
                Unit buildingScript = building.GetComponent<Unit>();

                textChild.GetComponent<TextMeshProUGUI>().text = buildingScript.name;

                textChild = currentTooltip.transform.GetChild(3).gameObject;
                string costs = "Cost: " + ((int)buildingScript.woodCost).ToString();
                textChild.GetComponent<TextMeshProUGUI>().text = costs;

                textChild = currentTooltip.transform.GetChild(4).gameObject;
                textChild.GetComponent<TextMeshProUGUI>().text = ((int)buildingScript.stoneCost).ToString();


                textChild = currentTooltip.transform.GetChild(5).gameObject;
                textChild.GetComponent<TextMeshProUGUI>().text = buildingScript.toolTip;
            }
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (building != null)
        {
            isPointerInside = false;
            DestroyTooltip();
        }
    }
    private void DestroyTooltip()
    {
        if (currentTooltip != null)
        {
            Destroy(currentTooltip);
            currentTooltip = null;
        }
    }


    public void OnPointerClick(PointerEventData eventData)
    {
        if (building == null) return;
        Unit unitScript = building.GetComponent<Unit>();
        if (unitScript.stoneCost <= BuildingManager.instance.stone &&
            unitScript.woodCost <= BuildingManager.instance.wood)
        {
            if (unitScript.unitType == Unit.Type.Building)
            {
                SelectionManager.instance.placingBuildingFlag = true;
                SelectionManager.instance.initBuilding = true;
                SelectionManager.instance.buildingInstance = null;
                SelectionManager.instance.buildingToPlace = building;
            }
            if (unitScript.trainable)
            {
                if (unitToSpawn == null)
                {
                    Building buildingScript = BuildingManager.instance.whatBuilding.GetComponent<Building>();
                    if (!BuildingManager.instance.spendResources(building))
                    {
                        notice.text = "Insufficient Resources!";

                        if (fadeCoroutine != null)
                        {
                            StopCoroutine(fadeCoroutine);
                        }
                        fadeCoroutine = StartCoroutine(FadeOutAndResetCoroutine());
                    }
                    buildingScript.EnqueueUnit(building);
                }
            }
        }
        else
        {
            notice.text = "Insufficient Resources!";

            if (fadeCoroutine != null)
            {
                StopCoroutine(fadeCoroutine);
            }
            fadeCoroutine = StartCoroutine(FadeOutAndResetCoroutine());
        }
    }


    private IEnumerator FadeOutAndResetCoroutine()
    {
        float elapsedTime = 0;

        while (elapsedTime < fadeDuration)
        {
            float alpha = Mathf.Lerp(1f, 0f, elapsedTime / fadeDuration);
            notice.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Ensure the text is completely faded out
        notice.color = new Color(originalColor.r, originalColor.g, originalColor.b, 0f);

        // Reset text color to original color
        notice.color = originalColor;
        notice.text = "";

    }
        // Update is called once per frame
    void Update()
    {
        
    }
}
