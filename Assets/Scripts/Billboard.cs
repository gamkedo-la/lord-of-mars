using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Billboard : MonoBehaviour
{
    private Transform cameraTransform;
    // Start is called before the first frame update
    void Start()
    {
        cameraTransform = Camera.main.transform;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        transform.LookAt(cameraTransform);
        // transform.rotation.SetLookRotation(-cameraTransform.forward);
        // transform.rotation = cameraTransform.rotation. * Quaternion.AngleAxis(180f, Vector3.up);
    }
}
