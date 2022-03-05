using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate : MonoBehaviour
{
    Vector3 startPos;

    // Start is called before the first frame update
    void Start()
    {
        startPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        float bounceRate = 1.0f;
        float bounceHeight = 0.5f;
        float spinRate = 90f;
        transform.position = startPos + Vector3.up * Mathf.Cos(Time.timeSinceLevelLoad * bounceRate) * bounceHeight;
        transform.Rotate(Vector3.up, spinRate * Time.deltaTime);
    }
}
