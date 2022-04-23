using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildInventorySelector : MonoBehaviour
{
    int selectedIndex = 0;
    public List<ItemSlot> Buildslots = new List<ItemSlot>();
    MaterialsManager matM;

    public bool canBuild()
    {
        return Buildslots[0].gameObject.activeInHierarchy;
    }

    private void Start()
    {
        matM = GetComponent<MaterialsManager>();
        SetIndex(selectedIndex);
    }

    private void Update()
    {
        Inputs();
    }

    public void SetIndex(int i)
    {
        foreach (var s in Buildslots)
        {
            s.GetComponent<CanvasGroup>().alpha = 0.5f;
        }
        selectedIndex = i;
        Buildslots[selectedIndex].GetComponent<CanvasGroup>().alpha = 1;
    }

    public GameObject GetSlotGameObject()
    {
        return Buildslots[selectedIndex].amount > 0 ? matM.SearchForItemByID(Buildslots[selectedIndex].itemID).Object : null;
    }

    void Inputs()
    {
        if (Input.inputString != "")
        {
            int number = 1;
            bool is_a_number = Int32.TryParse(Input.inputString, out number);
            if (is_a_number && number > 0 && number <= 7)
            {
                SetIndex(number-1);
            }
        }
        if (Input.mouseScrollDelta.y != 0)
        {
            selectedIndex -= Mathf.RoundToInt(Input.mouseScrollDelta.y);
            Debug.Log(Input.mouseScrollDelta.y);
            selectedIndex = Mathf.Clamp(selectedIndex, 0, 6);
            SetIndex(selectedIndex);
        }

    }
}
