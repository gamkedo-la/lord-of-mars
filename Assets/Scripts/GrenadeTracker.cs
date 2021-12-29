using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrenadeTracker : MonoBehaviour
{
    public Transform grenadeTracked;
    float angTo = 0.0f;
    Camera cam;

    private void Start()
    {
        cam = Camera.main;
    }




    // Update is called once per frame
    void Update()
    {
        Vector3 usToTarget = cam.transform.InverseTransformPoint(grenadeTracked.position);
        angTo = Mathf.Atan2(usToTarget.z, usToTarget.x) *Mathf.Rad2Deg;
        transform.localRotation = Quaternion.AngleAxis(angTo, Vector3.forward);
    }
}
