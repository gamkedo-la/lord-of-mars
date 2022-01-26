using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserGun : MonoBehaviour
{
    Transform cameraTransform;
    LayerMask bulletMask;
    public Transform fireFrom;
    public GameObject laserBeam;
    private LineRenderer line;

    // Start is called before the first frame update
    void Start()
    {
        cameraTransform = Camera.main.transform;
        bulletMask = LayerMask.GetMask("Default", "TransparentFX", "Water", "Grapple", "Ground");
        laserBeam.SetActive(false);
        line = laserBeam.GetComponent<LineRenderer>();
    }

    void Update ()
    {
        line.SetPosition(0, fireFrom.position);
        UpdateLaserEnd();

    }

    public void Shoot()
    {
        laserBeam.SetActive(true);
        line.SetPosition(0, fireFrom.position);
        UpdateLaserEnd();
    }

    void UpdateLaserEnd()
    {

        RaycastHit rhInfo;
        if (Physics.Raycast(cameraTransform.position, cameraTransform.forward, out rhInfo, 200.0f, bulletMask))
        {
            //Debug.Log(rhInfo.collider.name);
            Damageable hurtScript = rhInfo.collider.GetComponentInParent<Damageable>();
            if (hurtScript)
            {
                hurtScript.TakeDamage(25.0f*Time.deltaTime, cameraTransform.forward);
            }
            line.SetPosition(1, rhInfo.point);
        } else
        {
            line.SetPosition(1, fireFrom.position + fireFrom.forward * 200.0f);

        }


    }

    public void StopShoot()
    {
        laserBeam.SetActive(false);


    }

}