using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    public CharacterController controller;

    public float speed = 12f;
    public float gravity = -9.81f;
    public float jumpHeight = 4f;
    private float dashDistance = 20f;

    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;

    private float timeSinceDash = 0.0f;
    private float tapHoldTimeL = 0f;
    private float timeBetweenTapsL = 0.0f;
    private float tapHoldTimeR = 0f;
    private float timeBetweenTapsR = 0.0f;
    private const float MAX_HOLD_FOR_TAP = 0.25f;
    private const float MAX_TIME_BETWEEN_TAPS_FOR_DASH = 0.3f;
    private const float MIN_TIME_BETWEEN_DASHES = 1.0f;
    

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
        timeSinceDash += Time.deltaTime;
        HandleLeftDash();
        HandleRightDash();

        if (Input.GetButtonDown("Jump") )
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

    private void TeleDash(Vector3 dir)
    {
        if(timeSinceDash < MIN_TIME_BETWEEN_DASHES)
        {
            return;
        }
        timeSinceDash = 0.0f;
        //teleportPointer.SetActive(true);
        RaycastHit rhInfo;
        if (Physics.Raycast(transform.position, dir, out rhInfo, dashDistance))
        {
            Debug.Log(rhInfo.collider.gameObject.name);
            teleportPointer.transform.position = rhInfo.point;
            //transform.position = rhInfo.point;
        }
        else
        {
            teleportPointer.transform.position = transform.position + dir * dashDistance;
        }
        controller.Move(teleportPointer.transform.position - transform.position - dir);
        //teleportPointer.SetActive(false);
    }

    private void HandleLeftDash()
    {
        timeBetweenTapsL += Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.A))
        {
            tapHoldTimeL = 0.0f;
        }
        else if (Input.GetKeyUp(KeyCode.A))
        {
            if (tapHoldTimeL < MAX_HOLD_FOR_TAP)
            {
                if (timeBetweenTapsL < MAX_TIME_BETWEEN_TAPS_FOR_DASH)
                {
                    Debug.Log("dash left");
                    TeleDash(-transform.right);
                    timeBetweenTapsL = MAX_TIME_BETWEEN_TAPS_FOR_DASH; //prevent consecutive double taps
                }
                else
                {
                    timeBetweenTapsL = 0.0f;
                }
            }
        }
        else
        {
            tapHoldTimeL += Time.deltaTime;
        }
    }

    private void HandleRightDash()
    {
        timeBetweenTapsR += Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.D))
        {
            tapHoldTimeR = 0.0f;
        }
        else if (Input.GetKeyUp(KeyCode.D))
        {
            if (tapHoldTimeR < MAX_HOLD_FOR_TAP)
            {
                if (timeBetweenTapsR < MAX_TIME_BETWEEN_TAPS_FOR_DASH)
                {
                    Debug.Log("dash right");
                    TeleDash(transform.right);
                    timeBetweenTapsR = MAX_TIME_BETWEEN_TAPS_FOR_DASH; //prevent consecutive double taps
                }
                else
                {
                    timeBetweenTapsR = 0.0f;
                }
            }
        }
        else
        {
            tapHoldTimeR += Time.deltaTime;
        }
    }

}
