using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserGun : MonoBehaviour
{
    public GameObject laserbeamPrefab;
    public Transform fireFrom;

    public void Shoot()
    {
        Debug.Log("laserbeam fire");
        GameObject tempGO = GameObject.Instantiate(laserbeamPrefab, fireFrom.transform.position, fireFrom.transform.rotation);
    }

}
