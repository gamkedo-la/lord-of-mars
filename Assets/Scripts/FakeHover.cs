using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FakeHover : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float hoverHeightDif = 0.3f;
        float bobRate = 3.0f;
        float hoverHeight = 1.3f;
        transform.localPosition = Vector3.up * hoverHeight + Vector3.up * Mathf.Cos(Time.timeSinceLevelLoad * bobRate) * hoverHeightDif;
    }
}
