using System;
using UnityEngine;
using UnityEngine.UI;

public class DropItemSlot : MonoBehaviour
{
	public DraggableComponent CurrentItem = null;

	public void Initialize(DraggableComponent currentItem)
	{
		if (currentItem == null)
		{
			Debug.LogError("Tried to initialize the slot with an null item!");
			return;
		}

		OnItemDropped(currentItem);
	}

	public void OnItemDropped(DraggableComponent draggable)
	{
        draggable.gameObject.SetActive(true);
        DropItemSlot dragDropItem = draggable.preParent.GetComponent<DropItemSlot>();
        ItemSlot dragslot = draggable.preParent.GetComponent<ItemSlot>();
        ItemSlot Curslot = GetComponent<ItemSlot>();

        if (dragDropItem.transform.childCount <= 0)
        {
            dragslot.SetItem(0, "");
            dragDropItem.CurrentItem = null;
        }

        if (CurrentItem != null) { 

            if (CurrentItem.item.Equals(draggable.item))
            {
                draggable.amount += CurrentItem.amount;
                draggable.transform.GetChild(1).GetComponent<Text>().text = draggable.amount.ToString();
                Destroy(CurrentItem.gameObject);
            }
            else if (CurrentItem != draggable)
            {
                CurrentItem.SelectwithoutClickObject(draggable.StartPosition,draggable.preParent);
                dragslot.SetItem(CurrentItem.amount, CurrentItem.item);
                dragDropItem.CurrentItem = CurrentItem;
                /*
                CurrentItem.transform.position = dragDropItem.transform.position;
                CurrentItem.transform.parent = dragDropItem.transform;
                dragslot.SetItem(tmp.amount,tmp.item);
                */
            }
        }

        draggable.transform.position = transform.position;
        draggable.transform.parent = this.transform;
        draggable.preParent = this.transform;

        CurrentItem = draggable;

            
        Curslot.SetItem(CurrentItem.amount, CurrentItem.item);
	}
}
