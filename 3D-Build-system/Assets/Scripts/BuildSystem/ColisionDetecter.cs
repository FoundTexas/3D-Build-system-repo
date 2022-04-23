using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColisionDetecter : MonoBehaviour
{
    public bool hasColided;
    Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();

    }


    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.layer != LayerMask.NameToLayer("Ground"))
        {
            hasColided = true;
        }

    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer != LayerMask.NameToLayer("Ground"))
        {
            hasColided = false;
        }
    }
}
