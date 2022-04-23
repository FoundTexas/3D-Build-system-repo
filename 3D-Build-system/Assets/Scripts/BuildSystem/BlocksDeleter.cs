using UnityEngine;
/*--------------------------------------------------------------------
 Author: Rodolfo León Gasca.
 Email: rolega01@gmai.com
 
  This Function checks the Raycast from the player´s sight and 
  Delete the object if it is a CORKBRICK
--------------------------------------------------------------------
*/

public class BlocksDeleter : MonoBehaviour
{
    public static void DeleteBlock(RaycastHit hitinfo)
    {
        if (hitinfo.transform != null)
        {
            if (hitinfo.transform.gameObject.layer != LayerMask.NameToLayer("Ground"))
            {
                Destroy(hitinfo.transform.gameObject);
            }
        }
    }

}
