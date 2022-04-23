using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlocksPreviewer : MonoBehaviour
{
    public static BlocksPreviewer inst; 
    static int shiftVal = 1;
    Color canPlaceColor = new Color(0, 1, 0, 0.1f);
    Color cantPlaceColor = new Color(1, 0, 0, 0.1f);

    public bool canPlace;

    public GameObject preview = null;
    GameObject Last = null;

    private void Start()
    {
        inst = this;
    }

    public void SetBlockPreview(bool change, RaycastHit hitinfo, GameObject prev)
    {
        if (change)
        {
            Debug.Log(change);
            if (preview != null)
            {
                Destroy(preview);
                Last = null;
            }
        }
        else if (hitinfo.transform != null)
        {

            if (prev == null && preview != null)
            {
                Destroy(preview);

            }
            else if (Last != null && prev != null)
            {

                if (Last == prev)
                {
                    PreviewBlock(hitinfo);
                }
                else if (Last != prev)
                {
                    if (preview != null)
                    {
                        Destroy(preview);
                    }
                    preview = Instantiate(prev);
                    preview.layer = 2;
                    preview.GetComponent<Renderer>().material.color = canPlaceColor;
                    foreach(BoxCollider box in preview.GetComponents<BoxCollider>())
                    {
                        box.isTrigger = true;
                    }
                    PreviewBlock(hitinfo);
                }
            }
            else if (Last == null && prev != null)
            {
                preview = Instantiate(prev);
                preview.layer = 2;
                preview.GetComponent<Renderer>().material.color = canPlaceColor;
                PreviewBlock(hitinfo);
            }

            Last = prev;
        }
        else if (hitinfo.transform == null)
        {
            if (preview != null)
            {
                Destroy(preview);
                Last = null;
            }
        }
        
    }

    private void PreviewBlock(RaycastHit hitinfo)
    {
        shiftVal = (Input.GetKey(KeyCode.LeftShift) ? -1 : 1);
        if (preview != null)
        {
            preview.layer = 2;
            
            foreach (BoxCollider box in preview.GetComponents<BoxCollider>())
            {
                box.isTrigger = true;
            }

            bool hitingGround = hitinfo.transform.gameObject.layer == LayerMask.NameToLayer("Ground");

            string HitType = hitinfo.transform.tag;
            string BlockType = preview.transform.tag;

            var localnormal = hitinfo.transform.InverseTransformDirection(hitinfo.normal);
            var normal = hitinfo.normal;
            bool hitOnY = hitinfo.transform.position.y < hitinfo.point.y;

            float rotation = PlayerMovement.inst.GetPlayerYRotation();

            Quaternion Tmprot = BlocksPlacer.GetPlacementRotation(hitingGround, hitOnY, hitingGround ? rotation : BlocksPlacer.ClampRotation(rotation),
                HitType, BlockType, normal, hitinfo.transform);

            Vector3 spawnpos = hitingGround ?
                new Vector3(hitinfo.point.x, hitinfo.point.y + normal.y / 2, hitinfo.point.z) :
                BlocksPlacer.GetPlacementVector(hitingGround,hitOnY, Tmprot.y, HitType, BlockType, hitinfo.transform.position, hitinfo.transform.InverseTransformPoint(hitinfo.point), normal);

            if (BlockType.Equals("Cube") && HitType.Equals("Joint"))
            {
                spawnpos = hitinfo.transform.TransformPoint(spawnpos);
            }

            preview.transform.position = spawnpos;
            preview.transform.rotation = Tmprot;
            Quaternion r = Quaternion.LookRotation(preview.transform.forward, preview.transform.up * ((HitType.Equals("Joint") && BlockType.Equals("Cube")) ? -1 : shiftVal));
            preview.transform.rotation = r;

            canPlace = !preview.GetComponent<ColisionDetecter>().hasColided;

            preview.GetComponent<Renderer>().material.color = canPlace ? canPlaceColor : cantPlaceColor;

        }
    }
}
