using UnityEngine;

/*--------------------------------------------------------------------
 Author: Rodolfo León Gasca.
 Email: rolega01@gmai.com
 
 This Module checks the Raycast from the player´s sight and 
 And place the corresponding CORKBRICK "Cube"
--------------------------------------------------------------------
*/

public class BlocksPlacer : MonoBehaviour
{
    static int shiftVal = 1;

    public static int ClampRotation(float rotation)
    {
        int tmp = 0;
        if (rotation >= 315 || rotation <= 45) { tmp = 90; }
        else if (rotation >= 135 && rotation <= 225) { tmp = -90; }
        else if (rotation < 135 && rotation > 45) { tmp = 180; }
        else if (rotation > 225 && rotation < 315) { tmp = 0; }
        return tmp;
    }

    static bool CanPlace(GameObject g)
    {
        var child = g.transform.GetChild(0).GetComponent<BoxCollider>();
        var tmp = g.layer;
        g.layer = 2;

        var list = !Physics.CheckBox(child.center, child.size, g.transform.rotation, tmp);

        g.layer = tmp;
        return list;
    }
    static bool CanPlace(Vector3 v, float size, LayerMask rayMask)
    {

        var list = !Physics.CheckSphere(v, size, rayMask);

        return list;
    }

    public static void PlaceBlock(RaycastHit hitinfo, GameObject CorkBrick)
    {
        shiftVal = (Input.GetKey(KeyCode.LeftShift) ? -1 : 1);

        Debug.Log("PlaceBlock");
        if (hitinfo.transform != null)
        {
            bool hitingGround = hitinfo.transform.gameObject.layer == LayerMask.NameToLayer("Ground");

            string HitType = hitinfo.transform.tag;
            string BlockType = CorkBrick.transform.tag;

            var localnormal = hitinfo.transform.InverseTransformDirection(hitinfo.normal);
            var normal = hitinfo.normal;
            bool hitOnY = hitinfo.transform.position.y <= hitinfo.point.y;

            float rotation = PlayerMovement.inst.GetPlayerYRotation();

            Quaternion Tmprot = GetPlacementRotation(hitingGround, hitOnY, hitingGround ? rotation : ClampRotation(rotation),
                HitType, BlockType, normal, hitinfo.transform);

            Vector3 spawnpos = hitingGround ?
                new Vector3(hitinfo.point.x, hitinfo.point.y + normal.y / 2, hitinfo.point.z) :
                GetPlacementVector(hitingGround, hitOnY,Tmprot.y, HitType, BlockType, hitinfo.transform.position, hitinfo.transform.InverseTransformPoint(hitinfo.point), normal);

            if (BlockType.Equals("Cube") && HitType.Equals("Joint"))
            {
                spawnpos = hitinfo.transform.TransformPoint(spawnpos);
            }

            if (BlocksPreviewer.inst.canPlace)
            {
                GameObject g = Instantiate(
                CorkBrick,
                spawnpos,
                Tmprot
                );

                Quaternion r = Quaternion.LookRotation(g.transform.forward, g.transform.up * ((HitType.Equals("Joint") && BlockType.Equals("Cube")) ? (hitOnY ? -1 : 1) : shiftVal));
                g.transform.rotation = r;
            }
        }
    }

    public static Quaternion GetPlacementRotation(bool hitground, bool middle, float rotation, string hittype, string type, Vector3 normal, Transform curRotation)
    {
        var r = new Quaternion();
        Vector3 tmp = Vector3.zero;

        /*if (type.Equals("Joint"))
        {
            tmp = curRotation.rotation.eulerAngles;
            tmp.y += rotation;
        }*/
        if (!hitground)
        {
            if (normal.y == 0)
            {
                if (!middle)
                {
                    if (Input.GetKey(KeyCode.LeftControl))
                    {
                        r = Quaternion.LookRotation(normal, normal);
                    }
                    else
                    {
                        tmp = curRotation.eulerAngles;
                        //tmp.z += curRotation.eulerAngles.z;
                        r = Quaternion.Euler(tmp);
                    }
                    //r = Quaternion.Euler(tmp);
                    return r;
                }
                else if (middle)
                {
                    r = Quaternion.LookRotation(curRotation.up * -1, normal);
                    return r;
                }
            }
            else
            {
                //r = Quaternion.LookRotation(Vector3.up, normal);
                //tmp.x += curRotation.eulerAngles.x;
                //tmp.y += curRotation.eulerAngles.y;


                if (Input.GetKey(KeyCode.LeftControl))
                {
                    r = Quaternion.LookRotation(curRotation.right, curRotation.right);
                }
                else
                {
                    tmp = curRotation.eulerAngles;
                    r = Quaternion.Euler(tmp);
                }

                return r;
            }
        }
        else
        {
            tmp.y += rotation;
            return Quaternion.Euler(tmp);
        }

        return r;
    }
    public static Vector3 GetPlacementVector(bool hitground, bool middle, float rotation, string hittype, string type, Vector3 pos,Vector3 localpos, Vector3 normal)
    {
        Vector3 tmp = pos;
        if (type != hittype) //(type.Equals("Cube") && hittype.Equals("Joint"))
        {
            tmp = localpos;
        }

        tmp.x += normal.x;
        tmp.z += normal.z;
        tmp.y += normal.y;

        /*if (type != hittype)
        {
            tmp.y += normal.y / 2;
        }
        else
        {
            tmp.y += normal.y;
        }*/

        if (type != hittype)//(type.Equals("Cube") && hittype.Equals("Joint"))
            {
            tmp.y = Mathf.RoundToInt(tmp.y) + (middle ? -0.5f : +0.5f);
            if (tmp.x <= -0.1)
            {
                tmp.x = -1;
            }
            else if (tmp.x >= 0.1)
            {
                tmp.x = 1;
            }
            else if (tmp.x > -0.1 && tmp.x > -0.1)
            {
                tmp.x = 0;
            }

            if (tmp.z <= -0.1)
            {
                tmp.z = -1;
            }
            else if (tmp.z >= 0.1)
            {
                tmp.z = 1;
            }
            else if (tmp.z > -0.1 && tmp.z > -0.1)
            {
                tmp.z = 0;
            }
            tmp /= 5;
        }

        return tmp;
    }
}
