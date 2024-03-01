using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{

    public enum Type
    {
        Settler,
        Warrior,
    }
    public Type unitType;
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
