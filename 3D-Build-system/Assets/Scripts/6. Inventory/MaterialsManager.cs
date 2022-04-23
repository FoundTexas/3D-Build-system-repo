using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Item
{
    public string Id, type;
    public Sprite icon;
    public GameObject Object;
    public string[] returnIds;
}

public class MaterialsManager : MonoBehaviour
{
    public List<Item> items;

    public Item SearchForItemByID(string Id)
    {
        Item tmp = items.Find(x => x.Id.Contains(Id));
        Debug.Log(tmp.Id);
        return tmp;
    }
}
