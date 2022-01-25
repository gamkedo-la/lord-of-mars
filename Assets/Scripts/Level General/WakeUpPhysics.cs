using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WakeUpPhysics : MonoBehaviour
{

    public void TriggerActivate()
    {
        Debug.Log("Waking Up");
        Rigidbody rb = GetComponent<Rigidbody>();
        rb.isKinematic = false;
    }


}
