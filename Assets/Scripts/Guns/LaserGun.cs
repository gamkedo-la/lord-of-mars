using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserGun : MonoBehaviour
{
    Transform cameraTransform;
    LayerMask bulletMask;
    public GameObject laserbeamPrefab;
    public Transform fireFrom;


    // Start is called before the first frame update
    void Start()
    {
        cameraTransform = Camera.main.transform;
        bulletMask = LayerMask.GetMask("Default", "TransparentFX", "Water", "Grapple", "Ground");
    }



    public void Shoot()
    {
        RaycastHit rhInfo;
        FindObjectOfType<AudioManager>().SoundPlay("MainGun");
        if (Physics.Raycast(cameraTransform.position, cameraTransform.forward, out rhInfo, 200.0f, bulletMask))
        {
            //Debug.Log(rhInfo.collider.name);
            Damageable hurtScript = rhInfo.collider.GetComponentInParent<Damageable>();
            FindObjectOfType<AudioManager>().SoundPlay("BulletHit");
            if (hurtScript)
            {
                hurtScript.TakeDamage(25.0f, cameraTransform.forward);
            }
            Instantiate(laserbeamPrefab, fireFrom.transform.position, Quaternion.identity);
        }
        //GameObject tempGO = GameObject.Instantiate(laserbeamPrefab, fireFrom.transform.position, fireFrom.transform.rotation);
    }

}
