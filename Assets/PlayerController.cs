using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    public CharacterController controller;

    public float speed = 12f;
    public float gravity = -9.81f;
    public float jumpHeight = 4f;
    public float dashDistance = 50f;

    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;

    private float tapHoldTimeL = 0f;
    private const float MAX_HOLD_FOR_TAP = 0.3f;
    private float timeBetweenTapsL = 0.0f;
    private const float MAX_TIME_BETWEEN_TAPS_FOR_DASH = 0.5f;

    public GameObject teleportPointer;

    bool hasDoubleJumped = false;

    Vector3 velocity;
    bool isGrounded;

    // Update is called once per frame
    void Update()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if(isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
            hasDoubleJumped = false;
        }

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = transform.right * x + transform.forward * z;

        controller.Move(move * speed * Time.deltaTime);
        timeBetweenTapsL += Time.deltaTime;

        if(Input.GetKeyDown(KeyCode.A))
        {
            tapHoldTimeL = 0.0f;
        }
        else if (Input.GetKeyUp(KeyCode.A))
        {
            if(tapHoldTimeL < MAX_HOLD_FOR_TAP)
            {
                if(timeBetweenTapsL < MAX_TIME_BETWEEN_TAPS_FOR_DASH)
                {
                    Debug.Log("dash left");
                }
                timeBetweenTapsL = 0.0f;
            }
          
        }
        else
        {
            tapHoldTimeL += Time.deltaTime;
        }

        if (Input.GetKey(KeyCode.E))
        {
            teleportPointer.SetActive(true);
            RaycastHit rhInfo;
            if (Physics.Raycast(transform.position, Camera.main.transform.forward, out rhInfo, dashDistance))
            {
                Debug.Log(rhInfo.collider.gameObject.name);
                teleportPointer.transform.position = rhInfo.point;
                //transform.position = rhInfo.point;
            }
        }

        if(Input.GetKeyUp(KeyCode.E))
        {

            Debug.Log("teleport");
            controller.Move(teleportPointer.transform.position - transform.position - transform.forward);
            teleportPointer.SetActive(false);
        }

        if(Input.GetButtonDown("Jump") )
        {
            bool canJump = false;
            if (isGrounded)
            {
                canJump = true;
            }
            else if( hasDoubleJumped == false)
            {
                canJump = true;
                hasDoubleJumped = true;
            }
            if (canJump)
            {
                velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            }
        }

        velocity.y += gravity * Time.deltaTime;

        controller.Move(velocity * Time.deltaTime);
    }
}
