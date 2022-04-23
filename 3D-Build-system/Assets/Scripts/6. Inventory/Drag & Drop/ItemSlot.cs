using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class ItemSlotData
{
    public int slotNumber, amount;
    public string itemID;
}

public class ItemSlot : MonoBehaviour
{
    public int slotNumber, amount;
    public string itemID;

    public void SetItem(int amount, string itemID)
    {
        this.amount = amount;
        this.itemID = itemID;
    }

    public bool isEmpty()
    {
        return amount <= 0;
    }
}
