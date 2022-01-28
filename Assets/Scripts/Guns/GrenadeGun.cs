using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrenadeGun : MonoBehaviour
{
    public GameObject grenadePrefab;
    public Transform fireFrom;
    private float grenadeForce = 1500.0f;

    public void Shoot()
    {
        AudioManager.instance.SoundPlay("GrenadeLaunch");
        GameObject tempGO = GameObject.Instantiate(grenadePrefab, fireFrom.transform.position, fireFrom.transform.rotation);
        Rigidbody rb = tempGO.GetComponent<Rigidbody>();
        rb.AddForce(fireFrom.transform.forward * grenadeForce);
    }

}
