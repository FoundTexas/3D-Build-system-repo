using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System;

public class InventoryManager : MonoBehaviour
{

    public GameObject BuildUI, UIprefab;
    static GameObject CraftUI;
    
    public List<ItemSlot> slots = new List<ItemSlot>();
    bool isBuilding, isActive;

    MaterialsManager matM;

    //MockInfo
    public string dir;
 //----------------------------------------------------------------------

    void Start()
    {
        CraftUI = transform.GetChild(0).gameObject;
        for(int i = 0; i < slots.ToArray().Length; i++)
        {
            slots[i].slotNumber = i;
        }

        matM = GetComponent<MaterialsManager>();

        GetSlotsInfo();
        SetInventoryUI();
    }
    void Update()
    {
        if (Input.GetKeyDown("q"))
        {
            if (isActive)
            {
                isBuilding = !isBuilding;
                SetInventoryUI();
            }
        }
        if (Input.GetKeyDown("e"))
        {
            isActive = !isActive;
            SetInventoryUI();
        }
    }

    public static bool canMove()
    {
        return !CraftUI.activeInHierarchy;
    }

 //----------------------------------------------------------------------

    // This Function Activate and deactivate the Storage UI and Building UI depending if the player is building and if is accesing the UI
    void SetInventoryUI()
    {
        CraftUI.SetActive(isBuilding && isActive);
        BuildUI.SetActive(isActive);
    }

    void RefreshView()
    {
        foreach (var g in slots)
        {
            g.amount = 0;
            g.itemID = "";
            foreach (Transform h in g.gameObject.transform)
            {
                Destroy(g.gameObject.transform.GetChild(0).gameObject);
            }
        }
    }

    void GetSlotsInfo()
    { 

        string tmp = WebRequestForJason(); //tmp
        RefreshView();

        string txt = tmp;
        txt = txt.TrimStart('"', '{', 'd', 'a', 't', 'a', ':', '[');
        txt = "{\"" + txt;
        txt = txt.TrimEnd(']', ',',' ');
        string[] strs = txt.Split(new string[] { ", " }, StringSplitOptions.None);

        foreach (string s in strs)
        {
            Debug.Log(s);

            ItemSlotData tmpslot = JsonUtility.FromJson<ItemSlotData>(s);

            if (!tmpslot.itemID.Equals(""))
            {
                Transform parent = slots[tmpslot.slotNumber].gameObject.transform;

                Debug.Log(tmpslot.itemID);

                var tmpitem = matM.SearchForItemByID(tmpslot.itemID);

                if (tmpitem != null)
                {
                    DraggableComponent g = Instantiate(UIprefab, parent).GetComponent<DraggableComponent>();
                    slots[tmpslot.slotNumber].SetItem(tmpslot.amount, tmpslot.itemID);

                    g.preParent = parent;
                    g.item = tmpitem.Id;
                    g.amount = tmpslot.amount;

                    parent.GetComponent<DropItemSlot>().OnItemDropped(g);

                    g.GetComponent<Image>().sprite = tmpitem.icon;
                    g.transform.GetChild(1).GetComponent<Text>().text = g.amount.ToString();
                    g.name = g.item;
                }
            }

        }

    }

    //----------------------------------------------------------------------

    public void GiveItem(int amount, string id)
    {

        int slotindex = slots.FindIndex(x => x.itemID.Equals(id));

        if (slots[slotindex] != null)
        {
            slots[slotindex].amount++;
        }
        else
        {
            AddItem(amount, id);
        }

        SaveSlots();
    }

    public void GiveItem(int amount, string id, int slotindex)
    {

        if (slots[slotindex] != null)
        {
            slots[slotindex].amount++;
        }
        else
        {
            AddItem(amount, id);
        }

        SaveSlots();
    }

    void AddItem(int amount, string id)
    {
        ItemSlot tmpslot = slots.Find(x => x.itemID.Equals(""));

        tmpslot.SetItem(amount, id);

        Transform parent = slots[tmpslot.slotNumber].gameObject.transform;

        Debug.Log(tmpslot.itemID);

        var tmpitem = matM.SearchForItemByID(tmpslot.itemID);

        if (tmpitem != null)
        {
            DraggableComponent g = Instantiate(UIprefab, parent).GetComponent<DraggableComponent>();
            slots[tmpslot.slotNumber].SetItem(tmpslot.amount, tmpslot.itemID);

            g.preParent = parent;
            g.item = tmpitem.Id;
            g.amount = tmpslot.amount;

            parent.GetComponent<DropItemSlot>().OnItemDropped(g);

            g.GetComponent<Image>().sprite = tmpitem.icon;
            g.name = g.item;
        }
    }

    //----------------------------------------------------------------------

    string SlotsToJason()
    {
        string result = "";
        for (int i = 0; i < slots.ToArray().Length - 1; i++)
        {
            string json = JsonUtility.ToJson(slots[i]);
            result += json + ", ";
        }
        string json2 = JsonUtility.ToJson(slots[slots.ToArray().Length - 1]);
        result += json2;

        return result;
    }
    public void SaveSlots()
    {
        SaveTextFile();
    } 
    string WebRequestForJason()
    {
        dir = Application.dataPath + dir;//"/CorkbrickEurope/Scripts/6. Inventory/MockSaveData" + ".json";

        if (!File.Exists(dir)|| File.ReadAllText(dir) == "")
        {
            File.WriteAllText(dir, SlotsToJason());
        }
        return File.ReadAllText(dir);
    }
    void SaveTextFile()
    {
        if (File.Exists(dir))
        {
            File.WriteAllText(dir, SlotsToJason());
        }
    }
}