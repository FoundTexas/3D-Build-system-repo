using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public static PlayerMovement inst;
    CharacterController controller;
    public float speed = 5, gravity = -9.8f, JumpHeight = 1;
    public Vector3 mov, velocity;

    //-----------------------------------------------------------------
    //Check if on Ground

    [Header("Player Grounded")]
    public float GroundedOffset = -0.14f;
    [Tooltip("The radius of the grounded check. Should match the radius of the CharacterController")]
    public float GroundedRadius = 0.5f;
    [Tooltip("What layers the character uses as ground")]
    public LayerMask GroundLayers;
    public bool GroundedCheck()
    {
        Vector3 spherePosition = new Vector3(transform.position.x, transform.position.y - GroundedOffset, transform.position.z);
        return Physics.CheckSphere(spherePosition, GroundedRadius, GroundLayers, QueryTriggerInteraction.Ignore);
    }
    //-----------------------------------------------------------------

    void Start()
    {
        controller = GetComponent<CharacterController>();

        if (inst == null)
        {
            inst = this;
        }
    }
    void Update()
    {
        if(InventoryManager.canMove()){
            Jump();
            Move();
        }
    }

    //-----------------------------------------------------------------

    public float GetPlayerYRotation()
    {
        return transform.eulerAngles.y;
    }

    //-----------------------------------------------------------------
    void Move()
    {
        if (controller.isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        float x = Input.GetAxisRaw("Horizontal");
        float z = Input.GetAxisRaw("Vertical");

        mov = transform.right * x + transform.forward * z;

        velocity.y += gravity * Time.deltaTime;

        controller.Move(((mov * speed) + (velocity)) * Time.deltaTime);
    }
    //-----------------------------------------------------------------

    void Jump()
    {
        // Jump
        if (Input.GetKeyDown("space") && controller.isGrounded)
        {
            // the square root of H * -2 * G = how much velocity needed to reach desired height
            velocity.y = Mathf.Sqrt(JumpHeight * -2f * gravity);
        }
    }

    //-----------------------------------------------------------------
}
