using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DraggableComponent : MonoBehaviour, IPointerClickHandler, IDragHandler// IInitializePotentialDragHandler, IBeginDragHandler, IEndDragHandler
{
    public event Action<PointerEventData> OnBeginDragHandler;
    public event Action<PointerEventData> OnDragHandler;
    public event Action<PointerEventData, bool> OnEndDragHandler;
    public bool FollowCursor { get; set; } = true;

    public bool Selected;
    public Vector3 StartPosition;
    public bool CanDrag { get; set; } = true;

    private RectTransform rectTransform;
    private Canvas canvas;

    public Transform preParent;

    public string item;
    public int amount;

    public GameObject prefab;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvas = GetComponentInParent<Canvas>();
    }

    public void Update()
    {
        if (Selected)
        {
            Vector2 movePos;

            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                canvas.transform as RectTransform,
                Input.mousePosition, canvas.worldCamera,
                out movePos);

            Vector3 mousePos = canvas.transform.TransformPoint(movePos);
            transform.position = mousePos;

        }
    }

    public void SelectwithoutClickObject(Vector2 newstartpos, Transform newparent)
    {
        Debug.Log(newstartpos);
        Debug.Log(newparent);
        rectTransform.anchoredPosition = newstartpos;
        preParent = newparent;
        this.transform.parent = canvas.gameObject.transform;
        Selected = true;
        this.rectTransform.sizeDelta = new Vector2(111, 111);
    }

    DraggableComponent CreateItem(bool half)
    {
        DraggableComponent g = Instantiate(prefab, canvas.transform).GetComponent<DraggableComponent>();
        if (half)
        {
            int tmpamount = this.amount;
            this.amount = Mathf.RoundToInt(amount * 0.5f);

            g.item = item;
            g.amount = tmpamount - amount;
            g.preParent = preParent;
        }
        else if (!half)
        {
            g.item = item;
            g.amount = this.amount -1;

            this.amount = 1;
        }

        this.transform.GetChild(1).GetComponent<Text>().text = this.amount.ToString();

        g.GetComponent<Image>().sprite = this.GetComponent<Image>().sprite;
        g.transform.GetChild(1).GetComponent<Text>().text = g.amount.ToString();
        g.name = g.item;

        return g;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        var results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, results);

        if (eventData.button.ToString().Equals("Left")  || eventData.button.ToString().Equals("Right"))
        {
            Selected = !Selected;
        }

        Debug.Log(Selected);

        if (Selected)
        {

            if (eventData.button.ToString().Equals("Right"))
            {
                if (amount >= 2)
                {
                    DraggableComponent tmp = CreateItem(true);
                    tmp.SelectwithoutClickObject(StartPosition, preParent);
                    Selected = false;

                }
                else if (amount < 2)
                {

                    this.transform.parent = canvas.gameObject.transform;
                }
            }
            else if (eventData.button.ToString().Equals("Left"))
            {

                this.transform.parent = canvas.gameObject.transform;
            }

        }
        else if (!Selected)
        {
            if (eventData.button.ToString().Equals("Right"))
            {
                if (amount >= 2)
                {
                    var dcresult = new List<DraggableComponent>();
                    foreach(var obj in results)
                    {
                        DraggableComponent drag = null;
                        drag = obj.gameObject.GetComponent<DraggableComponent>();

                        if (drag != null)
                        {
                            dcresult.Add(drag);
                        }
                    }
                    DraggableComponent g = null;
                    g = dcresult.Find(x => x.item != item);

                    if(g!= null)
                    {
                        Debug.Log(g);
                    }

                    if (g == null)
                    {
                        DraggableComponent tmp = CreateItem(false);
                        CheckSlotCollision(results, this, eventData.button.ToString());
                        tmp.SelectwithoutClickObject(StartPosition, preParent);
                    }
                    else
                    {
                        Selected = true;
                    }
                }
                else
                {
                    CheckSlotCollision(results, this, eventData.button.ToString());
                }

            }
            else if (eventData.button.ToString().Equals("Left"))
            {

                CheckSlotCollision(results, this, eventData.button.ToString());

            }
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        var results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, results);
        if (eventData.button.ToString().Equals("Right"))
        {
            if (amount >= 2)
            {
                var dcresult = new List<DraggableComponent>();
                foreach (var obj in results)
                {
                    DraggableComponent drag = null;
                    drag = obj.gameObject.GetComponent<DraggableComponent>();

                    if (drag != null)
                    {
                        dcresult.Add(drag);
                    }
                }
                DraggableComponent g = null;
                g = dcresult.Find(x => x.item != item);

                if (g != null)
                {
                    Debug.Log(g);
                }

                if (g == null)
                {
                    DraggableComponent tmp = CreateItem(false);
                    CheckSlotCollision(results, this, eventData.button.ToString());
                    tmp.SelectwithoutClickObject(StartPosition, preParent);
                }
                else
                {
                    Selected = true;
                }
            }
            else
            {
                CheckSlotCollision(results, this, eventData.button.ToString());
            }

        }
        else if (eventData.button.ToString().Equals("Left"))
        {

            CheckSlotCollision(results, this, eventData.button.ToString());

        }
    }
    /*
        var results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, results);

        if (eventData.button.ToString().Equals("Right"))
        {
            if (amount >= 2)
            {
                DraggableComponent tmp = CreateItem(false);
                CheckSlotCollision(results, this, eventData.button.ToString());
                tmp.SelectwithoutClickObject(StartPosition, preParent);
            }
            else
            {
                CheckSlotCollision(results, this, eventData.button.ToString());
            }

        }
        else if (eventData.button.ToString().Equals("Left"))
        {

            CheckSlotCollision(results, this, eventData.button.ToString());

        }
    */

    void CheckSlotCollision(List<RaycastResult> results, DraggableComponent draggable, string type)
    {
        DropItemSlot dropArea = null;
        ItemSlot itemSlot = null;
        DraggableComponent other = null;

        foreach (var result in results)
        {
            dropArea = result.gameObject.GetComponent<DropItemSlot>();
            itemSlot = result.gameObject.GetComponent<ItemSlot>();



            if (dropArea != null)
            {
                if (itemSlot != null)
                {
                    if (type.Equals("Right"))
                    {
                        if (dropArea.CurrentItem == null || dropArea.CurrentItem.item.Equals(draggable.item))
                        {
                            dropArea.OnItemDropped(draggable);
                            break;
                        }
                        else
                        {
                            itemSlot = null;
                        }
                    }
                    else if (type.Equals("Left"))
                    {
                        dropArea.OnItemDropped(draggable);
                        return;
                    }
                }
            }
        }


        if (itemSlot == null)
        {
            Selected = true;
        }
        else
        {
            transform.parent = draggable.preParent;
            draggable.rectTransform.anchoredPosition = StartPosition;
        }
    }
}

    /*

    public void OnDrag(PointerEventData eventData)
    {
        Debug.Log(eventData.button);

        if (!CanDrag)
        {
            return;
        }

        OnDragHandler?.Invoke(eventData);

        if (FollowCursor)
        {
            rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (!CanDrag)
        {
            return;
        }

        var results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, results);

        DropItemSlot dropArea = null;
        ItemSlot itemSlot = null;

        foreach (var result in results)
        {
            dropArea = result.gameObject.GetComponent<DropItemSlot>();
            itemSlot = result.gameObject.GetComponent<ItemSlot>();

            if (dropArea != null)
            {
                if (itemSlot != null)
                {
                    dropArea.OnItemDropped(this);
                    return;
                }
            }

        }

        rectTransform.anchoredPosition = StartPosition;
        OnEndDragHandler?.Invoke(eventData, false);
    }*/
