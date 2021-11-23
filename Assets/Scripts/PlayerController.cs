using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public CharacterController controller;
    private MouseLook mLook;

    public float speed = 12f;
    public float gravity = -9.81f;
    public float jumpHeight = 4f;
    private float dashDistance = 20f;

    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;

    public Transform fireFrom;
    public GameObject flashParticle;
    public LayerMask bulletMask;
    public ParticleSystem flash;

    public float grappleStrength = 50f;
    public float grappleDecay = 0.95f;
    public Text cursor;
    private float grappleDistance = 80f;
    private GameObject currentGrapplePoint = null;

    private float timeSinceDash = 0.0f;
    private float tapHoldTimeL = 0f;
    private float timeBetweenTapsL = 0.0f;
    private float tapHoldTimeR = 0f;
    private float timeBetweenTapsR = 0.0f;
    private float tapHoldTimeF = 0f;
    private float timeBetweenTapsF = 0.0f;
    private float tapHoldTimeB = 0f;
    private float timeBetweenTapsB = 0.0f;

    private const float MAX_HOLD_FOR_TAP = 0.25f;
    private const float MAX_TIME_BETWEEN_TAPS_FOR_DASH = 0.3f;
    private const float MIN_TIME_BETWEEN_DASHES = 1.0f;

    private float wallRunGap = 2f;
    public LayerMask wallRunMask;

    private const int WALLRUNNING_SIDE_LEFT = -1;
    private const int WALLRUNNING_SIDE_NONE = 0;
    private const int WALLRUNNING_SIDE_RIGHT = 1;
    private int wallrunningSide = WALLRUNNING_SIDE_NONE;


    public GameObject teleportPointer;

    bool hasDoubleJumped = false;

    Vector3 velocity;
    bool isGrounded;

    private Transform CameraTransform;

    private void Start()
    {
        mLook = GetComponentInChildren<MouseLook>();
        print(mLook);
        Cursor.lockState = CursorLockMode.Locked; //doesn't work currently
        CameraTransform = GameObject.FindWithTag("MainCamera").transform;
    }


    // Update is called once per frame
    void Update()
    {
        RaycastHit rhInfo;
        float angleBelowUs = 90f;
        if (Physics.Raycast(transform.position, Vector3.down, out rhInfo, 4.0f))
        {
            angleBelowUs = Mathf.Acos(rhInfo.normal.y)*Mathf.Rad2Deg;
        }
        isGrounded = (angleBelowUs <= controller.slopeLimit);

        if(isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
            hasDoubleJumped = false;
        }
        if(wallrunningSide != WALLRUNNING_SIDE_NONE)
        {
            hasDoubleJumped = false;
        }
        mLook.cameraTilt = wallrunningSide;

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");
        bool runningForward = z >= 0.8f; //for Wallrunning
        Vector3 move = transform.right * x + transform.forward * z;

        controller.Move(move * speed * Time.deltaTime);
        timeSinceDash += Time.deltaTime;
        HandleLeftDash();
        HandleRightDash();
        HandleForwardDash();
        HandleBackwardsDash();
        HandleShoot();

        GameObject grappleTarget = null;
        if (Physics.Raycast(transform.position, CameraTransform.forward, out rhInfo, grappleDistance))
        {
            if(rhInfo.collider.gameObject.layer == LayerMask.NameToLayer("Grapple"))
            {
                grappleTarget = rhInfo.collider.gameObject;
            }
        }
        cursor.color = (grappleTarget != null ? Color.green : Color.red);
        if (Input.GetMouseButtonDown(1))
        {
            if (grappleTarget != null)
            {
                currentGrapplePoint = grappleTarget;
            }
            else
            {
                Debug.Log("grapple fail, play sound effect");
            }
        }
        if (Input.GetMouseButtonUp(1))
        {
            currentGrapplePoint = null;
        }

        if (currentGrapplePoint != null)
        {
            wallrunningSide = WALLRUNNING_SIDE_NONE; // Can't wallrun while grappling
            float distance = Vector3.Distance(currentGrapplePoint.transform.position, transform.position);
            if (distance > 5.0f)
            {
                velocity = (currentGrapplePoint.transform.position - transform.position).normalized * grappleStrength;
            }
            else
            {
                Vector3 relativePt = transform.InverseTransformPoint(currentGrapplePoint.transform.position);
                if(relativePt.z < 0.0f) //behind us
                {
                    currentGrapplePoint = null;
                }
            }
        }
        else //not grappling 
        {
            if (runningForward && Physics.Raycast(transform.position, -transform.right, out rhInfo, wallRunGap, wallRunMask))
            {
                // Debug.DrawLine(transform.position, transform.position + (-transform.right * wallRunGap), Color.red, 1f);
                // Debug.Log(rhInfo.collider.gameObject.name + " left");
                wallrunningSide = WALLRUNNING_SIDE_LEFT;
            }

            else if (runningForward && Physics.Raycast(transform.position, transform.right, out rhInfo, wallRunGap, wallRunMask))
            {
                // Debug.DrawLine(transform.position, transform.position + (transform.right * wallRunGap), Color.red, 1f);
                // Debug.Log(rhInfo.collider.gameObject.name + " right");
                wallrunningSide = WALLRUNNING_SIDE_RIGHT;
            }
            else
            {
                wallrunningSide = WALLRUNNING_SIDE_NONE;
            }

            if (Input.GetButtonDown("Jump"))
            {
                bool canJump = false;
                if (isGrounded) //|| wallrunningSide != WALLRUNNING_SIDE_NONE
                {
                    canJump = true;
                }
                else if (hasDoubleJumped == false)
                {
                    canJump = true;
                    hasDoubleJumped = true;
                }
                if (canJump)
                {
                    velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
                    isGrounded = false;
                    wallrunningSide = WALLRUNNING_SIDE_NONE;
                }
            } 
        }

    }

    private void FixedUpdate()
    {
        if(currentGrapplePoint == null)
        {
            velocity.x *= grappleDecay;
            velocity.z *= grappleDecay;
            if(wallrunningSide != WALLRUNNING_SIDE_NONE)
            {
                velocity.y = 0f;
            }
            else
            {
                velocity.y += gravity * Time.deltaTime; //only applying gravity when not grappling 
            }
        }
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

    private void HandleForwardDash()
    {
        timeBetweenTapsF += Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.W))
        {
            tapHoldTimeF = 0.0f;
        }
        else if (Input.GetKeyUp(KeyCode.W))
        {
            if (tapHoldTimeF < MAX_HOLD_FOR_TAP)
            {
                if (timeBetweenTapsF < MAX_TIME_BETWEEN_TAPS_FOR_DASH)
                {
                    Debug.Log("dash forward");
                    TeleDash(transform.forward);
                    timeBetweenTapsF = MAX_TIME_BETWEEN_TAPS_FOR_DASH; //prevent consecutive double taps
                }
                else
                {
                    timeBetweenTapsF = 0.0f;
                }
            }
        }
        else
        {
            tapHoldTimeF += Time.deltaTime;
        }
    }

    private void HandleBackwardsDash()
    {
        timeBetweenTapsB += Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.S))
        {
            tapHoldTimeB = 0.0f;
        }
        else if (Input.GetKeyUp(KeyCode.S))
        {
            if (tapHoldTimeB < MAX_HOLD_FOR_TAP)
            {
                if (timeBetweenTapsB < MAX_TIME_BETWEEN_TAPS_FOR_DASH)
                {
                    Debug.Log("dash backwards");
                    TeleDash(-transform.forward);
                    timeBetweenTapsB = MAX_TIME_BETWEEN_TAPS_FOR_DASH; //prevent consecutive double taps
                }
                else
                {
                    timeBetweenTapsB = 0.0f;
                }
            }
        }
        else
        {
            tapHoldTimeB += Time.deltaTime;
        }
    }


    private void HandleShoot()
    {
        if(Input.GetButtonDown("Fire1"))
        {
            RaycastHit rhInfo;
            if(Physics.Raycast(CameraTransform.position, CameraTransform.forward, out rhInfo, 200.0f, bulletMask))
            {
                Debug.Log(rhInfo.collider.name);
                Instantiate(flashParticle, rhInfo.point + rhInfo.normal * 0.1f, Quaternion.identity);
            }
            flash.Play();
        }
    }



}
