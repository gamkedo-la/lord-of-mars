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

    public void Shoot()
    {
        Debug.Log("laserbeam fire");
        laserBeam.SetActive(true);
        line.SetPosition(0, fireFrom.position);
        line.SetPosition(1, fireFrom.position+fireFrom.forward*10.0f);

        /*RaycastHit rhInfo;
        if (Physics.Raycast(cameraTransform.position, cameraTransform.forward, out rhInfo, 200.0f, bulletMask))
        {
            //Debug.Log(rhInfo.collider.name);
            Damageable hurtScript = rhInfo.collider.GetComponentInParent<Damageable>();
            if (hurtScript)
            {
                hurtScript.TakeDamage(25.0f, cameraTransform.forward);
            }
            Instantiate(laserbeamPrefab, rhInfo.point + rhInfo.normal * 0.1f, Quaternion.identity);
        }*/

    }
}