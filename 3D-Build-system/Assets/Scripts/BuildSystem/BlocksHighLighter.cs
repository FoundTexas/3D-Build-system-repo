using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*--------------------------------------------------------------------
 Author: Rodolfo León Gasca.
 Email: rolega01@gmai.com
 
 This Module checks the Raycast from the player´s sight and 
 change the renderer texture color if it is a CORKBRICK "cube"
--------------------------------------------------------------------
*/
public class BlocksHighLighter : MonoBehaviour
{
    public static Color normalColor = new Color(1, 1, 1), highlightColor = new Color(1, 0, 1);
    static GameObject LastHighlight;

    public BlocksHighLighter()
    {
        normalColor = new Color(1, 1, 1);
        highlightColor = new Color(1, 0, 1);
        LastHighlight = null;
    }

    public static void HeightLightBlock(RaycastHit hitinfo)
    {
        if (hitinfo.transform != null)
            {
            if (hitinfo.transform.gameObject.layer == LayerMask.NameToLayer("Cube"))
            {
                hitinfo.transform.gameObject.GetComponent<Renderer>().material.color = highlightColor;
                if (LastHighlight == null)
                {
                    LastHighlight = hitinfo.transform.gameObject;
                }
                else if (hitinfo.transform.gameObject != LastHighlight)
                {
                    LastHighlight.GetComponent<Renderer>().material.color = normalColor;
                    LastHighlight = hitinfo.transform.gameObject;
                }
            }
            else if (LastHighlight != null)
            {
                LastHighlight.GetComponent<Renderer>().material.color = normalColor;
            }
        }
        else if (LastHighlight != null)
        {
            LastHighlight.GetComponent<Renderer>().material.color = normalColor;
        }
    }
}
