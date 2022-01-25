using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

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
    public ParticleSystem flash;

    public GameObject gameOverMenu;

    public List<GameObject> weaponList;
    private int weaponSelected = 0;

    public float grappleStrength = 50f;
    public float grappleDecay = 0.95f;
    public Text cursor;
    private float grappleDistance = 80f;
    private GameObject currentGrapplePoint = null;
    public LineRenderer grappleBeam;

    private AmmoCount ammoCount;

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

    [SerializeField] GameObject parentGameObject;
    private Vector3 startingPosition;

    private void Awake()
    {
        gameOverMenu.SetActive(false);
    }


    private void Start()
    {
        mLook = GetComponentInChildren<MouseLook>();
        print(mLook);
        Cursor.lockState = CursorLockMode.Locked; //doesn't work currently
        CameraTransform = GameObject.FindWithTag("MainCamera").transform;
        startingPosition = gameObject.transform.position;
        ShowOnlyActiveWeapon();
        ammoCount = GetComponent<AmmoCount>();
    }
    
    // Update is called once per frame
    void Update()
    {
        //Debug.Log("gameObject.transform.position.y: " + gameObject.transform.position.y);
        if (gameObject.transform.position.y < -2.0f)
        {
            //Debug.Log("inside y position check");
            gameObject.transform.position = startingPosition;
        }
        RaycastHit rhInfo;
        float angleBelowUs = 90f;
        
        if (Physics.Raycast(transform.position, Vector3.down, out rhInfo, 4.0f))
        {
                angleBelowUs = Mathf.Acos(rhInfo.normal.y) * Mathf.Rad2Deg;
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
        HandleWeaponSwitch();


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
            
            // Disable grapple beam line renderer
            grappleBeam.gameObject.SetActive(false);
        }
        else
        {
            UpdateGrappleBeam();
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
            if (GameManagerScript.aTutorialPromptIsOn || GameManagerScript.gameIsPaused)
            {
                return;
            }
            if(ammoCount.HasAmmo())
            {
                weaponList[weaponSelected].SendMessage("Shoot");
                //may need to check if the gun is able to fire if reloading 
                ammoCount.UseAmmo();
            }
        }
    }

    private void UpdateGrappleBeam()
    {
        grappleBeam.gameObject.SetActive(true);
        grappleBeam.SetPositions(new Vector3[]
        {
            currentGrapplePoint.transform.position,
            fireFrom.position,
        });
    }

    private void HandleWeaponSwitch()
    {
        int wasSelected = weaponSelected;
        if(Input.GetKeyDown(KeyCode.Alpha1))
        {
            weaponSelected = 0;
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            weaponSelected = 1;
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            weaponSelected = 2;
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            weaponSelected = 3;
        }
        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            weaponSelected = 4;
        }
        if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            weaponSelected = 5;
        }
        if (Input.GetKeyDown(KeyCode.Alpha7))
        {
            weaponSelected = 6;
        }
        if (Input.GetKeyDown(KeyCode.Alpha8))
        {
            weaponSelected = 7;
        }
        if (Input.GetKeyDown(KeyCode.Alpha9))
        {
            weaponSelected = 8;
        }
        if (wasSelected != weaponSelected)
        {
            if(weaponSelected >= weaponList.Count)
            {
                Debug.Log("Invalid weapon selection");
                weaponSelected = wasSelected;
                return;
            }
            ShowOnlyActiveWeapon();
        }
    }

    void ShowOnlyActiveWeapon()
    {
        for (int i = 0; i < weaponList.Count; i++)
        {
            weaponList[i].SetActive(i == weaponSelected);
        }
    }


    public void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        Debug.Log("Reloading Scene");
    }

    public void ShowGameOverMenu()
    {
        gameOverMenu.SetActive(true);
    }

}
