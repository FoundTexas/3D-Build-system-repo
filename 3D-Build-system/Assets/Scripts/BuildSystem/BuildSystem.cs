using UnityEngine;

public class BuildSystem : MonoBehaviour
{
    public Transform ShootPoint;
    public LayerMask rayMask;
    public bool canBuild = false;

    BuildInventorySelector BIS;

    public GameObject[] CorkBrick;//Temp
    public int index;//Temp

    private void Start()
    {
        ShootPoint = transform.GetChild(0).GetChild(0);
        BIS = FindObjectOfType<BuildInventorySelector>();
    }

    void Update()
    {
        Inputs();

        BlocksHighLighter.HeightLightBlock(GetRay());

    }

    RaycastHit GetRay()
    {
        RaycastHit tmp = new RaycastHit();
        if(Physics.Raycast(ShootPoint.position, ShootPoint.forward, out RaycastHit hitinfo, 4, rayMask))
        {
            tmp = hitinfo;
        }
        return tmp;
    }

    void Inputs()
    {
        TempFunc();

        if (Input.GetKeyDown("q"))
        {
            Debug.Log("build");
            
        }

        canBuild = BIS.canBuild();

        if (InventoryManager.canMove())
        {
            if (BIS.GetSlotGameObject() != null)
            {
                if (Input.GetMouseButtonDown(1))
                {
                    BlocksPlacer.PlaceBlock(GetRay(), BIS.GetSlotGameObject());
                }
                else if (Input.GetMouseButtonDown(0))
                {
                    BlocksDeleter.DeleteBlock(GetRay());
                }
            }

            BlocksPreviewer.inst.SetBlockPreview(!canBuild, GetRay(), BIS.GetSlotGameObject());
        }
    }

    void TempFunc()
    {
        if (Input.GetKeyDown("0"))
        {
            index = 0;
        }
        else if (Input.GetKeyDown("1"))
        {
            index = 1;
        }
        else if (Input.GetKeyDown("2"))
        {
            index = 2;
        }
        else if (Input.GetKeyDown("3"))
        {
            index = 3;
        }
        else if (Input.GetKeyDown("4"))
        {
            index = 4;
        }
        else if (Input.GetKeyDown("5"))
        {
            index = 5;
        }
        else if (Input.GetKeyDown("6"))
        {
            index = 5;
        }
        else if (Input.GetKeyDown("7"))
        {
            index = 7;
        }
    }
}


