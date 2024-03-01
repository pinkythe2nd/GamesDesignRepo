using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : MonoBehaviour
{

    public enum Type
    {
        House,
        Barracks,
    }
    public Type buildingType;
    // Start is called before the first frame update
    void Start()
    {
        SelectionManager.instance.allUnitsList.Add(gameObject);
    }

    private void OnDestroy()
    {
        SelectionManager.instance.allUnitsList.Remove(gameObject);
    }
}
